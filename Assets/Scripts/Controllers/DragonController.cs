using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class DragonController : EnemyController
    {
        public float bulletForce = 20f;
        public DragonState state;
        
        private float _lastShot;
        private float _lastStateChanged;
        
        private static readonly int MustLand = Animator.StringToHash("mustLand");
        private static readonly int WalkStateTransition = Animator.StringToHash("walkStateTransition");
        private static readonly int ShouldWalk = Animator.StringToHash("shouldWalk");

        private const float WaitTime = 0.5f;
        private const float DelayTimeState = 10;

        public enum DragonState
        {
            Walking,
            Shooting,
            Flying
        }

        private void Start()
        {
            _lastShot = Time.time;
            _lastStateChanged = Time.time;
            Anim = GetComponent<Animator>();
            state = DragonState.Walking;
            
            enemyStats = GetComponent<DragonStats>();
        }

        private void FixedUpdate()
        {
            if (GameManager.CurrentStatus != GameManager.GameStatus.Playing) return;
            
            if (Time.time - _lastStateChanged > DelayTimeState)
            {
                _lastStateChanged = Time.time;
                
                if (state is DragonState.Walking)
                {
                    state = DragonState.Flying;
                    Anim.SetInteger(WalkStateTransition, 1);
                    Anim.SetBool(MustLand, false);
                    GetComponent<BoxCollider>().enabled = false;
                }
                else if (state is DragonState.Flying)
                {
                    state = DragonState.Walking;
                    Anim.SetBool(MustLand, true);
                    Anim.SetInteger(WalkStateTransition, 0);
                    StartCoroutine(WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length + 3, () =>
                    {
                        GetComponent<BoxCollider>().enabled = true;
                    }));
                }
            }
            
            var dragonPos = transform.position;
            dragonPos.y = 0;
            var playerPos = _playerPosition;
            playerPos.y = 0;
            var dist = Vector3.Distance(dragonPos, playerPos);
            if (dist < 15 && state != DragonState.Shooting)
            {
                
                if (state is DragonState.Flying)
                {
                    Anim.SetBool(MustLand, true);
                    
                    StartCoroutine(WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length + 3, () =>
                    {
                        GetComponent<BoxCollider>().enabled = true;
                    }));

                    StartCoroutine(WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length, () =>
                    {
                        state = DragonState.Shooting;
                    }));
                }
                else
                {
                    state = DragonState.Shooting;
                }
                Anim.SetBool(ShouldWalk, false);
                Anim.SetInteger(WalkStateTransition, 2);
            } 
            else if (dist >= 15 && state == DragonState.Shooting)
            {
                state = DragonState.Walking;
                Anim.SetInteger(WalkStateTransition, 0);
                Anim.SetBool(ShouldWalk, true);
            }

            switch (state)
            {
                case DragonState.Flying:
                    MoveTowardsPlayer(((DragonStats)enemyStats).FlyingMovementSpeed);
                    break;
                case DragonState.Walking:
                    MoveTowardsPlayer();
                    break;
                case DragonState.Shooting:
                    TurnTowardsPlayer();
                    Shoot();
                    break;
            }
        }
        
        private bool Cooldown()
        {
            if (Time.fixedTime < _lastShot + WaitTime)
            {
                return false;
            }
            
            _lastShot = Time.fixedTime;
            return true;
        }

        private void Shoot()
        {
            if (!Cooldown() || !Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle01"))
            {
                return;
            }
            
            Anim.Play("Basic Attack");
            StartCoroutine(SyncBulletWithAnimation(Anim.GetCurrentAnimatorStateInfo(0).length / 2.5f));
        }

        private IEnumerator SyncBulletWithAnimation(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            var bullet = PistolBulletPool.instance.GetPooledObject();

            if (bullet == null) yield break;
            
            bullet.GetComponent<BulletController>().shooter = gameObject; 
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            var pos = transform.position;
            var y = pos.y;
            pos += enemyLookDirection * (bullet.GetComponent<BulletStats>().OffsetPositionFactor + 0.5f);
            pos.y = y + 0.7f;
            bullet.transform.position = pos;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bullet.SetActive(true);

            bullet.transform.rotation = Quaternion.LookRotation(enemyLookDirection);
            bullet.transform.Rotate(bullet.GetComponent<BulletStats>().OffsetRotation);

            rb.AddForce(enemyLookDirection * bulletForce, ForceMode.Impulse);
        }

        private static IEnumerator WaitForSeconds(float seconds, Action cb)
        {
            yield return new WaitForSeconds(seconds);
            cb();
        }
    }
}
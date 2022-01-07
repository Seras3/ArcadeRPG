using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class WizardController : EnemyController
    {
        public float bulletForce = 20f;
        
        private float _lastShot;
        private float _lastStateChanged;
        public WizardState state;
        
        private const float WaitTime = 3;
        private const float DelayTimeState = 7;

        public enum WizardState
        {
            Walking,
            Shooting,
        }

        private void Start()
        {
            _lastShot = Time.time;
            _lastStateChanged = Time.time;
            state = WizardState.Walking;
            
            enemyStats = GetComponent<WizardStats>();
        }

        private void Update()
        {
            if (Time.time - _lastStateChanged > DelayTimeState)
            {
                _lastStateChanged = Time.time;
                if (state is WizardState.Walking)
                {
                    state = WizardState.Shooting;
                    GetComponent<Animator>().Play("Idle");
                }
                else
                {
                    state = WizardState.Walking;
                    GetComponent<Animator>().Play("Walk");
                }
            }

            if (GameManager.CurrentStatus != GameManager.GameStatus.Playing) return;
            
            switch (state)
            {
                case WizardState.Walking:
                    MoveTowardsPlayer();
                    break;
                case WizardState.Shooting:
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
            if (!Cooldown()) return;

            var anim = GetComponent<Animator>();
            anim.Play("Attack01");
            
            StartCoroutine(SyncBulletWithAnimation(anim.GetCurrentAnimatorStateInfo(0).length / 2));
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
            pos += enemyLookDirection * bullet.GetComponent<BulletStats>().OffsetPositionFactor;
            pos.y = y + 1;
            bullet.transform.position = pos;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bullet.SetActive(true);

            bullet.transform.rotation = Quaternion.LookRotation(enemyLookDirection);
            bullet.transform.Rotate(bullet.GetComponent<BulletStats>().OffsetRotation);

            rb.AddForce(enemyLookDirection * bulletForce, ForceMode.Impulse);
        }
    }
}
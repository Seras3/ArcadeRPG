using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Stats;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //public GameObject bullet;
    private Weapon fromWeapon;
    public GameObject shooter;

    private float _offsetPosition;
    private Renderer _renderer;
    private Transform _playerTransform;
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int WalkStateTransition = Animator.StringToHash("walkStateTransition");

    public int Damage;
    void Start()
    {
        // _offsetPosition = GetComponent<BulletStats>().OffsetPosition;
        // _renderer = GetComponent<Renderer>();
        // _playerTransform = GameObject.Find("B-spine").transform;
        
        GetComponent<Collider>().isTrigger=enabled;
        // if (shooter.CompareTag("Enemy"))
        // {
        //     fromWeapon = shooter.GetComponentInParent<Stats.VampireStats>().weapon;
        // }
        // else 
        // {
        //     fromWeapon = shooter.GetComponent<Stats.PlayerStats>().Weapon;
        // }
    }

    // private void Update()
    // {
    //     if (!_renderer.enabled && Vector3.Distance(_playerTransform.position, transform.position) > _offsetPosition)
    //     {
    //         _renderer.enabled = true;
    //     }
    // }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject != shooter)
        {
            Damage = shooter.CompareTag("Enemy") ? shooter.GetComponent<EnemyStats>().Damage : GetComponent<BulletStats>().Damage;
            try
            {
                var charStats = other.gameObject.GetComponentInParent<CharacterStats>();
                if (charStats != null) {
                    charStats.TakeDamage(Damage);
                    var anim = other.gameObject.GetComponentInParent<Animator>();
                    if (anim.GetBool(IsDead) == false)
                    {
                        anim.Play("GetHit", 0);
                    }
                }
                else
                {
                    charStats = other.gameObject.GetComponent<CharacterStats>();
                    charStats.TakeDamage(Damage);
                    var anim = other.gameObject.GetComponent<Animator>();
                    if (anim.GetBool(IsDead) == false)
                    {
                        anim.Play("GetHit", 0);
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), shooter.GetComponent<Collider>(), false);
                gameObject.SetActive(false);
            }   
        }
    }
}

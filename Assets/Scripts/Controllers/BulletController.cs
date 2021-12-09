using System;
using System.Collections;
using System.Collections.Generic;
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

    public int Damage => GetComponent<BulletStats>().Damage;
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
            try
            {
                if (other.gameObject.GetComponentInParent<CharacterStats>() != null) {
                    other.gameObject.GetComponentInParent<CharacterStats>().TakeDamage(Damage);
                }
                else {
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(Damage);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private Weapon fromWeapon;
    private GameObject player;

    public int damage { get { return fromWeapon.Damage; } }

    void Start()
    {
        player = GameObject.Find("Dummy");
        fromWeapon = player.GetComponent<Stats.PlayerStats>().weapon;
        firePoint = player.transform;
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<CharacterController>(), true);
    }

    void OnCollisionEnter(Collision other) 
    { 
        if (other.transform != firePoint){
            Destroy (this.bullet);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    private Weapon fromWeapon;
    private GameObject shooter;

    public int damage { get { return fromWeapon.Damage; } }

    void Start()
    {
        GetComponent<Collider>().isTrigger=enabled;
        shooter = GameObject.Find("Dummy");
        fromWeapon = shooter.GetComponent<Stats.PlayerStats>().weapon;
        Physics.IgnoreCollision(GetComponent<Collider>(), shooter.GetComponent<CharacterController>(), true);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject != shooter)
        {
            try
            {
                other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
            }
            catch
            {
            }
            finally
            {
                Destroy (this.bullet);
            }    
        }
    }
}

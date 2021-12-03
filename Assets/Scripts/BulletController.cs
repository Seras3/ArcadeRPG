using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //public GameObject bullet;
    private Weapon fromWeapon;
    public GameObject shooter;

    public int damage { get { return fromWeapon.Damage; } }

    void Start()
    {
        GetComponent<Collider>().isTrigger=enabled;
        if (shooter.tag == "Enemy") {
            fromWeapon = shooter.GetComponentInParent<Stats.VampireStats>().weapon;
        }
        else {
            fromWeapon = shooter.GetComponent<Stats.PlayerStats>().weapon;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject != shooter)
        {
            try
            {
                if (other.gameObject.GetComponentInParent<CharacterStats>() != null) {
                    other.gameObject.GetComponentInParent<CharacterStats>().TakeDamage(damage);
                }
                else {
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
                }
            }
            catch
            {
            }
            finally
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), shooter.GetComponent<Collider>(), false);
                //Destroy (this.bullet);
                gameObject.SetActive(false);
            }    
        }
    }
}

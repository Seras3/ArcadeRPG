using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = GameObject.Find("Dummy").transform;
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Dummy").GetComponent<CharacterController>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) { 
        Debug.Log(other.gameObject.name);
        
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<EnemyController>().TakeDamage(5);
        }
        if (other.transform != firePoint){
            Destroy (this.bullet);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    void Start()
    {
        firePoint = GameObject.Find("Dummy").transform;
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Dummy").GetComponent<CharacterController>(), true);
    }

    void OnCollisionEnter(Collision other) 
    { 
        if (other.transform != firePoint){
            Destroy (this.bullet);
        }
    }
}

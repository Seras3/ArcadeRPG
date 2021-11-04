using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) { 
        Debug.Log(other.transform.tag);
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<Object>().TakeDamage(5);
        }
        
        Destroy (this.bullet);
    }
}

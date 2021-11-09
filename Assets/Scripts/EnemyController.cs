using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{    
    public float interpolant; 

    public Stats.EnemyStats enemyStats;
    
    void Start()
    { 
        interpolant = 0.1f;
        GetComponent<Collider>().isTrigger=enabled;

        enemyStats = GetComponent<Stats.EnemyStats>(); 
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;
        

        playerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, 
                                                 Vector3.Lerp(enemyPosition, playerPosition, interpolant), 
                                                 enemyStats.MovementSpeed);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Vector3 pushBack = (collider.gameObject.transform.position - transform.position).normalized;
            pushBack.y = 0;
            collider.gameObject.transform.position += pushBack;
        }

        if (collider.gameObject.name == "Dummy")
        {
            collider.gameObject.GetComponent<Stats.PlayerStats>().TakeDamage(enemyStats.Damage);
        }

        if (collider.gameObject.name == "Bullet(Clone)"){
            int damage = collider.gameObject.GetComponent<BulletController>().damage;
            enemyStats.TakeDamage(damage);

            Destroy(collider.gameObject);
        }
    }

}
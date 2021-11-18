using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private static bool IsFrozen { get; set; }
    
    public float interpolant; 

    public Stats.EnemyStats enemyStats;

    void Start()
    {
        IsFrozen = false;
        
        interpolant = 0.1f;

        enemyStats = GetComponent<Stats.EnemyStats>();
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (IsFrozen) return;
        
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;

        playerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, 
                                                 Vector3.Lerp(enemyPosition, playerPosition, interpolant), 
                                                 enemyStats.MovementSpeed);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 pushBack = (other.gameObject.transform.position - transform.position).normalized;
            pushBack.y = 0;
            other.gameObject.transform.position += pushBack;
        }

        if (other.gameObject.name == "Dummy")
        {
            other.gameObject.GetComponent<CharacterStats>().TakeDamage(enemyStats.Damage);
        }
    }
    
    public static void FreezeAll()
    {
        IsFrozen = true;
    }
}
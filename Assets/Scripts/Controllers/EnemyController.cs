using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


public class EnemyController : MonoBehaviour
{
    public float interpolant; 

    public Stats.EnemyStats enemyStats;

    void Start()
    { 
        enemyStats = GetComponent<Stats.EnemyStats>(); 
    }

    void Update()
    {
        //if (GameManager.CurrentStatus is GameManager.GameStatus.Playing) MoveTowardsPlayer();
    }

    protected void MoveTowardsPlayer()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;

        playerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, enemyStats.MovementSpeed);
    }

    protected void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.CompareTag("Enemy"))
        // {
        //     Vector3 pushBack = (other.gameObject.transform.position - transform.position).normalized;
        //     pushBack.y = 0;
        //     other.gameObject.transform.position += pushBack;
        // }
        //
        // if (other.gameObject.name == "Dummy")
        // {
        //     other.gameObject.GetComponent<CharacterStats>().TakeDamage(enemyStats.Damage);
        // }
    }
}
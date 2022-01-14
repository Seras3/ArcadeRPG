using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


public class EnemyController : MonoBehaviour
{
    public float interpolant; 

    public Stats.EnemyStats enemyStats;
    public Vector3 enemyLookDirection;

    protected Vector3 _playerPosition;

    protected Animator Anim;
    public float attackDebounceTime = 0.2f;
    private float lastAttackTime;
    
    private void Start()
    { 
        enemyStats = GetComponent<Stats.EnemyStats>();
        Anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (GameManager.CurrentStatus is GameManager.GameStatus.Playing &&
            Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            MoveTowardsPlayer();
        }
    }

    protected void MoveTowardsPlayer(float MovementSpeed = -1)
    {
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;

        playerPosition.y = enemyPosition.y;
        if (MovementSpeed != -1)
        {
            transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, MovementSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, enemyStats.MovementSpeed);
        }
        
        TurnTowardsPlayer();
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 pushBack = (other.gameObject.transform.position - transform.position).normalized;
            pushBack.y = 0;
            other.gameObject.transform.position += pushBack;
        }
        else if (other.gameObject.name == "Dummy")
        {
            if (Time.time - lastAttackTime > attackDebounceTime)
            {
                other.gameObject.GetComponent<CharacterStats>().TakeDamage(enemyStats.Damage);
                
                GetComponent<Animator>().Play("Attack02", 0);
                lastAttackTime = Time.time;
            }
        }
    }

    protected void TurnTowardsPlayer()
    {
        _playerPosition = GameObject.Find("B-spine").transform.position;
        if (_playerPosition == null) return;
        
        var lookPos = _playerPosition - transform.position;
        enemyLookDirection = lookPos.normalized;
        enemyLookDirection.y = 0;
        
        var rotation = Quaternion.LookRotation(enemyLookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }
}
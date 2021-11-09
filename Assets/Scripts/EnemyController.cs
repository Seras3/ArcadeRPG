using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    #region Variables
    // death support
    const float EnemyLifespanSeconds = 10;
    Timer deathTimer;

    public float t;
    public float speed;

    public GameObject enemy;
    public int hp;

    #endregion

    void Start()
    { 
        GetComponent<Collider>().isTrigger=enabled;

        // create and start timer
        hp = 10;
        deathTimer = gameObject.AddComponent<Timer>();
        deathTimer.Duration = EnemyLifespanSeconds;
        deathTimer.Run();
    }

    void Update()
    {
        if (hp <= 0) {
            Destroy(this.gameObject); 
        }
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;

        t = 0.1f;
        speed = 0.005f;
        
        playerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, Vector3.Lerp(enemyPosition, playerPosition, t), speed);
    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Vector3 pushBack = (collider.gameObject.transform.position - transform.position).normalized;
            pushBack.y = 0;
            collider.gameObject.transform.position += pushBack;
        }

        if (collider.gameObject.name == "Bullet(Clone)"){
            TakeDamage(5);
            Destroy(collider.gameObject);
        }
    }

}
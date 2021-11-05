using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A enemy
/// </summary>
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


    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    { 
        // create and start timer
        hp = 10;
        deathTimer = gameObject.AddComponent<Timer>();
        deathTimer.Duration = EnemyLifespanSeconds;
        deathTimer.Run(); 
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // destroy enemy if death timer finished
        /*if(deathTimer){
            if (deathTimer.Finished)
            {    
                Destroy(gameObject);       
            }
        }*/

        if (hp <= 0) {
            Destroy(this.gameObject); 
        }
        MoveTowardsPlayer();

    }

    void MoveTowardsPlayer()
    {
        Vector3 a = transform.position;
        Vector3 b = GameObject.Find("B-spine").transform.position;

        t = 0.1f;
        speed = 0.005f;

        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
    }

     public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
    }

}
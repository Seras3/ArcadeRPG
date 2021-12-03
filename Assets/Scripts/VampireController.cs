using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VampireController : MonoBehaviour
{    
    public float interpolant; 

    public Stats.EnemyStats enemyStats;
    private Vector3 vampireLookDirection;

    public GameObject vampire;
    public GameObject bat;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    private Transform bulletSpawned;
    private float waitTime = 3;
    private float lastShot;
    private float delayTimeState = 7;
    private float lastStateChange;
    public float bulletForce = 20f;
    private int state;
    
    private Renderer vampireMesh;
    private BoxCollider vampireBoxCollider;

    private Rigidbody vampireRB;
    private Renderer batMesh;

    private Vector3 playerPosition;
    
    void Start()
    { 
        interpolant = 0.1f;

        enemyStats = GetComponent<Stats.EnemyStats>(); 

        lastShot = Time.time;

        lastStateChange = Time.time;

        state = 1;

        vampireMesh = vampire.GetComponent<Renderer>();

        vampireBoxCollider = vampire.GetComponent<BoxCollider>();

        vampireRB = vampire.GetComponent<Rigidbody>();
        
        batMesh = bat.GetComponent<Renderer>();
        
        vampireMesh.enabled = false;
        vampireBoxCollider.enabled = false;
        vampireRB.detectCollisions = false;
        batMesh.enabled = true;
    }

    void Update()
    {
        vampire.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, vampire.gameObject.transform.position.y, this.gameObject.transform.position.z);

        // Debug.Log(this.gameObject.transform.position);
        // Debug.Log(vampire.gameObject.transform.position);


        playerPosition = GameObject.Find("B-spine").transform.position;

        if (playerPosition != null) 
        {
            var lookPos = playerPosition - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
        }

        if (Time.time - lastStateChange > delayTimeState)
        {
            lastStateChange = Time.time;

            if (state == 1) {
                vampireMesh.enabled = true;
                vampireBoxCollider.enabled = true;
                vampireRB.detectCollisions = true;
                batMesh.enabled = false;
                state = 2;
                vampire.transform.position = bat.transform.position - new Vector3(0, 1, 0);
            }
            else {
                vampireMesh.enabled = false;
                vampireBoxCollider.enabled = false;
                vampireRB.detectCollisions = false;
                batMesh.enabled = true;
                state = 1;
                bat.transform.position = vampire.transform.position + new Vector3(0, 1, 0);;
            }
        }

        if (state == 1) 
        {
            MoveTowardsPlayer();
        }
        

        if (state == 2) {
            Shoot();
        }
    }

    private bool Cooldown()
    {
        if (Time.fixedTime >= lastShot + waitTime)
        {
            lastShot = Time.fixedTime;
            return true;
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        Vector3 enemyPosition = transform.position;

        playerPosition.y = enemyPosition.y;
        transform.position = Vector3.MoveTowards(enemyPosition, 
                                                 Vector3.Lerp(enemyPosition, playerPosition, interpolant), 
                                                 enemyStats.MovementSpeed);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
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
    
    /*void Shoot() 
    {
        Vector3 playerPosition = GameObject.Find("B-spine").transform.position;
        GameObject bulletSpawned = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        Rigidbody rb = bulletSpawned.GetComponent<Rigidbody>();
        bulletSpawned.GetComponent<BulletController>().shooter = this.gameObject.transform.GetChild(0).gameObject;
        // rb.AddForce(bulletSpawned.transform.position , ForceMode.VelocityChange);
        rb.velocity = (playerPosition - rb.position).normalized * bulletForce;

    }*/

    void Shoot()
    {
        if (Cooldown())
        {
            Debug.Log("Vampire shot");
            // Spawn bullet at the position of firePoint, add force to it

            GameObject bullet = BulletPool.instance.GetPooledObject();

            if (bullet != null)
            {
                bullet.GetComponent<BulletController>().shooter = vampire;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                //vampireLookDirection = (playerPosition - rb.position).normalized;
                vampireLookDirection = new Vector3(playerPosition.x - this.gameObject.transform.position.x, 0, playerPosition.z - this.gameObject.transform.position.z).normalized;
                bullet.transform.position = vampire.transform.position;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                bullet.SetActive(true);
                rb.AddForce(vampireLookDirection * bulletForce, ForceMode.Impulse);
            }
        }
    }

}
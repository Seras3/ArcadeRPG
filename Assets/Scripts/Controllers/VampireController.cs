using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


public class VampireController : EnemyController
{    
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

    private Plane _plane;

    void Start()
    { 
        _plane = new Plane(Vector3.up, 0);

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

        _playerPosition = GameObject.Find("B-spine").transform.position;

        if (_playerPosition != null) 
        {
            var lookPos = _playerPosition - transform.position;
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

        if (state == 1 && GameManager.CurrentStatus is GameManager.GameStatus.Playing) 
        {
           base.MoveTowardsPlayer();
        }
        

        if (state == 2 && GameManager.CurrentStatus is GameManager.GameStatus.Playing) {
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



    void Shoot()
    {
        if (Cooldown())
        {
            // Spawn bullet at the position of firePoint, add force to it

            GameObject bullet = PistolBulletPool.instance.GetPooledObject();

            if (bullet != null)
            {
                bullet.GetComponent<BulletController>().shooter = vampire;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                vampireLookDirection = new Vector3(_playerPosition.x - this.gameObject.transform.position.x, 0, _playerPosition.z - this.gameObject.transform.position.z).normalized;
                bullet.transform.position = vampire.transform.position;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                bullet.SetActive(true);
                
                var lookDirection = (_playerPosition - bullet.transform.position).normalized;
                bullet.transform.rotation = Quaternion.LookRotation(lookDirection);
                bullet.transform.Rotate(-90, 0, 0);

                rb.AddForce(vampireLookDirection * bulletForce, ForceMode.Impulse);
            }
        }
    }

}
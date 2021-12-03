using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    private Vector3 dummyLookDirection;
    public float bulletForce = 20f;

    private float shootingCooldown;
    private float lastShotTime;

    private void Start() {
        shootingCooldown = 0.5f;
        lastShotTime = -shootingCooldown; // fire as soon as the game starts
    }
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }
    private bool Cooldown(){
        if (Time.fixedTime >= (lastShotTime + shootingCooldown)){
            lastShotTime = Time.fixedTime;
            return true;
        }
        return false;
    }

    void Shoot()
    {
        
        if (Cooldown()){
            
            // Spawn bullet at the position of firePoint, add force to it

            GameObject bullet = BulletPool.instance.GetPooledObject();

            if (bullet != null){
                bullet.GetComponent<BulletController>().shooter = this.gameObject;
                Physics.IgnoreCollision(GetComponent<Collider>(), bullet.GetComponent<Collider>(), true);
                dummyLookDirection = GetComponent<HumanoidMovementPlayer>().lookDirection;
                bullet.transform.position = firePoint.position;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                bullet.SetActive(true);
                rb.AddForce(dummyLookDirection * bulletForce, ForceMode.Impulse);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private Vector3 dummyLookDirection;
    public float bulletForce = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            dummyLookDirection = GetComponent<HumanoidMovementPlayer>().lookDirection;
            Shoot();
        }
    }

    void Shoot()
    {
        // Spawn bullet at the position of firePoint, add force to it
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, new Quaternion(0,0,0,0));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<BulletController>().shooter = this.gameObject;
        rb.AddForce(dummyLookDirection * bulletForce, ForceMode.Impulse);
    }
}

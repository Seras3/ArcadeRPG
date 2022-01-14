using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Transform firePoint;
        private Vector3 dummyLookDirection;
        public float bulletForce = 20f;

        private float shootingCooldown;
        private float lastShotTime;

        private Stats.PlayerStats _stats;

        private Plane _plane;
        private GameObject _activeWeapon;
        private BulletStats _activeBulletStats;
        private void Start()
        {
            _stats = GameObject.Find("Dummy").GetComponent<Stats.PlayerStats>();
            _plane = new Plane(Vector3.up, 0);
            lastShotTime = -shootingCooldown; // fire as soon as the game starts
        }
        void Update()
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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

        private void Shoot()
        {
            if (!Cooldown()) return;

            UpdateWeaponInfo();
            // Spawn bullet at the position of firePoint, add force to it

            if(!_activeWeapon.GetComponent<Weapon>().HasAmmo())
            {
                _stats.ChangeWeapon();
                return;
            } 
            else 
            {
                _activeWeapon.GetComponent<Weapon>().ReduceAmmo();
            }


            GameObject bullet = _activeWeapon.GetComponent<Weapon>().Bullet.transform.name == "RiffleBullet" 
                ? ShotgunBulletPool.instance.GetPooledObject() : PistolBulletPool.instance.GetPooledObject();

            if (bullet == null) return;
                
            bullet.GetComponent<BulletController>().shooter = this.gameObject;
            Physics.IgnoreCollision(GetComponent<Collider>(), bullet.GetComponent<Collider>(), true);
            dummyLookDirection = GetComponent<HumanoidMovementPlayer>().lookDirection;
            bullet.transform.position = firePoint.position;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bullet.SetActive(true);
            
            // rotate bullet towards dummyLookDirection
            bullet.transform.rotation = Quaternion.LookRotation(dummyLookDirection);
            bullet.transform.Rotate(_activeBulletStats.OffsetRotation);

            // set bullet position in front of the weapon
            var position = bullet.transform.position;
            position += dummyLookDirection * _activeBulletStats.OffsetPositionFactor;
            position = new Vector3(position.x, (float) (GlobalConstants.WeaponPositionYOffset + GlobalConstants.BulletPositionYOffset), position.z);
            bullet.transform.position = position;

            
            float accRange = (1-_activeWeapon.GetComponent<Weapon>().accuracy) * 90;
            Quaternion accuracyRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-accRange, accRange), Vector3.up);

            rb.AddForce(accuracyRotation * dummyLookDirection * bulletForce, ForceMode.Impulse);
            FindObjectOfType<AudioManager>().Play("pistolShot");
        }

        private void UpdateWeaponInfo()
        {
            _activeWeapon = GetComponent<PlayerStats>().ActiveWeapon;
            _activeBulletStats = _activeWeapon.GetComponent<Weapon>().Bullet.GetComponent<BulletStats>();
            shootingCooldown = _activeWeapon.GetComponent<Weapon>().FireRate;
        }
    }
}




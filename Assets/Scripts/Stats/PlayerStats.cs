using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public GameObject ActiveWeapon;

        public List<GameObject> WeaponsList = new List<GameObject>();

        private int _activeWeaponIndex;
        private bool _isNextWeaponReady;

        private UIHandler _uiHandler;
        private void Start()
        {
            ActiveWeapon = Instantiate(ActiveWeapon, transform);
            ActiveWeapon.transform.position += ActiveWeapon.GetComponent<Weapon>().OffsetPosition;
            _uiHandler = GameObject.Find("UIModifier").GetComponent<UIHandler>();

            _activeWeaponIndex = 0;
            PoolWeapons();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeWeapon();
            }
        }

        public override void Die()
        {
            Debug.Log(transform.name + " died.");

            GameObject.Find("GameManagerObject").GetComponent<GameManager>().GameOver();
        }

        private void PoolWeapons() 
        {
            GameObject pooledWeapon;
            for(int i = 1; i < WeaponsList.Count; i++) {
                pooledWeapon = Instantiate(WeaponsList[i], transform);
                pooledWeapon.transform.position += pooledWeapon.GetComponent<Weapon>().OffsetPosition;
            }
        }

        public void ChangeWeapon()
        {
            _isNextWeaponReady = false;
            while(!_isNextWeaponReady) 
            {
                _isNextWeaponReady = true;
                _activeWeaponIndex++;
                _activeWeaponIndex %= WeaponsList.Count;

                var existingWeapon = transform.GetChild(3 + _activeWeaponIndex).gameObject;
                if(existingWeapon.GetComponent<Weapon>().AmmoCount == 0) 
                {
                    _isNextWeaponReady = false;
                }

                if(_isNextWeaponReady) 
                {
                    ActiveWeapon.SetActive(false);
                    existingWeapon.SetActive(true);
                    ActiveWeapon = existingWeapon;
                }
            }

        }

        public void PickDrop(string WeaponName, int AmmoCount) 
        {
            Weapon weaponStats;
            _uiHandler.DisplayNewDrop("+" + AmmoCount + " " + WeaponName + " Bullets");
            for(int i = 0; i < WeaponsList.Count; i++) 
            {
                weaponStats = transform.GetChild(3 + i).gameObject.GetComponent<Weapon>();
                if(weaponStats.Name == WeaponName) 
                {  
                    weaponStats.AmmoCount += AmmoCount;
                }
            }
        }
    }
}
using System;
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

        private void Start()
        {
            ActiveWeapon = Instantiate(ActiveWeapon, transform);
            ActiveWeapon.transform.position += ActiveWeapon.GetComponent<Weapon>().OffsetPosition;

            _activeWeaponIndex = 0;
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

        private void ChangeWeapon()
        {
            _activeWeaponIndex++;
            _activeWeaponIndex %= WeaponsList.Count;
            
            ActiveWeapon.SetActive(false);

            try
            {
                var existingWeapon = transform.GetChild(3 + _activeWeaponIndex).gameObject;
                existingWeapon.SetActive(true);
                ActiveWeapon = existingWeapon;
            }
            catch (Exception)
            {
                ActiveWeapon = Instantiate(WeaponsList[_activeWeaponIndex], transform);
                
                var position = ActiveWeapon.transform.position;
                position += GetComponent<HumanoidMovementPlayer>().lookDirection *
                            (float) GlobalConstants.WeaponPositionFactorOffset;
                position = new Vector3(position.x, (float) GlobalConstants.WeaponPositionYOffset, position.z);
                ActiveWeapon.transform.position = position;
            }

        }
    }
}
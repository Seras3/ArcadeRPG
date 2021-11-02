using System;
using UnityEngine;

namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public Weapon weapon = new Weapon("StormBreaker", 25);

        //private List<Weapon> ownedWeapons = new List<Weapon>();
        
        protected override void Die()
        {
            Debug.Log(transform.name + " died.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                this.TakeDamage(weapon.Damage);
            }
        }
    }
}
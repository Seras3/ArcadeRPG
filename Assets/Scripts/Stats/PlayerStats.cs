using System;
using UnityEngine;

namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public Weapon weapon = new Weapon("StormBreaker", 25);

        //private List<Weapon> ownedWeapons = new List<Weapon>();
        
        public override void Die()
        {
            Debug.Log(transform.name + " died.");
        }
    }
}
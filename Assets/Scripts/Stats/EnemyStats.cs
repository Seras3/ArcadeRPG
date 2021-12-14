using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        [System.Serializable]
        public class Drop
        {
            public GameObject Weapon;
            public int AmmoCount;
            public float DropChance; 
        }

        private GameObject objectSpawner;
        public int Damage { get; set; }
        public int ScoreCount;
        public List<Drop> DropList;

        void Start() 
        {
            if(ScoreCount == 0) { ScoreCount = GlobalConstants.DefaultEnemyScoreCount; }

            objectSpawner = GameObject.Find("ObjectSpawner");
        }
        public EnemyStats() 
        {
            Damage = 20;
        }

        public void TryDropLoot() 
        {
            foreach(Drop drop in DropList)
            {
                if(UnityEngine.Random.value <= drop.DropChance) 
                {
                    var WeaponName = drop.Weapon.GetComponent<Weapon>().Name;
                    GameObject.Find("Dummy").GetComponent<Stats.PlayerStats>().PickDrop(WeaponName, drop.AmmoCount);
                    break;
                }
            }

        }

        public override void Die()
        {
            TryDropLoot();

            objectSpawner.GetComponent<WaveHandler>().killEnemy();
            GameManager.AddScore(ScoreCount);

            this.gameObject.SetActive(false);
            Debug.Log(transform.name + " died.");
        }

    }
}
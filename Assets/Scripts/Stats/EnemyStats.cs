using UnityEngine;
using Utils;
using System.Collections.Generic;
using System;
using System.Collections;

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
        public float OffsetYPosition;
        public int Damage;
        public int ScoreCount;
        public List<Drop> DropList;

        private static readonly int IsDead = Animator.StringToHash("isDead");

        void Start() 
        {
            if(ScoreCount == 0) { ScoreCount = GlobalConstants.DefaultEnemyScoreCount; }

            objectSpawner = GameObject.Find("ObjectSpawner");
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
            var anim = GetComponent<Animator>();
            anim.SetBool(IsDead, true);
            TryDropLoot();

            GetComponent<EnemyController>().enabled = false;
            objectSpawner.GetComponent<LevelController>().killEnemy();
            GameManager.AddScore(ScoreCount);
            
            StartCoroutine(RunDieAnimation(anim.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator RunDieAnimation(float seconds)
        {
            yield return new WaitForSeconds(seconds + 1);
            gameObject.SetActive(false);
        }
    }
}
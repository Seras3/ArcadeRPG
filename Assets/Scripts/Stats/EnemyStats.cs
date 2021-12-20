﻿using UnityEngine;
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
        public int Damage { get; set; }
        public int ScoreCount;
        public List<Drop> DropList;
        // private static readonly int IsDead = Animator.StringToHash("isDead");

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
            GetComponent<BoxCollider>().enabled = false;
            
            var anim = GetComponent<Animator>();
            anim.SetBool("isDead", true);
            Debug.Log("PARAM: " +anim.GetBool("isDead"));
            // anim.Play("Die", 0);
            
            objectSpawner.GetComponent<LevelController>().killEnemy();
            GameManager.AddScore(ScoreCount);
            
            StartCoroutine(RunDieAnimation(anim.GetCurrentAnimatorStateInfo(0).length));
        }

        private IEnumerator RunDieAnimation(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            gameObject.SetActive(false);
        }
    }
}
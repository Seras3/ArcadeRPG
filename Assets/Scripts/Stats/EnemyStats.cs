using UnityEngine;
using Utils;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        private GameObject objectSpawner;
        public int Damage { get; set; }
        public int ScoreCount;

        void Start() 
        {
            if(ScoreCount == 0) { ScoreCount = GlobalConstants.DefaultEnemyScoreCount; }

            objectSpawner = GameObject.Find("ObjectSpawner");
        }
        public EnemyStats() 
        {
            Damage = 20;
        }
        
        public override void Die()
        {
            objectSpawner.GetComponent<WaveHandler>().killEnemy();
            GameManager.AddScore(ScoreCount);

            this.gameObject.SetActive(false);
            Debug.Log(transform.name + " died.");
        }

    }
}
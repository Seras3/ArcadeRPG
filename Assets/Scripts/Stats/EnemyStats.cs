using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        public UIHandler handler;
        private GameObject objectSpawner;
        public int Damage { get; set; }

        public int ScoreCount;

        private int DEFAULT_SCORE_COUNT = 5;

        void Start() 
        {
            if(ScoreCount == 0) { ScoreCount = DEFAULT_SCORE_COUNT; }

            handler = GameObject.Find("UIModifier").GetComponent<UIHandler>();
            objectSpawner = GameObject.Find("ObjectSpawner");
        }
        public EnemyStats() 
        {
            Damage = 20;
        }
        
        public override void Die()
        {
            handler.AddScore(ScoreCount);
            objectSpawner.GetComponent<WaveHandler>().killEnemy();

            this.gameObject.SetActive(false);
            Debug.Log(transform.name + " died.");
        }

    }
}
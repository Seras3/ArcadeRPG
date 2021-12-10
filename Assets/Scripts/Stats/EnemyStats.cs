using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        private UIHandler handler;
        private GameObject objectSpawner;
        public int Damage { get; set; }
        public int ScoreCount;

        void Start() 
        {
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
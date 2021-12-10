using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        public UIHandler handler;
        private GameObject objectSpawner;
        public int Damage { get; set; }

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
            handler.AddScore(5);
            objectSpawner.GetComponent<WaveHandler>().killEnemy();

            this.gameObject.SetActive(false);
            Debug.Log(transform.name + " died.");
        }

    }
}
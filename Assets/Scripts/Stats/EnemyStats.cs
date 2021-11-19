using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        private GameObject objectSpawner;

        public int Damage { get; set; }

        void Start()
        {
            objectSpawner = GameObject.Find("ObjectSpawner");
        }
        
        public EnemyStats() 
        {
            Damage = 50;
        }
        
        public override void Die()
        {
            objectSpawner.GetComponent<WaveHandler>().killEnemy();

            this.gameObject.SetActive(false);
            Debug.Log(transform.name + " died.");
        }

    }
}
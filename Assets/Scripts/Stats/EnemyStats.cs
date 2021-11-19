using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        public UIHandler handler;
        public int Damage { get; set; }

        void Start() 
        {
            handler = GameObject.Find("UIModifier").GetComponent<UIHandler>();
        }

        public EnemyStats() 
        {
            Damage = 50;
        }
        
        public override void Die()
        {
            handler.AddScore(5);
            Destroy(this.gameObject);
            Debug.Log(transform.name + " died.");
        }
    }
}
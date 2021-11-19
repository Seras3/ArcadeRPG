using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        public int Damage { get; set; }

        public EnemyStats() 
        {
            Damage = 50;
        }
        
        public override void Die()
        {
            Destroy(this.gameObject);
            Debug.Log(transform.name + " died.");
            FindObjectOfType<AudioManager>().Play("zombieDies");
        }
    }
}
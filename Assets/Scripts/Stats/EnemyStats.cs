using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        protected override void Die()
        {
            Debug.Log(transform.name + " died.");
        }
    }
}
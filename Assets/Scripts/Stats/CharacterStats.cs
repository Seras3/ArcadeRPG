using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    private const int MaxHealth = 100;

    public Stat CurrentHealth { get; set; }
    public float MovementSpeed { get; set; }

    public void Awake()
    {
        CurrentHealth = new Stat(MaxHealth);
        MovementSpeed = 0.005f;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth.GetValue() > 0)
        {
            CurrentHealth.SubtractValue(damage);
            Debug.Log(transform.name + " takes " + damage + " damage.");
        }

        Debug.Log(CurrentHealth.GetValue());
        if (CurrentHealth.GetValue() <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
}

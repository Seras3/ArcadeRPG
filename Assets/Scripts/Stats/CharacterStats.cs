using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    private const int MaxHealth = 100;

    public Stat currentHealth;
    public double movementSpeed;

    public void Awake()
    {
        currentHealth = new Stat(MaxHealth);
    }

    protected void TakeDamage(int damage)
    {
        if (currentHealth.GetValue() > 0)
        {
            currentHealth.SubtractValue(damage);
            Debug.Log(transform.name + " takes " + damage + " damage.");
        }

        if (currentHealth.GetValue() <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}

using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Invulnerability")]
    public bool isInvulnerable = false;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    //Handling Damage that will be called by other entities:
    /// <param name="amount">Amount of damage.</param>
    /// <param name="hitDirection">The direction from which damage came (for knockback, etc).</param>
    public virtual void Damage(int amount, Vector2 hitDirection)
    {
        if (isInvulnerable || amount <= 0)
            return;

        currentHealth -= amount;

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Optional: play hit animation, knockback, etc.
        }
    }

    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
        // Default death behavior
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}

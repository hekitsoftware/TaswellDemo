using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public float currentHealth;
    public bool isInvulnerable = false;

    public UnityEvent OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth < 0) {
        currentHealth = 0;
            Die();
        }
    }

    public virtual void Damage(int amount, Vector2 hitDirection)
    {
        if (isInvulnerable || amount <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //DEATH
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
        // Default death behavior
        Destroy(gameObject);
    }

    public double GetHealth()
    {
        return currentHealth;
    }
}

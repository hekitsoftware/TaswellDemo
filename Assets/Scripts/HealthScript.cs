using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool isInvulnerable = false;
    public Image healthBar;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        //this needs to be negatvie due to how I'm making the health bar work
        healthBar.fillAmount = (maxHealth - currentHealth);
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

    //HEALING
    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    //DEATH
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

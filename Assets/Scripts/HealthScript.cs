using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isInvulnerable = false;
    public float healthMulti = 1f;

    public Collider2D hitbox;
    public ItemManager itemScript;

    public UnityEvent OnDeath;
    public UnityEvent OnHit;

    public GameObject floatingTextPrefab;
    public TextMeshPro text;
    public Transform selfLocation;

    [SerializeField] public bool IsPlayer;

    private void Awake()
    {
        currentHealth = maxHealth;
        selfLocation = transform;

        if (floatingTextPrefab != null)
        {
            text = floatingTextPrefab.GetComponent<TextMeshPro>();
        }
    }

    private void Update()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }

        if (currentHealth > 100)
            currentHealth = 100;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void updateMaxHealth()
    {
        maxHealth *= healthMulti;
    }

    public void Damage(int amount)
    {
        if (isInvulnerable || amount <= 0)
            return;

        currentHealth -= amount;

        if (floatingTextPrefab != null)
        {
            text.text = $"{amount}";
            Instantiate(floatingTextPrefab, selfLocation.position, Quaternion.identity);
        }

        OnHit?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public double GetHealth()
    {
        return currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log($"{this} got hit!");
            OnHit?.Invoke();
        }
    }
}

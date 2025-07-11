using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public double maxHealth = 100f;
    public double currentHealth;
    public bool isInvulnerable = false;
    public double healthMulti;

    public Collider2D hitbox;

    public ItemManager itemScript;

    public UnityEvent OnDeath;
    public UnityEvent OnHit;

    public GameObject floatingTextPrefab;
    public TextMeshPro text;
    public Transform selfLocation;

    [SerializeField] public bool IsPlayer;

    private void Start()
    {
        selfLocation = this.transform;
        text = floatingTextPrefab.GetComponent<TextMeshPro>();
        healthMulti = 1;
    }

    public void updateMaxHealth()
    {
        maxHealth *= healthMulti;
    }

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

        if (currentHealth > 100)
        {
            currentHealth = 100;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Damage(int amount)
    {
        if (isInvulnerable || amount <= 0)
            return;

        currentHealth -= amount;
        text.text = $"{amount}";
        Instantiate(floatingTextPrefab, selfLocation.position, Quaternion.identity);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(double amount)
    {
        currentHealth += amount;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            OnHit?.Invoke();
            Debug.Log($"{this} got hit!");
        }
    }
}

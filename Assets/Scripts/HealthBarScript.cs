using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text healthText;

    private HealthScript healthScript;

    private void Awake()
    {
        healthScript = player.GetComponent<HealthScript>();

        // Initialize slider range
        healthBar.minValue = 0f;
        healthBar.maxValue = 1f;
    }

    private void Update()
    {
        healthBar.value = ((healthScript.currentHealth / 100));
        healthText.text = $"{healthScript.GetHealth()} / {(int)healthScript.maxHealth}";
    }
}

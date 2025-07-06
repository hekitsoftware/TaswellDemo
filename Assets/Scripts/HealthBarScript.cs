using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] public Slider healthBar;
    [SerializeField] public GameObject Player;
    [SerializeField] public TMP_Text healthText;

    public HealthScript healthScript;
    private double healthBarPercentage;
    private void Awake()
    {
        healthScript = Player.GetComponent<HealthScript>();

        healthBar.maxValue = 1;
        healthBar.minValue = 0;
    }
    private void Update()
    {
        healthBarPercentage = healthScript.currentHealth / 100;

        healthBar.value = (float)healthBarPercentage;
        healthText.text = $"{(int)healthScript.currentHealth} / {(int)healthScript.maxHealth}";
    }
}

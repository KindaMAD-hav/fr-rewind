using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public string HealthBarName = "HealthBar";
    public Image healthBarFill;
    public float depletionRate = 4f; // Health depletion rate per second

    void Start()
    {
        currentHealth = maxHealth;
        //healthBarFill = GameObject.Find(HealthBarName).GetComponent<Image>();
    }

    void Update()
    {
        // Deplete health over time
        DepleteHealth(Time.deltaTime * depletionRate);

        // Update health bar UI
        healthBarFill.fillAmount = (float)currentHealth / maxHealth;

        // Check if health is zero
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the player
        }
    }

    void DepleteHealth(float amount)
    {
        currentHealth -= Mathf.RoundToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Tooltip("Slider that displays the healthbar")]
    public Slider healthBar;

    [Tooltip("Maximum health")]
    public float maxHealth = 100f;

    [Tooltip("Health regeneration per second")]
    public float healthRegenPerSecond = 1f;

    public float CurrentHealth { get; private set; }

    public void TakeDamage(float damage)
    {
        if(damage > CurrentHealth)
        {
            CurrentHealth = 0;
            Destroy(gameObject);
        }
        else
        {
            CurrentHealth -= damage;
        }

        UpdateHealthBar();
    }

    private void FixedUpdate()
    {
        CurrentHealth += healthRegenPerSecond * Time.fixedDeltaTime;
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar() => healthBar.normalizedValue = CurrentHealth / maxHealth;
}

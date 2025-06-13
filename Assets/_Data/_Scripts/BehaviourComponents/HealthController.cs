using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 250; // Maximum health
    private int currentHealth; // Current health
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Types type; // Type of health controller
    public enum Types
    {
        Cloud,
        Enemy
    }
   

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    // Method to apply damage
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StartCoroutine(FlashRedThenWhite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to heal
    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    }

    // Method to handle death
    private void Die()
    {
        //Debug.Log($"{gameObject.name} has died.");
        if (type == Types.Cloud)
        {
            Debug.Log($"{gameObject.name} - Cloud die");
        }else if (type == Types.Enemy)
        {
            Debug.Log($"{gameObject.name} - Enemy die");
            MMEventManager.TriggerEvent(new EEnemyDie(gameObject.transform.position));
        }
        Destroy(gameObject);
    }

    // Method to get current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Method to get max health
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    private IEnumerator FlashRedThenWhite()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = Color.white;
    }

}
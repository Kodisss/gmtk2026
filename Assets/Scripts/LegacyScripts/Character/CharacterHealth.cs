using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        // TODO: Update health UI
    }

    public void TakeDamage(int amount)
    {
        if (IsDead)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // TODO:
        // Play hurt animation
        // Camera shake
        // Invincibility frames
        // Flash sprite

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");

        // Disable controls
        // Play animation
        // Respawn / Game Over
    }
}
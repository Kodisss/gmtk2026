using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyDropTable dropTable;

    [SerializeField]
    private int maxHealth = 50;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        dropTable = GetComponent<EnemyDropTable>();
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        currentHealth -= damage;

        ApplyKnockback(knockback);

        if(currentHealth <= 0)
        {
            Die();
        }
    }



    private void ApplyKnockback(Vector2 force)
    {
        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();


        if(rb != null)
        {
            rb.AddForce(force,
                ForceMode2D.Impulse);
        }
    }



    private void Die()
    {
        if(dropTable != null)
        {
            dropTable.DropCard();
        }


        Destroy(gameObject);
    }
}
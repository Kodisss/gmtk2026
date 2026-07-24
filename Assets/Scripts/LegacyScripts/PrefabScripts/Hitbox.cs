using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private int damage;

    private float knockback;

    private Vector2 direction;



    public void Initialize(
        int damage,
        float knockback,
        Vector2 direction)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.direction = direction;


        Destroy(gameObject,0.3f);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();


        if(enemy == null)
            return;



        enemy.TakeDamage(
            damage,
            direction * knockback
        );
    }
}
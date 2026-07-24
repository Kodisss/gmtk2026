using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;

    private float speed;



    public int damage = 10;



    public void Initialize(
        Vector2 direction,
        float speed)
    {
        this.direction = direction;
        this.speed = speed;


        Destroy(gameObject,5f);
    }



    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();


        if(enemy == null)
            return;


        enemy.TakeDamage(
            damage,
            direction * 5f
        );


        Destroy(gameObject);
    }
}
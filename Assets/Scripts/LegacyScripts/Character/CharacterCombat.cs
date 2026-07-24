using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    private Animator animator;

    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        spriteRenderer =
            GetComponentInChildren<SpriteRenderer>();
    }



    public void PerformAttack(Attack attack)
    {
        if(attack == null)
            return;


        PlayAnimation(attack);


        SpawnHitbox(attack);
    }



    private void PlayAnimation(Attack attack)
    {
        if(animator == null)
            return;


        if(!string.IsNullOrEmpty(attack.animationTrigger))
        {
            animator.SetTrigger(
                attack.animationTrigger
            );
        }
    }



    private void SpawnHitbox(Attack attack)
    {
        if(attack.hitboxPrefab == null)
            return;


        Vector3 direction =
            spriteRenderer.flipX
            ? Vector3.left
            : Vector3.right;



        Vector3 position =
            transform.position +
            direction * attack.attackOffset;



        GameObject hitbox =
            Instantiate(
                attack.hitboxPrefab,
                position,
                Quaternion.identity
            );



        Hitbox hitboxComponent =
            hitbox.GetComponent<Hitbox>();


        if(hitboxComponent != null)
        {
            hitboxComponent.Initialize(
                attack.damage,
                attack.knockbackForce,
                direction
            );
        }
    }



    public void ApplyKnockback(float force)
    {
        // For future:
        // apply knockback to player
    }



    public void ApplyDamageOverTime(
        int damage,
        float duration,
        float interval)
    {
        // Could later connect to player status system
    }
}
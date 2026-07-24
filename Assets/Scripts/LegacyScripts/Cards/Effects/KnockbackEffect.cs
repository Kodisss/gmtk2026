using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Knockback")]
public class KnockbackEffect : CardEffect
{
    public float force = 10f;


    public override void Execute(CardContext context)
    {
        if(context.combat == null)
            return;


        context.combat.ApplyKnockback(force);
    }
}
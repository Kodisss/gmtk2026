using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Damage Over Time")]
public class DamageOverTimeEffect : CardEffect
{
    public int damage;

    public float duration;

    public float interval = 1f;



    public override void Execute(CardContext context)
    {
        if(context.combat == null)
            return;


        context.combat.ApplyDamageOverTime(
            damage,
            duration,
            interval
        );
    }
}
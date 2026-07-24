using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Damage")]
public class DamageEffect : CardEffect
{
    public Attack attack;


    public override void Execute(CardContext context)
    {
        if(context.combat == null)
        {
            Debug.LogWarning("No CharacterCombat found");
            return;
        }


        context.combat.PerformAttack(attack);
    }
}
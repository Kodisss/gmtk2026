using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Heal")]
public class HealEffect : CardEffect
{
    public int healAmount;


    public override void Execute(CardContext context)
    {
        if(context.health == null)
        {
            Debug.LogWarning("No CharacterHealth found");
            return;
        }


        context.health.Heal(healAmount);
    }
}
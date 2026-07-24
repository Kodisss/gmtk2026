using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Speed Buff")]
public class SpeedBuffEffect : CardEffect
{
    public float multiplier = 2f;
    public float duration = 5f;

    public override void Execute(CardContext context)
    {
        if(context.movement == null) return;

        context.stats.ApplySpeedMultiplier(multiplier, duration);
    }
}
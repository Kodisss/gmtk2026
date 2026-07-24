using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Double Jump")]
public class DoubleJumpEffect : CardEffect
{
    public float duration = 10f;

    public override void Execute(CardContext context)
    {
        context.stats.EnableDoubleJump(duration);
    }
}
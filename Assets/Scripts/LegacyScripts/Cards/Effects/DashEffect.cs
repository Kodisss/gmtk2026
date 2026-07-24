using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Dash")]
public class DashEffect : CardEffect
{
    public float distance = 5f;

    public float force = 20f;


    public override void Execute(CardContext context)
    {
        if(context.movement == null)
        {
            Debug.LogWarning("No CharacterMovement2D found");
            return;
        }


        // context.movement.Dash(distance, force);
    }
}
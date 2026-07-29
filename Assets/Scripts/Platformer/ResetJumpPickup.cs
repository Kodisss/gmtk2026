using UnityEngine;

public class ResetJumpPickup : Boost
{
    protected override void ApplyBoost()
    {
        stats.NewDashPickup();
    }
}

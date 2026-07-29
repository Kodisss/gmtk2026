using UnityEngine;

public class SpeedBoostPickup : Boost
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float boostDuration = 5f;

    protected override void ApplyBoost()
    {
        stats.ApplySpeedMultiplier(speedMultiplier, boostDuration);
    }
}

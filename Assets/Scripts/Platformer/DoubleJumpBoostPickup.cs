using UnityEngine;

public class DoubleJumpBoostPickup : Boost
{
    [SerializeField] private float boostDuration = 5f;

    protected override void ApplyBoost()
    {
        stats.EnableDoubleJump(boostDuration);
    }
}

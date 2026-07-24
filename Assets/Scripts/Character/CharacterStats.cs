using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Movement")]

    public float SpeedMultiplier { get; private set; } = 1f;

    public bool DoubleJumpEnabled { get; private set; }



    [Header("Combat")]

    public float DamageMultiplier { get; private set; } = 1f;

    public float AttackSpeedMultiplier { get; private set; } = 1f;



    [Header("Defense")]

    public bool Invincible { get; private set; }



    private Coroutine speedRoutine;
    private Coroutine doubleJumpRoutine;
    private Coroutine damageRoutine;
    private Coroutine invincibleRoutine;



    #region Speed

    public void ApplySpeedMultiplier(float multiplier, float duration)
    {
        if (speedRoutine != null)
            StopCoroutine(speedRoutine);

        speedRoutine = StartCoroutine(SpeedRoutine(multiplier, duration));
    }

    private IEnumerator SpeedRoutine(float multiplier, float duration)
    {
        SpeedMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        SpeedMultiplier = 1f;
    }

    #endregion



    #region Double Jump

    public void EnableDoubleJump(float duration)
    {
        if (doubleJumpRoutine != null)
            StopCoroutine(doubleJumpRoutine);

        doubleJumpRoutine = StartCoroutine(DoubleJumpRoutine(duration));
    }

    private IEnumerator DoubleJumpRoutine(float duration)
    {
        DoubleJumpEnabled = true;

        yield return new WaitForSeconds(duration);

        DoubleJumpEnabled = false;
    }

    #endregion



    #region Damage

    public void ApplyDamageMultiplier(float multiplier, float duration)
    {
        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(DamageRoutine(multiplier, duration));
    }

    private IEnumerator DamageRoutine(float multiplier, float duration)
    {
        DamageMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        DamageMultiplier = 1f;
    }

    #endregion



    #region Invincible

    public void BecomeInvincible(float duration)
    {
        if (invincibleRoutine != null)
            StopCoroutine(invincibleRoutine);

        invincibleRoutine = StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        Invincible = true;

        yield return new WaitForSeconds(duration);

        Invincible = false;
    }

    #endregion
}
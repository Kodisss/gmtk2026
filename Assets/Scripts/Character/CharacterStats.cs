using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Movement")]
    public float SpeedMultiplier { get; private set; } = 1f;
    public bool DoubleJumpEnabled { get; private set; }

    private Coroutine speedRoutine;
    private Coroutine doubleJumpRoutine;

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
}
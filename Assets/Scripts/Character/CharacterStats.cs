using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Movements")]
    public float SpeedMultiplier { get; private set; } = 1f;
    public bool DoubleJumpEnabled { get; private set; } = false;

    [SerializeField] private AudioSource boostAudioSource;
    

    [Header("Speed boost grejer")]
    [SerializeField] private SpriteRenderer speedBoostRenderer;
    [SerializeField] private AudioClip speedIn;
    [SerializeField] private AudioClip speedOut;
    [SerializeField] private Sprite speedBoostSprite;

    [Header("Double jump boost grejer")]
    [SerializeField] private SpriteRenderer doubleJumpBoostRenderer;
    [SerializeField] private Sprite doubleJumpSprite;

    private Coroutine speedRoutine;
    private Coroutine doubleJumpRoutine;

    #region Speed

    private void Start()
    {
        speedBoostRenderer.sprite = null;
        doubleJumpBoostRenderer.sprite = null;
    }

    public void ApplySpeedMultiplier(float multiplier, float duration)
    {
        if (speedRoutine != null) StopCoroutine(speedRoutine);

        speedRoutine = StartCoroutine(SpeedRoutine(multiplier, duration));
    }

    private IEnumerator SpeedRoutine(float multiplier, float duration)
    {
        SpeedMultiplier = multiplier;
        speedBoostRenderer.sprite = speedBoostSprite;
        boostAudioSource.PlayOneShot(speedIn);

        yield return new WaitForSeconds(duration);

        SpeedMultiplier = 1f;
        speedBoostRenderer.sprite = null;
        boostAudioSource.PlayOneShot(speedOut);
    }

    #endregion

    #region Double Jump

    public void EnableDoubleJump(float duration)
    {
        if (doubleJumpRoutine != null) StopCoroutine(doubleJumpRoutine);

        doubleJumpRoutine = StartCoroutine(DoubleJumpRoutine(duration));
    }

    private IEnumerator DoubleJumpRoutine(float duration)
    {
        DoubleJumpEnabled = true;
        doubleJumpBoostRenderer.sprite = doubleJumpSprite;

        yield return new WaitForSeconds(duration);

        DoubleJumpEnabled = false;
        doubleJumpBoostRenderer.sprite = null;
    }

    #endregion
}
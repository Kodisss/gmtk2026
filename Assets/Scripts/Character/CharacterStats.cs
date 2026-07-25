using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private CharacterAnimation characterAnimation;
    private CharacterMovement2D movements;

    [Header("Movements")]
    public float SpeedMultiplier { get; private set; } = 1f;
    public bool DoubleJumpEnabled { get; private set; } = false;

    [SerializeField] private AudioSource boostAudioSource;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<int> OnHealthChanged;
    // public event Action OnDeath;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Damage")]
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private Color damageColor = Color.red;
    private bool invincible;
    

    [Header("Speed boost grejer")]
    [SerializeField] private SpriteRenderer speedBoostRenderer;
    [SerializeField] private AudioClip speedIn;
    [SerializeField] private AudioClip speedOut;
    [SerializeField] private Sprite speedBoostSprite;

    [Header("Double jump boost grejer")]
    [SerializeField] private SpriteRenderer doubleJumpBoostRenderer;
    [SerializeField] private Sprite doubleJumpSprite;

    [SerializeField] private AudioClip pickUpItemSound;

    private Coroutine speedRoutine;
    private Coroutine doubleJumpRoutine;
    private Coroutine invincibilityCoroutine;

    #region Speed

    private void Start()
    {
        CurrentHealth = maxHealth;
        characterAnimation = GetComponentInChildren<CharacterAnimation>();

        movements = GetComponent<CharacterMovement2D>();
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

        boostAudioSource.PlayOneShot(pickUpItemSound);
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

    public void NewDashPickup()
    {
        boostAudioSource.PlayOneShot(pickUpItemSound);
        movements.CanDash = true;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || invincible)
            return;

        CurrentHealth -= damage;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        invincible = true;

        Color startColor = Color.white;

        float halfTime = invincibilityTime * 0.5f;

        // White -> Red
        float timer = 0f;
        while (timer < halfTime)
        {
            timer += Time.deltaTime;

            spriteRenderer.color = Color.Lerp(
                startColor,
                damageColor,
                timer / halfTime
            );

            yield return null;
        }

        // Red -> White
        timer = 0f;
        while (timer < halfTime)
        {
            timer += Time.deltaTime;

            spriteRenderer.color = Color.Lerp(
                damageColor,
                startColor,
                timer / halfTime
            );

            yield return null;
        }

        spriteRenderer.color = startColor;

        invincible = false;
        invincibilityCoroutine = null;
    }

    public void Die()
    {
        if (IsDead) return;

        IsDead = true;

        movements.StopMovement();
        Debug.Log("Stopped Movements");

        GetComponent<CharacterMovement2D>().enabled = false;

        Debug.Log("Disabled Movements");

        GetComponentInChildren<CharacterAudio>().PlayDeathSoundEffect();

        characterAnimation.PlayDeath();
    }

    public void FinishDeath()
    {
        GameManager.Instance.PlayerDied();

        gameObject.SetActive(false);
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}
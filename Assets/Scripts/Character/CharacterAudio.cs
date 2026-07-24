using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterMovement2D movement;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource effectsSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip landClip;

    [Header("Footsteps")]
    [SerializeField] private float stepInterval = 0.35f;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;
    [SerializeField] private float footstepVolume = 1f;

    [Header("Effects")]
    [SerializeField] private float jumpVolume = 1f;
    [SerializeField] private float dashVolume = 1f;
    [SerializeField] private float landVolume = 1f;

    private CharacterState previousState;
    private bool previousGrounded;

    private float stepTimer;

    private void Awake()
    {
        if (movement == null)
            movement = GetComponent<CharacterMovement2D>();

        footstepSource.playOnAwake = false;
        footstepSource.loop = false;
        footstepSource.spatialBlend = 0f;

        effectsSource.playOnAwake = false;
        effectsSource.loop = false;
        effectsSource.spatialBlend = 0f;
    }

    private void Update()
    {
        HandleFootsteps();
        HandleEvents();

        previousState = movement.CurrentState;
        previousGrounded = movement.IsGrounded;
    }

    private void HandleFootsteps()
    {
        bool walking =
            movement.IsGrounded &&
            movement.CurrentState == CharacterState.Walking;

        if (!walking)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer > 0f)
            return;

        footstepSource.pitch = Random.Range(minPitch, maxPitch);
        footstepSource.PlayOneShot(footstepClip, footstepVolume);

        stepTimer = stepInterval;
    }

    private void HandleEvents()
    {
        // Jump
        if (previousGrounded && !movement.IsGrounded)
        {
            PlayEffect(jumpClip, jumpVolume);
        }

        // Dash
        if (movement.CurrentState == CharacterState.Dashing &&
            previousState != CharacterState.Dashing)
        {
            PlayEffect(dashClip, dashVolume);
        }

        // Landing
        if (!previousGrounded && movement.IsGrounded)
        {
            PlayEffect(landClip, landVolume);
        }
    }

    private void PlayEffect(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        effectsSource.pitch = Random.Range(minPitch, maxPitch);
        effectsSource.PlayOneShot(clip, volume);
    }
}
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement2D movement;
    private CharacterStats stats;

    private CharacterState previousState;

    private static readonly int StateHash = Animator.StringToHash("State");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<CharacterMovement2D>();
        stats = GetComponentInParent<CharacterStats>();

        if (movement == null)
        {
            Debug.LogError("CharacterAnimation requires CharacterMovement2D in parent");
            enabled = false;
            return;
        }

        previousState = movement.CurrentState;
        animator.SetInteger(StateHash, (int)previousState);
    }

    private void Update()
    {
        if (movement.CurrentState != previousState)
        {
            animator.SetInteger(StateHash, (int)movement.CurrentState);

            previousState = movement.CurrentState;
        }
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DieHash);
    }

    public void DeathAnimationFinished()
    {
        stats.FinishDeath();
    }
}
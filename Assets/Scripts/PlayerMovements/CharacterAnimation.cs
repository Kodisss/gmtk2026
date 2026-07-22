using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement2D movement;

    private CharacterState previousState;
    private float previousSpeed = 0f;

    private static readonly int StateHash = Animator.StringToHash("State");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<CharacterMovement2D>();
    }

    private void Update()
    {
        animator.SetBool(GroundedHash, movement.IsGrounded);

        if (movement.CurrentState != previousState)
        {
            animator.SetInteger(StateHash, (int)movement.CurrentState);
            previousState = movement.CurrentState;
        }

        float currentSpeed = Mathf.Abs(movement.Velocity.x);

        if (previousSpeed != currentSpeed)
        {
            animator.SetFloat(SpeedHash, currentSpeed);
            previousSpeed = currentSpeed;
        }
    }
}
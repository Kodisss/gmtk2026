using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement2D movement;

    private CharacterState previousState;

    private static readonly int StateHash = Animator.StringToHash("State");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int DashHash = Animator.StringToHash("Dash");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<CharacterMovement2D>();

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
        animator.SetBool(GroundedHash, movement.IsGrounded);

        if (movement.CurrentState != previousState)
        {
            animator.SetInteger(StateHash, (int)movement.CurrentState);

            if (movement.CurrentState == CharacterState.Dashing)
            {
                animator.SetTrigger(DashHash);
            }

            previousState = movement.CurrentState;
        }

        animator.SetFloat(SpeedHash, movement.CurrentSpeed / movement.MaxSpeed);
    }
}
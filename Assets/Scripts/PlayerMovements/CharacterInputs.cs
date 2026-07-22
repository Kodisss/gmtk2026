using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputs : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    public bool IsSprinting { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }


    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
        }

        if (context.canceled)
        {
            IsSprinting = false;
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpPressed = true;
        }

        if (context.canceled)
        {
            JumpReleased = true;
        }
    }


    private void LateUpdate()
    {
        JumpPressed = false;
        JumpReleased = false;
    }
}
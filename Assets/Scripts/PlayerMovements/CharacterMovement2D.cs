using UnityEngine;

public enum CharacterState
{
    Idle, // 0
    Walking, // 1
    Sprinting, // 2
    Jumping, // 3
    Falling // 4
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInputs))]
public class CharacterMovement2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterInputs characterInput;
    private SpriteRenderer spriteRenderer;

    private CharacterState currentState;

    [Header("Movement")]
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 8f;
    [SerializeField] private float acceleration = 150f;
    [SerializeField] private float deceleration = 150f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpGravityMultiplier = 4f;
    [SerializeField] private float fallGravityMultiplier = 2.5f;
    [SerializeField] private float lowJumpGravityMultiplier = 2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Double Jump")]
    [SerializeField] private float doubleJumpForce = 18f;
    [SerializeField] private float doubleJumpCooldown = 0.2f;

    private bool canDoubleJump;
    private float doubleJumpTimer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private float coyoteTimer;
    private float jumpBufferTimer;

    private float currentHorizontalSpeed;
    private bool isGrounded;

    // Access for animmations
    public CharacterState CurrentState => currentState;
    public Vector2 Velocity => rb.linearVelocity;
    public bool IsGrounded => isGrounded;
    public bool IsMoving => Mathf.Abs(rb.linearVelocity.x) > 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterInput = GetComponent<CharacterInputs>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentState = CharacterState.Idle;
    }

    private void Update()
    {
        CheckGround();

        HandleJumpBuffer();
        HandleCoyoteTime();
        HandleJump();
        HandleDoubleJumpCooldown();
        HandleBetterJump();

        HandleSpriteFlip();

        UpdateState();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleSpriteFlip()
    {
        if (characterInput.MoveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (characterInput.MoveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void HandleMovement()
    {
        //Debug.Log($"Move Input: {characterInput.MoveInput}");

        float targetSpeed = 0f;

        if (Mathf.Abs(characterInput.MoveInput.x) > 0.01f)
        {
            targetSpeed = characterInput.IsSprinting ? sprintingSpeed : walkingSpeed;
            targetSpeed *= Mathf.Sign(characterInput.MoveInput.x);
        }

        float rate = Mathf.Abs(targetSpeed) > Mathf.Abs(currentHorizontalSpeed) ? acceleration : deceleration;

        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, targetSpeed, rate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(currentHorizontalSpeed, rb.linearVelocity.y);
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = true;
            doubleJumpTimer = 0f;
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    private void HandleJumpBuffer()
    {
        if (characterInput.JumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;
    }

    private void HandleJump()
    {
        if (jumpBufferTimer <= 0)
            return;

        // Normal jump
        if (coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            return;
        }

        // Double jump
        if (canDoubleJump && doubleJumpTimer <= 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                doubleJumpForce
            );

            canDoubleJump = false;
            doubleJumpTimer = doubleJumpCooldown;

            jumpBufferTimer = 0f;
        }
    }

    private void HandleDoubleJumpCooldown()
    {
        if (doubleJumpTimer > 0)
        {
            doubleJumpTimer -= Time.deltaTime;
        }
    }

    private void HandleBetterJump()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling
            rb.gravityScale = fallGravityMultiplier;
        }
        else if (characterInput.JumpReleased && rb.linearVelocity.y > 0)
        {
            // Released early
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            rb.gravityScale = lowJumpGravityMultiplier;
        }
        else
        {
            // Rising
            rb.gravityScale = jumpGravityMultiplier;
        }
    }

    private void UpdateState()
    {
        if (!isGrounded)
        {
            currentState = rb.linearVelocity.y > 0 ? CharacterState.Jumping : CharacterState.Falling;
            return;
        }

        if (Mathf.Abs(currentHorizontalSpeed) < 0.1f)
        {
            currentState = CharacterState.Idle;
        }
        else if (characterInput.IsSprinting)
        {
            currentState = CharacterState.Sprinting;
        }
        else
        {
            currentState = CharacterState.Walking;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
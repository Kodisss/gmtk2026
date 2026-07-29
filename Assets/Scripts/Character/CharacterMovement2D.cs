using System.Collections;
using UnityEngine;

public enum CharacterState
{
    Idle, // 0
    Walking, // 1
    Jumping, // 2
    Falling, // 3
    Dashing, // 4
    Landing // 5
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInputs))]
[RequireComponent(typeof(CharacterStats))]
public class CharacterMovement2D : MonoBehaviour
{
    // public access to completely make it impossible for the character to move
    public bool CanMove { get; private set; } = true;

    // access to important stuff
    private Rigidbody2D rb;
    private CharacterInputs characterInput;
    private SpriteRenderer spriteRenderer;
    private MoveCamera moveCamera;

    // access to metadata on the game and character
    private CharacterStats stats;
    private CharacterState currentState;

    [Header("Movement")]
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float acceleration = 150f;
    [SerializeField] private float deceleration = 150f;

    [Header("Jump")]
    [SerializeField] private float baseGravity = 3f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpGravityMultiplier = 4f;
    [SerializeField] private float fallGravityMultiplier = 2.5f;
    [SerializeField] private float lowJumpGravityMultiplier = 2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float landingVelocityThresholdToLand = -8f;
    [SerializeField] private float landingDuration = 0.08f;

    private bool isJumping;

    private float landingTimer;
    private float lastVerticalVelocity;

    [Header("Double Jump")]
    [SerializeField] private bool allowedToDoubleJump = false;
    [SerializeField] private float doubleJumpForce = 18f;
    [SerializeField] private float doubleJumpCooldown = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float groundDashCooldown = 0.5f;

    private bool dashStartedGrounded;
    private float groundDashTimer;
    private bool isDashing;
    public bool CanDash { get; set; } = true;
    private Vector2 dashDirection;

    private bool canDoubleJump;
    private float doubleJumpTimer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.15f);
    [SerializeField] private float groundCheckXOffset = 0f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera Offsets")]
    [SerializeField] private float walkingCameraOffset = 2f;
    [SerializeField] private float idleCameraOffset = 0f;

    public float CurrentSpeed => Mathf.Abs(currentHorizontalSpeed);
    public float MaxSpeed => walkingSpeed;

    private float coyoteTimer;
    private float jumpBufferTimer;

    private float currentHorizontalSpeed;
    private bool isGrounded;

    // Access for animations
    public CharacterState CurrentState => currentState;
    public Vector2 Velocity => rb.linearVelocity;
    public bool IsGrounded => isGrounded;
    public bool IsMoving => Mathf.Abs(rb.linearVelocity.x) > 0.1f;

    // Initialize everthing
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterInput = GetComponent<CharacterInputs>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        moveCamera = Camera.main.transform.GetComponent<MoveCamera>();
        stats = GetComponent<CharacterStats>();
        currentState = CharacterState.Idle;
    }

    private void Update()
    {
        if (!CanMove) return;

        lastVerticalVelocity = rb.linearVelocity.y;

        CheckGround();
        HandleGroundDashCooldown();
        HandleDash();

        HandleJumpBuffer();
        HandleCoyoteTime();
        HandleJump();

        if (stats.DoubleJumpEnabled) HandleDoubleJumpCooldown();

        HandleBetterJump();

        HandleSpriteFlip();

        if (landingTimer > 0f)
        {
            landingTimer -= Time.deltaTime;
        }

        UpdateState();
        UpdateCameraOffset();
    }

    private void FixedUpdate()
    {
        if(!CanMove) return; 
        HandleMovement();
    }

    // method for the access to CanMove globally
    public void SetMovementEnabled(bool enabled)
    {
        CanMove = enabled;

        if (!enabled)
        {
            rb.linearVelocity = Vector2.zero;
            currentHorizontalSpeed = 0f;
            currentState = CharacterState.Idle;
        }
    }

    // this flips the character sprite based on inputs
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

    // Everything that makes the character move around
    private void HandleMovement()
    {
        if (isDashing) return; // we don't do anything if we're dashing

        float targetSpeed = 0f; // we set a target speed to accelerate to

        // only if we're moving and we get a deadzone in case
        if (Mathf.Abs(characterInput.MoveInput.x) > 0.01f)
        {
            targetSpeed = walkingSpeed; // set the target speed
            targetSpeed *= Mathf.Sign(characterInput.MoveInput.x); // set the direction
            targetSpeed *= stats.SpeedMultiplier; // add any multiplier
        }

        // check if we're going faster or slower to use the acceleration stat or deceleration stat for the rate
        float rate = Mathf.Abs(targetSpeed) > Mathf.Abs(currentHorizontalSpeed) ? acceleration : deceleration;

        // get the speed towards the target speed at the acceleration or deceleration rate
        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, targetSpeed, rate * Time.fixedDeltaTime);

        // update the velocity of the rigibody
        rb.linearVelocity = new Vector2(currentHorizontalSpeed, rb.linearVelocity.y);
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(GroundCheckPosition(), groundCheckSize, 0f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            CanDash = true;

            canDoubleJump = true;
            doubleJumpTimer = 0f;
            isJumping = false;

            if (lastVerticalVelocity < landingVelocityThresholdToLand)
            {
                landingTimer = landingDuration;
            }
        }
    }

    private Vector2 GroundCheckPosition()
    {
        return groundCheck.position + new Vector3(groundCheckXOffset, 0f, 0f);
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
        if (jumpBufferTimer <= 0) return;

        // Normal jump
        if (coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            return;
        }

        // Double jump
        if(!stats.DoubleJumpEnabled) return;

        if (canDoubleJump && doubleJumpTimer <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
            isJumping = true;

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
        if (characterInput.JumpReleased && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.gravityScale = baseGravity * jumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void HandleDash()
    {
        if (!characterInput.DashPressed) return;

        if (!CanDash)  return;

        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        dashStartedGrounded = isGrounded;

        isDashing = true;
        isJumping = false;
        CanDash = false;

        currentState = CharacterState.Dashing;

        dashDirection = new Vector2(
            characterInput.MoveInput.x,
            characterInput.MoveInput.y
        );

        if (dashDirection == Vector2.zero)
        {
            dashDirection = spriteRenderer.flipX
                ? Vector2.left
                : Vector2.right;
        }

        rb.gravityScale = 0;
        rb.linearVelocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rb.gravityScale = baseGravity;


        // Ground dash: wait before allowing another dash
        if (dashStartedGrounded)
        {
            yield return new WaitForSeconds(groundDashCooldown);

            if (isGrounded)
            {
                CanDash = true;
            }
        }
    }

    private void HandleGroundDashCooldown()
    {
        if (groundDashTimer > 0)
        {
            groundDashTimer -= Time.deltaTime;

            if (groundDashTimer <= 0 && isGrounded)
            {
                CanDash = true;
            }
        }
    }

    private void UpdateState()
    {
        if (isDashing)
        {
            currentState = CharacterState.Dashing;
            return;
        }

        if (landingTimer > 0f)
        {
            currentState = CharacterState.Landing;
            return;
        }

        if (!isGrounded)
        {
            currentState = isJumping && rb.linearVelocity.y > 0 ? CharacterState.Jumping : CharacterState.Falling;
            return;
        }

        if (Mathf.Abs(currentHorizontalSpeed) < 0.1f)
        {
            currentState = CharacterState.Idle;
        }
        else
        {
            currentState = CharacterState.Walking;
        }
    }

    public void StopMovement()
    {
        Debug.Log("StopMovements");

        StopAllCoroutines();

        Debug.Log("StoppedCoroutines");

        isDashing = false;
        isJumping = false;

        CanDash = false;

        // currentState = CharacterState.Idle;

        currentHorizontalSpeed = 0;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        rb.gravityScale = baseGravity;
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force,ForceMode2D.Impulse);
    }

    private void UpdateCameraOffset()
    {
        if (moveCamera == null) return;

        if (currentState == CharacterState.Walking)
        {
            walkingCameraOffset *= - Mathf.Sign(characterInput.MoveInput.x); // set the direction
            moveCamera.SetOffset(walkingCameraOffset);
        }
        else if (currentState == CharacterState.Idle)
        {
            idleCameraOffset *= Mathf.Sign(characterInput.MoveInput.x); // set the direction
            moveCamera.SetOffset(idleCameraOffset);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            GroundCheckPosition(),
            groundCheckSize
        );
    }
}
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 StartPoint;
    private SpriteRenderer sr;
    private Collider2D col;

    // Character Control
    private float horizontalInput;
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public int facingDirection;

    // --- Jump Feel (Dawnosaur-style, embedded) ---
    [Header("Jump Feel")]
    [Tooltip("Desired jump height in Unity units.")]
    public float jumpHeight = 3.5f;

    [Tooltip("Time to reach the apex of the jump.")]
    public float jumpTimeToApex = 0.35f;

    [Tooltip("Grace period after leaving ground where jump still works.")]
    [Range(0.01f, 0.5f)] public float coyoteTime = 0.12f;

    [Tooltip("Grace period after pressing jump where jump will trigger when allowed.")]
    [Range(0.01f, 0.5f)] public float jumpBufferTime = 0.12f;

    [Header("Gravity Multipliers")]
    public float fallGravityMult = 1.8f;
    public float maxFallSpeed = 18f;

    public float fastFallGravityMult = 2.5f;
    public float maxFastFallSpeed = 25f;

    [Tooltip("Higher gravity when jump is released early.")]
    public float jumpCutGravityMult = 2.0f;

    [Tooltip("Lower gravity near apex for a tiny 'hang' feel (0.7-1.0).")]
    [Range(0f, 1f)] public float jumpHangGravityMult = 0.85f;

    [Tooltip("If |vy| < threshold, we consider it near apex.")]
    public float jumpHangTimeThreshold = 1.0f;

    // Computed physics
    private float gravityStrength;
    private float gravityScale;
    private float jumpForce;

    // Jump state + timers (same idea as PlayerMovement)
    public bool IsOnGround;
    private bool IsJumping;
    private bool _isJumpCut;
    private bool _isJumpFalling;

    private float LastOnGroundTime;
    private float LastPressedJumpTime;

    // Double Jump
    [Header("Double Jump")]
    public int MaxJumps = 2;
    private int JumpsLeft;

    // Checks (instead of "any collision = ground", we do proper check)
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    // Elevator
    private bool moveUp;
    private GameObject elev;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        StartPoint = transform.position;
        facingDirection = 1;

        RecalculateJumpPhysics();
        rb.gravityScale = gravityScale;

        JumpsLeft = MaxJumps;
    }

    void Update()
    {
        // Timers
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        // Control of the character
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0.1f && facingDirection == -1) Flip();
        else if (horizontalInput < -0.1f && facingDirection == 1) Flip();

        // --- Ground Check (proper) ---
        bool groundedNow = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        if (groundedNow)
        {
            IsOnGround = true;
            LastOnGroundTime = coyoteTime;
            JumpsLeft = MaxJumps;

            _isJumpCut = false;
            _isJumpFalling = false;
            if (!IsJumping) IsJumping = false;
        }
        else
        {
            IsOnGround = false;
        }

        // --- Input Buffer ---
        if (Input.GetButtonDown("Jump"))
        {
            LastPressedJumpTime = jumpBufferTime;
        }

        // Jump cut input (release jump early)
        if (Input.GetButtonUp("Jump"))
        {
            if (CanJumpCut())
                _isJumpCut = true;
        }

        // Track jump state transitions
        if (IsJumping && rb.linearVelocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;
        }

        // --- Jump Execution (ground/coyote OR double jump) ---
        if (LastPressedJumpTime > 0)
        {
            if (CanGroundJump())
            {
                DoJump();
            }
            else if (CanDoubleJump())
            {
                DoDoubleJump();
            }
        }

        // --- Gravity handling (Dawnosaur-style) ---
        ApplyBetterGravity();

        // Elevator
        if (moveUp && elev != null && elev.transform.position.y < 12.7f)
        {
            Vector3 targetPos = new Vector3(3.5f, 12.7f, 0f);
            elev.transform.position = Vector3.MoveTowards(
                elev.transform.position,
                targetPos,
                speed * 0.1f * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    // Flip Character
    void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }

    // --- Jump Methods (embedded logic) ---
    private void DoJump()
    {
        // consume buffered input + coyote
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        IsJumping = true;
        _isJumpCut = false;
        _isJumpFalling = false;

        // make jump consistent if falling
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void DoDoubleJump()
    {
        LastPressedJumpTime = 0;

        IsJumping = true;
        _isJumpCut = false;
        _isJumpFalling = false;

        // consume one air jump
        JumpsLeft--;

        // reset y so double jump feels crisp
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CanGroundJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanDoubleJump()
    {
        // if not on ground/coyote, allow remaining jumps
        return LastOnGroundTime <= 0 && JumpsLeft > 0;
    }

    private bool CanJumpCut()
    {
        return (IsJumping || _isJumpFalling) && rb.linearVelocity.y > 0;
    }

    private void ApplyBetterGravity()
    {
        // Fast fall if holding down
        if (rb.linearVelocity.y < 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            rb.gravityScale = gravityScale * fastFallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFastFallSpeed));
            return;
        }

        // Jump cut: released jump early
        if (_isJumpCut)
        {
            rb.gravityScale = gravityScale * jumpCutGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
            return;
        }

        // Jump hang near apex
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(rb.linearVelocity.y) < jumpHangTimeThreshold)
        {
            rb.gravityScale = gravityScale * jumpHangGravityMult;
            return;
        }

        // Falling
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
            return;
        }

        // Default
        rb.gravityScale = gravityScale;
    }

    private void RecalculateJumpPhysics()
    {
        // Same formulas as PlayerData.OnValidate()
        gravityStrength = -(2f * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravityScale = gravityStrength / Physics2D.gravity.y;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }

    // Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Moving platform
        if (collision.gameObject.CompareTag("MovingPlateform"))
        {
            transform.SetParent(collision.transform);
        }

        // Notes
        if (collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlateform"))
        {
            transform.SetParent(null);
        }
    }

    // Elevator + Death
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Elevator"))
        {
            moveUp = true;
            elev = other.gameObject.transform.parent.gameObject;
        }

        if (other.CompareTag("Death"))
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        sr.enabled = false;
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        yield return new WaitForSeconds(1f);

        transform.position = StartPoint;
        rb.simulated = true;
        sr.enabled = true;
        col.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }
}

using UnityEngine;

public class Patroller2D : MonoBehaviour
{
    public float speed = 2f;

    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;

    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;

    public LayerMask groundLayer;

    [Header("Anti-wiggle")]
    public float turnCooldown = 0.15f;

    private Rigidbody2D rb;
    private int direction = 1;
    private float nextTurnTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        bool groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        bool wallAhead = false;
        if (wallCheck != null)
            wallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, wallCheckDistance, groundLayer);

        if (Time.time >= nextTurnTime && (!groundAhead || wallAhead))
        {
            TurnAround();
            nextTurnTime = Time.time + turnCooldown;
        }
    }

    private void TurnAround()
    {
        direction *= -1;

        // Flip sprite
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * direction;
        transform.localScale = s;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.cyan;
            // Can't use "direction" reliably in edit mode, so show both
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectibleNotes.Instance.LooseNote();
        }
    }
}
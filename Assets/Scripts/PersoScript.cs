using System.Collections;
using UnityEngine;

public class PersoScript : MonoBehaviour
{
    public Vector3 StartPoint;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;
    //Character Control
    private float horizontalInput;
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public float jump = 5f;
    public int facingDirection;
    //Double Jump
    public bool IsOnGround;
    public int MaxJumps = 2;
    private int JumpsLeft;
    private bool moveUp;
    private GameObject elev;
    //public bool isOnGround; Later
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        StartPoint = transform.position;
        anim = GetComponent<Animator>();
        facingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Control of the character
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.1f && facingDirection == -1)
            Flip();
        else if (horizontalInput < -0.1f && facingDirection == 1)
            Flip();
        if (horizontalInput == 0f)
        { anim.speed = 0f; }
        else
        {
            anim.speed = 1f;
        }

        // Simple, snappy jump (single or air jumps via JumpsLeft)
        if (UnityEngine.Input.GetButtonDown("Jump") && JumpsLeft > 0)
        {
            anim.speed = 0f;
            // treat 'jump' as desired jump height (units)
            float desiredJumpHeight = jump; // reuse existing public field
            float g = -Physics2D.gravity.y * rb.gravityScale; // positive gravity magnitude
            float jumpVelocity = Mathf.Sqrt(2f * g * Mathf.Max(0.1f, desiredJumpHeight));

            // apply an immediate, consistent vertical velocity (nice crisp feel)
            rb.linearVelocity = new Vector2(horizontalInput * speed, jumpVelocity);

            JumpsLeft--;
            IsOnGround = false;
        }

        // Jump cut: releasing jump makes the ascent shorter
        if (UnityEngine.Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Clamp fall speed to keep falling snappy and controllable
        float maxFall = -12f;
        if (rb.linearVelocity.y < maxFall)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFall);

        //Elevator
        if (moveUp&& elev.transform.position.y <  12.7f)
        {
            Vector3 targetPos =new Vector3(3.5f, 12.7f, 0f);
            elev.transform.position = Vector3.MoveTowards
                (
                    transform.position,
                    targetPos,
                    speed *0.1f* Time.deltaTime
                );
        }
    }

    //Flip Character
    void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }
    void FixedUpdate() 
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }
    //Double Jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        anim.speed = 1f;
        IsOnGround = true;
        JumpsLeft = MaxJumps;

        //Moving pateform
        if (collision.gameObject.CompareTag("MovingPlateform"))
        {transform.SetParent(collision.transform);}

        //Notes
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(collision.gameObject);
            //AudioPig.PlayOneShot(Collect);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlateform"))
        {
            transform.SetParent(null);
        }
    }

    //Elevator
    void OnTriggerEnter2D(Collider2D other)
    {
        //Elevator
        if (other.CompareTag("Elevator"))
        {
            moveUp = true;
            elev = other.gameObject.transform.parent.gameObject;
        }
        //Fall to Death
        if (other.CompareTag("Death"))
        {StartCoroutine(DieAndRespawn());}
    }
    //Fall to Death
    IEnumerator DieAndRespawn()
    {
        // "Destroy" the player
        sr.enabled = false;
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        yield return new WaitForSeconds(1f);

        // Respawn
        transform.position = StartPoint;
        rb.simulated = true;
        sr.enabled = true;
        col.enabled = true;
    }
}

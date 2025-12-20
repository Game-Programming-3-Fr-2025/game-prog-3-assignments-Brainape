using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveForce = 10f;
    public float maxSpeed = 5f;
    public float topGravity = 1f;
    public float bottomGravity = -1f;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    public Transform startPosition; 

    private Rigidbody2D rb;
    private float lastJumpTime = -Mathf.Infinity;

    private int jumpCount = 0;
    private int maxJumps = 1;
    public PlatformShooter platformShooter;
    public Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * moveInput * moveForce);

        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            && Time.time >= lastJumpTime + jumpCooldown
            && jumpCount < maxJumps)
        {
            Vector2 jumpDirection = rb.gravityScale > 0 ? Vector2.up : Vector2.down;
            anim.Play("jump");
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Top"))
        {
            rb.gravityScale = topGravity;
            jumpCount = 0;
            anim.Play("land");

        }
        else if (collision.collider.CompareTag("Bottom"))
        {
            rb.gravityScale = bottomGravity;
            jumpCount = 0;
        }
        else if (collision.collider.CompareTag("Death"))
        {
            Respawn();
            ScreenShake2D.Instance.Shake(0.5f, 0.3f);
        }


    }

    void Respawn()
    {
        if (startPosition != null)
        {
            transform.position = startPosition.position;
            rb.linearVelocity = Vector2.zero;
            platformShooter.ammo -= 1;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ammo"))
        {
            if (platformShooter != null)
            {
                CameraZoom2D.Instance.Zoom();
                platformShooter.AddAmmo(2); 
            }
            Destroy(other.gameObject); 
        }
    }
}

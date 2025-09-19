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
    private Rigidbody2D rb;
    private float lastJumpTime = -Mathf.Infinity;

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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCooldown)
        {
            Vector2 jumpDirection = rb.gravityScale > 0 ? Vector2.up : Vector2.down;

            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Top"))
        {
            rb.gravityScale = topGravity;
        }
        else if (collision.collider.CompareTag("Bottom"))
        {
            rb.gravityScale = bottomGravity;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        moveInput.x = Input.GetAxisRaw("Horizontal"); 
        moveInput.y = Input.GetAxisRaw("Vertical");   

        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}

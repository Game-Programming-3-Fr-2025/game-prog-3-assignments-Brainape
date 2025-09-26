using UnityEngine;

public class BallController : MonoBehaviour
{
    public float launchForce = 10f;
    private Rigidbody2D rb;
    private Transform attachedCircle;
    private Vector2 localOffset; 
    private bool isAttached;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAttached)
        {
            transform.position = attachedCircle.TransformPoint(localOffset);

            Vector2 direction = (transform.position - attachedCircle.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (Input.GetMouseButtonDown(0))
            {
                Launch(direction);
            }
        }
    }

    private void Launch(Vector2 direction)
    {
        isAttached = false;
        attachedCircle = null;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = direction * launchForce;
    }

    private void AttachToCircle(Transform circle)
    {
        attachedCircle = circle;
        isAttached = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        localOffset = circle.InverseTransformPoint(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttached && other.CompareTag("Circle"))
        {
            AttachToCircle(other.transform);
        }
    }
}

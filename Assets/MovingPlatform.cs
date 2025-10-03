using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 5f;    

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float maxRange;
    private bool isMoving = true;

    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void Initialize(Vector3 start, Vector3 target, float range)
    {
        startPosition = start;
        targetPosition = target;
        maxRange = range;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 direction = (targetPosition - startPosition).normalized;

        Vector3 step = direction * moveSpeed * Time.deltaTime;
        transform.position += step;

        float distanceFromStart = Vector3.Distance(startPosition, transform.position);
        if (distanceFromStart >= maxRange)
        {
            StopPlatform();
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget <= 0.1f) 
        {
            transform.position = targetPosition; 
            StopPlatform();
        }
    }

    void StopPlatform()
    {
        isMoving = false;
        col.enabled = true; 
    }
}

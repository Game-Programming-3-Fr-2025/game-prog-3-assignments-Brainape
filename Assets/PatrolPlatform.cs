using UnityEngine;

public class PatrolPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f; 
    public float closeEnoughDistance = 0.1f; 

    private Transform targetPoint;

    void Start()
    {
        targetPoint = pointA;
    }

    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < closeEnoughDistance)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }
}

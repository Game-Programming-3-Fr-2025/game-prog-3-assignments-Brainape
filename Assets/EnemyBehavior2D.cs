using UnityEngine;

public class EnemyBehavior2D : MonoBehaviour
{
    public enum EnemyState { RunAway, Fire }
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 5f;


    public float moveDuration = 2f;
    public Vector2 moveDirection = Vector2.right; 
    public float runAwayRadius = 5f;            
    public float runAwayDuration = 3f;     
    
    public float fireDuration = 2f;            
    public float alwaysRunAwayDistance = 3f;    
    
    public float fireRate = 0.5f;
    private float fireCooldown;
    private EnemyState currentState;
    private float stateTimer;

    private Rigidbody2D rb;

    private bool isMovingPhase = false; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Fire;
        stateTimer = fireDuration;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= alwaysRunAwayDistance)
        {
            SwitchState(EnemyState.RunAway);
        }

        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.RunAway:
                RunAwayBehavior(distance);
                break;
            case EnemyState.Fire:
                FireBehavior(distance);
                break;
        }
    }

    void RunAwayBehavior(float distance)
    {
        Vector2 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);

        if (stateTimer <= 0f)
        {
            SwitchState(EnemyState.Fire);
        }
    }

    void FireBehavior(float distance)
    {
        if (distance <= runAwayRadius)
        {
            SwitchState(EnemyState.RunAway);
            return;
        }

        Vector2 lookDir = (player.position - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }

        if (!isMovingPhase && stateTimer <= 0f)
        {
            StartCoroutine(MoveThenFireAgain());
        }
    }

    System.Collections.IEnumerator MoveThenFireAgain()
    {
        isMovingPhase = true;
        float t = 0f;

        while (t < moveDuration)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        isMovingPhase = false;
        SwitchState(EnemyState.Fire);
    }

    void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void SwitchState(EnemyState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case EnemyState.RunAway:
                stateTimer = runAwayDuration;
                break;
            case EnemyState.Fire:
                stateTimer = fireDuration;
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runAwayRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alwaysRunAwayDistance);
    }
}

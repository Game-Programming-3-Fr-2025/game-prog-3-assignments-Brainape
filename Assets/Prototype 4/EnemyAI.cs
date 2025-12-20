using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    private enum TargetType { Harvester, Player }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform player; 
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackStopThreshold = 0.1f; 
    private bool isKnockback = false;

    private TargetType currentTargetType = TargetType.Harvester;
    private Transform currentTarget;
    private float lastAttackTime;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        FindNewHarvesterTarget();
    }

    private void Update()
    {
        if (isKnockback)
        {
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.magnitude <= knockbackStopThreshold)
            {
                isKnockback = false;
                rb.linearVelocity = Vector3.zero;
            }
            return; 
        }

        if (currentTarget == null)
        {
            if (currentTargetType == TargetType.Harvester)
                FindNewHarvesterTarget();
            return;
        }

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);

            rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }

        TryAttack();
    }

    private void TryAttack()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Health targetHealth = currentTarget.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(attackDamage);
                    FeedbacksManager.Instance.Play("DamageFeedback", transform.position);

                }
                lastAttackTime = Time.time;
            }
        }
    }

    private void FindNewHarvesterTarget()
    {
        GameObject[] harvesters = GameObject.FindGameObjectsWithTag("Harvester");
        if (harvesters.Length == 0)
        {
            currentTarget = null;
            return;
        }

        currentTarget = harvesters
            .OrderBy(h => Vector3.Distance(transform.position, h.transform.position))
            .First()
            .transform;
        currentTargetType = TargetType.Harvester;
    }

    public void AggroToPlayer(Transform playerTransform)
    {
        player = playerTransform;
        currentTarget = player;
        currentTargetType = TargetType.Player;
    }

    public void TakeDamage(int amount, Transform attacker = null, bool applyKnockback = false)
    {
        health.TakeDamage(amount);

        if (attacker != null)
            AggroToPlayer(attacker);

        if (applyKnockback && attacker != null && rb != null)
        {
            Vector3 knockDir = (transform.position - attacker.position).normalized;
            knockDir.y = 0; 
            rb.linearVelocity = Vector3.zero; 
            rb.AddForce(knockDir * knockbackForce, ForceMode.Impulse);
            isKnockback = true;
        }
    }

}

using UnityEngine;
using System.Collections.Generic;

public class StationaryProjectile : MonoBehaviour
{
    [Header("Stationary Settings")]
    [SerializeField] private int initialDamage = 20;          // damage when first triggered
    [SerializeField] private int expansionDamage = 1;         // damage per tick while expanding
    [SerializeField] private float expansionTime = 1.5f;      // time to fully expand
    [SerializeField] private float refundRadius = 5f;         // player must be within this to get refund
    [SerializeField] private float damageTickInterval = 0.2f; // seconds between applying damage during expansion
    [SerializeField] private float lifetime = 6f;             // seconds before auto-destroy

    private Transform owner;
    private int energyCost;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool isTriggered = false;
    private float expansionTimer = 0f;
    private float damageTimer = 0f;
    private float lifeTimer = 0f;

    private void Start()
    {
        initialScale = transform.localScale;
        float diameter = refundRadius * 2f;
        targetScale = Vector3.one * diameter;
    }

    public void Initialize(Transform ownerTransform, int cost)
    {
        owner = ownerTransform;
        energyCost = cost;
    }

    private void Update()
    {
        // Update lifetime
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            TryRefundEnergy();
            Destroy(gameObject);
            return;
        }

        if (!isTriggered) return;

        // Expand the projectile
        expansionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(expansionTimer / expansionTime);
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

        // Apply damage to enemies inside during expansion
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageTickInterval)
        {
            ApplyExpansionDamage();
            damageTimer = 0f;
        }

        // Destroy after fully expanded
        if (t >= 1f)
        {
            TryRefundEnergy();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform == owner) return; // ignore owner
        if (isTriggered) return;

        bool hitSomething = false;

        var enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(initialDamage, owner, false); // false = no knockback
            hitSomething = true;
        }

        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(initialDamage);
            hitSomething = true;
        }

        if (other.GetComponent<PlayerProjectile>() != null)
        {
            hitSomething = true;
        }

        if (hitSomething)
        {
            TriggerExpansion();
        }
    }

    private void TriggerExpansion()
    {
        isTriggered = true;
        expansionTimer = 0f;
        damageTimer = 0f;
    }

    private void ApplyExpansionDamage()
    {
        float radius = transform.localScale.x / 2f;
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        HashSet<EnemyAI> damagedEnemies = new HashSet<EnemyAI>();

        foreach (Collider col in hits)
        {
            EnemyAI enemy = col.GetComponent<EnemyAI>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(expansionDamage, owner);
                damagedEnemies.Add(enemy);
            }
        }
    }

    private void TryRefundEnergy()
    {
        if (owner == null) return;

        float dist = Vector3.Distance(owner.position, transform.position);
        if (dist <= refundRadius)
        {
            LaserShooter shooter = owner.GetComponent<LaserShooter>();
            if (shooter != null)
            {
                int refundAmount = Mathf.FloorToInt(energyCost / 2f);
                shooter.AddNukeReserves(refundAmount);
            }
        }
    }
}

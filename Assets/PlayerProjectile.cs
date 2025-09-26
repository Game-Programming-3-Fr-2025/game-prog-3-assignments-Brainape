using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;
    private Transform owner;

    public void Initialize(Transform ownerTransform, float life = 3f)
    {
        owner = ownerTransform;
        lifetime = life;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore hitting the owner
        if (owner != null && other.transform == owner) return;

        // Prefer enemy-specific logic (so they can aggro)
        var enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, owner, true); // true = apply knockback
            Destroy(gameObject);
            return;
        }

        // Otherwise fallback to Health component
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        var cluster = other.GetComponent<NukeCluster>();
        if (cluster != null)
        {
          
            return;
        }
        // If it hits anything else (walls), destroy
        Destroy(gameObject);
    }
}

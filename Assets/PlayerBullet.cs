using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, FindObjectOfType<IsometricPlayerController>().transform);
            Destroy(gameObject);
        }
    }
}

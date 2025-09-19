using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxHealth = 10f;
    public float currentHealth;
    public float damage = 1f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Ship").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float dmg)
    {
        Die();

    }

    public void Die()
    {
        Debug.Log(name + " died!");
        Destroy(gameObject);
    }
}

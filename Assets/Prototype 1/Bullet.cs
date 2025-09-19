using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float lifeTime = 3f;
    public float damage = 5f;

    private float lifeTimer;
    private Vector3 moveDirection = Vector3.right;

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

  
}

using UnityEngine;

public class EnemyBullet2D : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;

    public Vector3 startScale = new Vector3(0.3f, 0.3f, 0.3f);
    public Vector3 endScale = new Vector3(1f, 1f, 1f);
    public float scaleSpeed = 2f;

    private Transform player;
    private Vector2 moveDir;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
        {
            moveDir = (player.position - transform.position).normalized;
        }

        transform.localScale = startScale;
    }

    void Update()
    {
        transform.position += (Vector3)moveDir * speed * Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, endScale, Time.deltaTime * scaleSpeed);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

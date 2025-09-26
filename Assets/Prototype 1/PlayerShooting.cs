using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;      
    public float bulletOffset = 0.5f;   

    private float fireTimer;
    public int lives;
    public TextMeshProUGUI text;
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        text.text = "Lives: " + lives;
        if(lives == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    void Shoot()
    {
        Vector3 rightDir = transform.right;
        Vector3 leftDir = -transform.right;

        Vector3 rightPos = transform.position + rightDir * bulletOffset;
        Vector3 leftPos = transform.position + leftDir * bulletOffset;

        GameObject rightBullet = Instantiate(bulletPrefab, rightPos, Quaternion.identity);
        rightBullet.GetComponent<Bullet>().SetDirection(rightDir);

        GameObject leftBullet = Instantiate(bulletPrefab, leftPos, Quaternion.identity);
        leftBullet.GetComponent<Bullet>().SetDirection(leftDir);
    }

   

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die();
            lives -= 1;
        }
    }
}

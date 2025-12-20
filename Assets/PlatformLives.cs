using UnityEngine;

public class PlatformLives : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int lives;
    public Color Color;
    public SpriteRenderer spriteColor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            lives -= 1;
            spriteColor.color = Color;

        }
    }
}

using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            Destroy(collision.gameObject);
            Animator animator = GetComponentInChildren<Animator>();
            animator.Play("Shake");
        }
        Debug.Log("haha");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            Destroy(collision.gameObject);
            Animator animator = GetComponentInChildren<Animator>();
            animator.Play("Shake");
        }
        Debug.Log("haha");
    }
}

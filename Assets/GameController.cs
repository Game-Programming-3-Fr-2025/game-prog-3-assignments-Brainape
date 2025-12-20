using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Transform player;           
    public Transform targetPosition;   
    public float teleportInterval = 10f; 
    public TextMeshProUGUI textMeshPro;
    public float timeLeft;             
    public Animator anim;
    private void Start()
    {
        timeLeft = teleportInterval;
    }

    private void Update()
    {
        HandleTimer();
    }

    void HandleTimer()
    {
        timeLeft -= Time.deltaTime;
        textMeshPro.text = timeLeft.ToString();
        if (timeLeft <= 0f)
        {
            TeleportPlayer();
            timeLeft = teleportInterval;
        }
    }

    void TeleportPlayer()
    {
        anim.Play("flash");
        if (player != null && targetPosition != null)
        {
            player.position = targetPosition.position;
        }
      
    }
}

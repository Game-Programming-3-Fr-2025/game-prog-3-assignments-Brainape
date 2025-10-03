using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform player;           
    public Transform targetPosition;   
    public float teleportInterval = 10f; 
    public TextMeshProUGUI textMeshPro;
    [Header("Timer Info (read-only)")]
    public float timeLeft;             

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
        if (player != null && targetPosition != null)
        {
            player.position = targetPosition.position;
        }
        else
        {
            Debug.LogWarning("Player or TargetPosition not assigned in GameController!");
        }
    }
}

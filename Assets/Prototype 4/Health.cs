using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public UnityEvent onDeath;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Canvas mainCanvas; 

    private GameObject healthBarInstance;
    private Slider healthSlider;
    private Camera mainCamera;

    private IsometricPlayerController playerController;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<IsometricPlayerController>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // Find main camera
        mainCamera = Camera.main;

        if (mainCanvas == null)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    mainCanvas = canvas;
                    break;
                }
            }
        }

        if (healthBarPrefab != null && mainCanvas != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

            if (healthSlider != null)
                healthSlider.maxValue = maxHealth;
        }
    }

    private void Update()
    {
        if (healthBarInstance == null || mainCamera == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position + Vector3.up * 2f);

        bool isInFront = screenPos.z > 0;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        bool isOnScreen = isInFront &&
                          viewportPos.x >= 0 && viewportPos.x <= 1 &&
                          viewportPos.y >= 0 && viewportPos.y <= 1;

        healthBarInstance.SetActive(isOnScreen);

        if (isOnScreen)
        {
            healthBarInstance.transform.position = screenPos;
            healthSlider.value = currentHealth;
        }
    }


    public void TakeDamage(int amount)
    {
        if (playerController != null && playerController.IsInvulnerable)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(healthBarInstance);
        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NukeCluster : MonoBehaviour
{
    [SerializeField] private int clusterRadiation = 200;
    [SerializeField] private int maxClusterRadiation = 200;
    [SerializeField] private float radiationPerSecond = 5f;
    [SerializeField] private int radiationRegainOnEnemyDeath = 25;
    [SerializeField] private GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthSlider;
    private Camera mainCamera;

    private List<LaserShooter> playersInside = new List<LaserShooter>();
    private HashSet<GameObject> enemiesInside = new HashSet<GameObject>(); 
    private float radiationAccumulator = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        if (healthBarPrefab != null)
        {
            Canvas mainCanvas = FindObjectOfType<Canvas>();
            healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

            if (healthSlider != null)
                healthSlider.maxValue = maxClusterRadiation;

            healthBarInstance.SetActive(false);
        }
    }

    private void Update()
    {
        HandleRadiation();
        UpdateHealthBarUI();

        if (enemiesInside.Count > 0)
        {
            List<GameObject> destroyedEnemies = new List<GameObject>();
            foreach (var enemy in enemiesInside)
            {
                if (enemy == null)
                    destroyedEnemies.Add(enemy);
            }

            foreach (var destroyed in destroyedEnemies)
            {
                RegainRadiation(radiationRegainOnEnemyDeath);
                enemiesInside.Remove(destroyed);
            }
        }
    }

    private void HandleRadiation()
    {
        if (playersInside.Count > 0 && clusterRadiation > 0)
        {
            radiationAccumulator += radiationPerSecond * Time.deltaTime;

            while (radiationAccumulator >= 1f && clusterRadiation > 0)
            {
                int amountToGive = Mathf.Min(1, clusterRadiation);

                foreach (var player in playersInside)
                {
                    if (player.GetNukeReserves() < player.GetMaxNukeReserves())
                    {
                        player.AddNukeReserves(amountToGive);
                        clusterRadiation -= amountToGive;
                    }
                }

                radiationAccumulator -= 1f;
            }
        }
    }

    private void UpdateHealthBarUI()
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
            if (healthSlider != null)
                healthSlider.value = clusterRadiation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LaserShooter shooter = other.GetComponent<LaserShooter>();
            if (shooter != null && !playersInside.Contains(shooter))
                playersInside.Add(shooter);
        }

        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInside.Contains(other.gameObject))
                enemiesInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LaserShooter shooter = other.GetComponent<LaserShooter>();
            playersInside.Remove(shooter);
        }

        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Remove(other.gameObject);
        }
    }

    public void RegainRadiation(int amount)
    {
        clusterRadiation = Mathf.Clamp(clusterRadiation + amount, 0, maxClusterRadiation);

        if (healthSlider != null)
            healthSlider.value = clusterRadiation;
    }
}

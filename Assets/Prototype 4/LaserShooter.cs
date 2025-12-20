using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaserShooter : MonoBehaviour
{
    [SerializeField] private GameObject distanceProjectilePrefab;  
    [SerializeField] private GameObject stationaryProjectilePrefab;  
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private int nukeReserves = 50;      
    [SerializeField] private int maxNukeReserves = 100; 
    [SerializeField] private int distanceShotCost = 10;  
    [SerializeField] private int stationaryShotCost = 5;

    public TextMeshProUGUI radMeter;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        nukeReserves = Mathf.Clamp(nukeReserves, 0, maxNukeReserves);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFireDistance();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TryFireStationary();
        }

        if (radMeter != null)
            radMeter.text = "RADS: " + nukeReserves.ToString();
    }

    private bool TrySpendNuke(int cost)
    {
        if (nukeReserves < cost) return false;
        nukeReserves -= cost;
        return true;
    }

    private void TryFireDistance()
    {
        if (!TrySpendNuke(distanceShotCost))
        {
            return;
        }

        if (distanceProjectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject proj = Instantiate(distanceProjectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed; 
        }
        

        PlayerProjectile projScript = proj.GetComponent<PlayerProjectile>();
        if (projScript != null)
        {
            projScript.Initialize(gameObject.transform, projectileLifetime);
        }
        else
        {
            Destroy(proj, projectileLifetime);
        }
    }

    private void TryFireStationary()
    {
        if (!TrySpendNuke(stationaryShotCost))
        {
            return;
        }

        if (stationaryProjectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject proj = Instantiate(stationaryProjectilePrefab, firePoint.position, firePoint.rotation);

        StationaryProjectile sp = proj.GetComponent<StationaryProjectile>();
        if (sp != null)
        {
            sp.Initialize(gameObject.transform, stationaryShotCost);
        }

    }

    public void AddNukeReserves(int amount)
    {
        if (amount <= 0) return;
        nukeReserves = Mathf.Clamp(nukeReserves + amount, 0, maxNukeReserves);
    }
    public int GetNukeReserves() => nukeReserves;
    public int GetMaxNukeReserves() => maxNukeReserves;
}

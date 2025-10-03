using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformShooter : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;   
    public float maxPlatformRange = 5f; 
    public Camera mainCamera;           
    public TextMeshProUGUI textMeshPro;

    [Header("Ammo Settings")]
    public int ammo = 2;               

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            FirePlatform();
        }

        textMeshPro.text = "Ammo: " + ammo.ToString();
        if (ammo <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    void FirePlatform()
    {
        if (platformPrefab == null || mainCamera == null) return;

        if (ammo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }        GameObject newPlatform = Instantiate(platformPrefab, transform.position, Quaternion.identity);

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        MovingPlatform mp = newPlatform.GetComponent<MovingPlatform>();
        if (mp != null)
        {
            mp.Initialize(transform.position, mouseWorld, maxPlatformRange);
        }





        ammo--; // consume ammo
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        Debug.Log("Ammo added! Current ammo: " + ammo);
    }
}

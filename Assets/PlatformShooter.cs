using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformShooter : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject breakPlatformPrefab;

    public float maxPlatformRange = 5f;
    public Camera mainCamera;
    public TextMeshProUGUI textMeshPro;
    public int ammo = 2;
    public RectTransform crosshairUI;
    public bool platformPrefabOn;
    public Animator animator;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FirePlatform();
        }


        if (crosshairUI != null) UpdateCrosshair();

        textMeshPro.text = "Ammo: " + ammo.ToString();

        if (ammo <= -1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            platformPrefabOn = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            platformPrefabOn = false;
        }
    }

    void FirePlatform()
    {
        if (platformPrefab == null || mainCamera == null || ammo < 0) return;

        if (platformPrefabOn)
        {
            GameObject newPlatform = Instantiate(platformPrefab, transform.position, Quaternion.identity);
            MovingPlatform mp = newPlatform.GetComponent<MovingPlatform>();
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            if (mp != null)
            {
                mp.Initialize(transform.position, mouseWorld, maxPlatformRange);
            }

        }
        else
        {

            GameObject newPlatform = Instantiate(breakPlatformPrefab, transform.position, Quaternion.identity);
            MovingPlatform mp = newPlatform.GetComponent<MovingPlatform>();
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            if (mp != null)
            {
                mp.Initialize(transform.position, mouseWorld, maxPlatformRange);
            }
        }

      


        ammo--;
    }

    void UpdateCrosshair()
    {
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir = mouseWorld - transform.position;
        if (dir.magnitude > maxPlatformRange)
            dir = dir.normalized * maxPlatformRange;

        Vector3 targetWorld = transform.position + dir;
        crosshairUI.position = mainCamera.WorldToScreenPoint(targetWorld);
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        animator.Play("Collect");
    }
}

using UnityEngine;

public class CameraZoom2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomOutSize = 10f;       
    public float zoomDuration = 0.5f;     
    public float holdTime = 0.2f;        

    private Camera cam;
    private Coroutine zoomRoutine;

    private static CameraZoom2D _instance;
    public static CameraZoom2D Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<CameraZoom2D>();
            return _instance;
        }
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

   

    public void Zoom()
    {
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(ZoomRoutine());
    }

    private System.Collections.IEnumerator ZoomRoutine()
    {
        float originalSize = cam.orthographicSize;

        float t = 0f;
        while (t < zoomDuration)
        {
            cam.orthographicSize = Mathf.Lerp(originalSize, zoomOutSize, t / zoomDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = zoomOutSize;

        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        t = 0f;
        while (t < zoomDuration)
        {
            cam.orthographicSize = Mathf.Lerp(zoomOutSize, originalSize, t / zoomDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = originalSize;
    }
}

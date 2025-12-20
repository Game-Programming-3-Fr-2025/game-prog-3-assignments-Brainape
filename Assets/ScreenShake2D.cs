using UnityEngine;

public class ScreenShake2D : MonoBehaviour
{
    public float defaultShakeAmount = 0.2f;   
    public float defaultShakeDuration = 0.2f;
    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    private static ScreenShake2D _instance;
    public static ScreenShake2D Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<ScreenShake2D>();
            return _instance;
        }
    }

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        StartShake(defaultShakeAmount, defaultShakeDuration);
    }

    public void Shake(float amount, float duration)
    {
        StartShake(amount, duration);
    }

    private void StartShake(float amount, float duration)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(amount, duration));
    }

    private System.Collections.IEnumerator ShakeRoutine(float amount, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * amount;
            float offsetY = Random.Range(-1f, 1f) * amount;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

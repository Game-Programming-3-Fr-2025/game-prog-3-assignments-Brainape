using UnityEngine;

public class RotatingCircle : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}

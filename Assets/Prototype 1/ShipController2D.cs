using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController2D : MonoBehaviour
{
  
    public float maxForwardSpeed = 10f;
    public float maxBackwardSpeed = 5f;
    public float baseSpeed = 0f;
    public float accelerationTime = 2f;
    public AnimationCurve accelerationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float turnSpeed = 180f;

    [Range(0f, 1f)] public float turnDampeningOnIdle = 0.5f;

    public float waterDrag = 0.95f;
    public float currentDragMultiplier = 1f;
    private Rigidbody2D rb;
    private float forwardInput;
    private float turnInput;
    private float accelTimer = 0f;
    public float dragCoefficient = 2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        forwardInput = Input.GetAxisRaw("Vertical"); 
        turnInput = -Input.GetAxisRaw("Horizontal"); 
    }

    void FixedUpdate()
    {
        Vector2 forward = transform.up;

        if (forwardInput > 0)
        {
            accelTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(accelTimer / accelerationTime);
            float accelFactor = accelerationCurve.Evaluate(t);

            rb.AddForce(forward * accelFactor * maxForwardSpeed, ForceMode2D.Force);

            if (rb.linearVelocity.magnitude > maxForwardSpeed + baseSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * (maxForwardSpeed + baseSpeed);
        }
        else if (forwardInput < 0)
        {
            accelTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(accelTimer / accelerationTime);
            float accelFactor = accelerationCurve.Evaluate(t);

            rb.AddForce(forward * accelFactor * -maxBackwardSpeed, ForceMode2D.Force);

            if (rb.linearVelocity.magnitude > maxBackwardSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxBackwardSpeed;
        }
        else
        {
            accelTimer = 0f;
        }

        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            Vector2 dragForce = -rb.linearVelocity.normalized * rb.linearVelocity.sqrMagnitude * dragCoefficient * Time.fixedDeltaTime;
            rb.AddForce(dragForce);
        }

        float speedFactor = rb.linearVelocity.magnitude / maxForwardSpeed;

        if (Mathf.Abs(forwardInput) < 0.1f)
            speedFactor *= turnDampeningOnIdle;

        float rotationAmount = turnInput * turnSpeed * speedFactor * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);
    }

   
}

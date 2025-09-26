using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 720f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpUpDuration = 0.3f;
    [SerializeField] private float fallDuration = 0.2f;
    private Vector3 _input;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private float currentJumpOffset = 0f; 
    private float baseY; 
    public Health health;
    public TextMeshProUGUI healthBar;

    public bool IsInvulnerable { get; private set; }

    private void Start()
    {
        baseY = transform.position.y;
    }

    private void Update()
    {
        GatherInput();
        LookAtMouse();

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        if (isJumping)
        {
            UpdateJump();
        }

        healthBar.text = health.currentHealth.ToString() + " :HEALTH";

    }

    private void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _input = rawInput.ToIso().normalized;
    }

    void LookAtMouse()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, baseY, 0));

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = (hitPoint - transform.position).normalized;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
            }
        }
    }

    void Move()
    {
        Vector3 targetPos = transform.position;

        if (_input != Vector3.zero)
        {
            Vector3 moveDelta = _input * _speed * Time.fixedDeltaTime;
            targetPos += moveDelta;
        }

        targetPos.y = baseY + currentJumpOffset;

        _rb.MovePosition(targetPos);
    }

    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        IsInvulnerable = true;
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float totalDuration = jumpUpDuration + fallDuration;

        if (jumpTimer < jumpUpDuration)
        {
            float t = jumpTimer / jumpUpDuration;
            currentJumpOffset = Mathf.Sin(t * Mathf.PI * 0.5f) * jumpHeight;
        }
        else if (jumpTimer < totalDuration)
        {
            float t = (jumpTimer - jumpUpDuration) / fallDuration;
            currentJumpOffset = Mathf.Cos(t * Mathf.PI * 0.5f) * jumpHeight;
        }
        else
        {
            // Land
            currentJumpOffset = 0f;
            isJumping = false;
            IsInvulnerable = false;
        }
    }
}

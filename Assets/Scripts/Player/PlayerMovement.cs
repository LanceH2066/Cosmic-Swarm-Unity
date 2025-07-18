using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 5f; // Maximum speed of the player
    public float accelerationTime = 0.3f; // Time to reach max speed
    public float drag = 1.0f; // Higher drag = faster deceleration

    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Camera mainCam;

    void Awake()
    {
        controls = new PlayerControls();
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += _ => moveInput = Vector2.zero;
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput.normalized * maxSpeed;
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, accelerationTime);
        if (moveInput == Vector2.zero)
        {
            rb.linearVelocity *= 1f - (drag * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}

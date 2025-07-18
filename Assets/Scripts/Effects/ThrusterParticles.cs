using UnityEngine;

public class PlayerThrusterFX : MonoBehaviour
{
    public ParticleSystem leftThruster;
    public ParticleSystem rightThruster;
    public float moveSpeedThreshold = 0.1f;
    public float rotationThreshold = 0.5f; // Degrees per frame to consider spinning

    private Rigidbody2D rb;
    private float lastRotationZ;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastRotationZ = transform.eulerAngles.z;
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        float currentRotationZ = transform.eulerAngles.z;
        float deltaRotation = Mathf.DeltaAngle(lastRotationZ, currentRotationZ);
        lastRotationZ = currentRotationZ;

        bool isMoving = speed > moveSpeedThreshold;
        bool isRotatingCW = deltaRotation < -rotationThreshold;
        bool isRotatingCCW = deltaRotation > rotationThreshold;

        // Case 1: Moving → fire both
        if (isMoving)
        {
            SetThruster(leftThruster, true);
            SetThruster(rightThruster, true);
        }
        // Case 2: Not moving but rotating
        else if (!isMoving)
        {
            SetThruster(leftThruster, isRotatingCW);   // Rotate CW → fire left
            SetThruster(rightThruster, isRotatingCCW); // Rotate CCW → fire right
        }
        else
        {
            SetThruster(leftThruster, false);
            SetThruster(rightThruster, false);
        }
    }

    void SetThruster(ParticleSystem ps, bool state)
    {
        var emission = ps.emission;
        emission.enabled = state;
    }
}

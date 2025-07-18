using UnityEngine;

[RequireComponent(typeof(EnemyStats), typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{
    private EnemyStats stats;
    private Rigidbody2D rb;
    private GameObject player;
    private Transform playerTransform;
    public float damageCooldown = 0.5f;
    private float lastDamageTime = -999f;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player) playerTransform = player.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = playerTransform.position - transform.position;
        float distance = direction.magnitude;

        // Normalize direction only if there's movement
        direction.Normalize();

        // Smooth slowdown when approaching stoppingDistance
        float slowdownFactor = Mathf.Clamp01((distance - stats.stoppingDistance) / stats.stoppingDistance);
        float adjustedSpeed = stats.moveSpeed * slowdownFactor;

        Vector2 move = direction * adjustedSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Face the player at all times
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                collision.GetComponent<PlayerStats>().TakeDamage(stats.damage);
            }
        }
    }
}

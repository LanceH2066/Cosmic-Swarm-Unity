using UnityEngine;

public class XpOrb : MonoBehaviour
{
    public float attractRange = 6f;
    public float moveSpeed = 7f;
    public float slowDownRate = 2f;
    private Transform player;

    private Vector2 velocity = Vector2.zero;
    private bool isAttracting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attractRange)
        {
            // Move toward player
            Vector2 direction = (player.position - transform.position).normalized;
            velocity = Vector2.Lerp(velocity, direction * moveSpeed, Time.deltaTime * 5f);
            isAttracting = true;
        }
        else
        {
            // Gradually slow down
            velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * slowDownRate);
            isAttracting = false;
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerStats>().GainXP(1);
            Destroy(gameObject);
        }
    }
}

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
        StartCoroutine(SpawnPopEffect());
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

    private System.Collections.IEnumerator SpawnPopEffect()
    {
        float duration = 0.25f;
        float elapsed = 0f;

        Vector3 overshootScale = new Vector3(0.5f, 0.5f, 1f);
        Vector3 finalScale = new Vector3(0.25f, 0.25f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (t < 0.5f)
                transform.localScale = Vector3.Lerp(Vector3.zero, overshootScale, t * 2f);
            else
                transform.localScale = Vector3.Lerp(overshootScale, finalScale, (t - 0.5f) * 2f);

            yield return null;
        }

        transform.localScale = finalScale;
    }
    
}

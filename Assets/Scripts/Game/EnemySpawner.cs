using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 15f;
    public float runDuration = 1800f; // 30 minutes in seconds

    private float elapsedTime = 0f;
    private float lastSpawnTime = 0f;

    void Update()
    {
        if (player == null) return;

        elapsedTime += Time.deltaTime;

        float timeFactor = Mathf.Clamp01(elapsedTime / runDuration);
        float spawnInterval = Mathf.Max(0.05f, 2f * Mathf.Exp(-3f * timeFactor));
        int maxEnemies = Mathf.Min(50 + Mathf.FloorToInt(450 * timeFactor), 500);
        int batchSize = Mathf.Min(5 + Mathf.FloorToInt(25 * timeFactor), 30);

        if (Time.time - lastSpawnTime >= spawnInterval && CountEnemies() < maxEnemies)
        {
            for (int i = 0; i < batchSize; i++)
            {
                Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
                Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }

            lastSpawnTime = Time.time;
        }
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}

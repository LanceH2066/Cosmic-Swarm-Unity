using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float speed;
    protected float damage;
    protected float aoeSize;
    protected WeaponData data;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(WeaponData weaponData, Vector3 direction, float dmg, float aoe)
    {
        data = weaponData;
        damage = dmg;
        aoeSize = aoe;
        speed = weaponData.projectileSpeed;
        transform.localScale *= aoeSize;

        if (rb != null)
            rb.linearVelocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, 5f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        var enemy = col.GetComponent<EnemyStats>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        OnHitEnemy(col);
    }

    /// <summary>
    /// Allows child bullet types (rockets, explosions, etc.) to override behavior.
    /// </summary>
    protected virtual void OnHitEnemy(Collider2D col)
    {
        Destroy(gameObject);
    }
}


// ========================= ROCKET BULLET =========================

public class RocketBullet : Bullet
{
    [Header("Explosion")]
    public GameObject explosionPrefab;
    public float explosionRadius = 2f;

    protected override void OnHitEnemy(Collider2D col)
    {
        Explode();
    }

    void Explode()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            var enemy = hit.GetComponent<EnemyStats>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

}
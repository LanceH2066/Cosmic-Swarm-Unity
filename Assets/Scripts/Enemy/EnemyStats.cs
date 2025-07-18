using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHP = 20f;
    public float currentHP;
    public float moveSpeed = 2f;
    public float damage = 10f;
    public float stoppingDistance = 1f;
    private DamageFlash damageFlash;
    public GameObject XpOrbPrefab;
    public GameObject explosionEffectPrefab;

    void Awake()
    {
        currentHP = maxHP;
        damageFlash = GetComponent<DamageFlash>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0f)
        {
            Die();
        }
        else
        {
            if (damageFlash != null)
            {
                damageFlash.CallDamageFlash();
            }
        }
    }

    void Die()
    {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Instantiate(XpOrbPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

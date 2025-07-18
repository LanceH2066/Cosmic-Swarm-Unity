using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private float aoeSize;
    private WeaponType type;

    public void Init(WeaponData data, Vector3 direction, float dmgMult, float aoeMult)
    {
        damage = data.Damage(dmgMult);
        speed = data.ProjSpeed(1);
        aoeSize = data.AoeSize(aoeMult);
        type = data.type;


        GetComponent<SpriteRenderer>().sprite = data.projectileSprite;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);


        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

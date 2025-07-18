using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public List<WeaponData> weaponLoadout;
    public GameObject greenLaserPrefab;
    public Transform leftGun;
    public Transform rightGun;
    private PlayerStats playerStats;
    private float[] fireCooldowns;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        fireCooldowns = new float[weaponLoadout.Count];
    }

    void Update()
    {
        for (int i = 0; i < weaponLoadout.Count; i++)
        {
            fireCooldowns[i] -= Time.deltaTime;

            if (CanFire(weaponLoadout[i], fireCooldowns[i]))
            {
                FireWeapon(i);
                fireCooldowns[i] = weaponLoadout[i].FireRate(playerStats.fireRateMultiplier);
            }
        }
    }

    void FireWeapon(int index)
    {
        var weapon = weaponLoadout[index];
        Vector3 dir = transform.up;

        if (weapon.type == WeaponType.Default)
        {
            SpawnBullet(leftGun.position, dir, weapon);
            SpawnBullet(rightGun.position, dir, weapon);
        }
        else
        {
            SpawnBullet(transform.position, dir, weapon);
        }
    }

    void SpawnBullet(Vector3 position, Vector3 direction, WeaponData weapon)
    {
        GameObject bullet = Instantiate(greenLaserPrefab, position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(weapon, direction, playerStats.damageMultiplier, playerStats.aoeSizeMultiplier);
    }

    bool CanFire(WeaponData weapon, float cooldown) => weapon != null && cooldown <= 0f;
}

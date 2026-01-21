using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct WeaponLoadoutItem
{
    public WeaponData data;
    public int level;
}

public class WeaponController : MonoBehaviour
{
    public List<WeaponLoadoutItem> weaponLoadout;
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

            if (CanFire(weaponLoadout[i].data, fireCooldowns[i]))
            {
                FireWeapon(i);
                fireCooldowns[i] = weaponLoadout[i].data.FireRate(playerStats.fireRateMultiplier * GetWeaponLevelMultiplier(weaponLoadout[i].level));
            }
        }
    }

    void FireWeapon(int index)
    {
        var weapon = weaponLoadout[index].data;
        int level = weaponLoadout[index].level;
        Vector3 dir = transform.up;

        if (weapon.type == WeaponType.Default)
        {
            SpawnBullet(leftGun.position, dir, weapon, level);
            SpawnBullet(rightGun.position, dir, weapon, level);
        }
        else
        {
            SpawnBullet(transform.position, dir, weapon, level);
        }
    }

    void SpawnBullet(Vector3 position, Vector3 direction, WeaponData weapon, int level)
    {
        GameObject bullet = Instantiate(greenLaserPrefab, position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(weapon, direction, playerStats.damageMultiplier * GetWeaponLevelMultiplier(level), playerStats.aoeSizeMultiplier * GetWeaponLevelMultiplier(level));
    }

    bool CanFire(WeaponData weapon, float cooldown) => weapon != null && cooldown <= 0f;

    public void AddWeapon(WeaponData weapon)
    {
        WeaponLoadoutItem item = new WeaponLoadoutItem { data = weapon, level = 1 };
        weaponLoadout.Add(item);
        // Resize fireCooldowns
        float[] newCooldowns = new float[weaponLoadout.Count];
        fireCooldowns.CopyTo(newCooldowns, 0);
        fireCooldowns = newCooldowns;
    }

    private float GetWeaponLevelMultiplier(int level)
    {
        return 1f + 0.2f * (level - 1); // 20% increase per level
    }
}

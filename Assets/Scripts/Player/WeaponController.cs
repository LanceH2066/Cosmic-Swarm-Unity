using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponInstance
{
    public WeaponData data;
    public int level = 1;

    [HideInInspector] public float cooldown;
}

public class WeaponController : MonoBehaviour
{
    [Header("Loadout")]
    [Tooltip("All weapons the player currently has. First slot is always the default green laser.")]
    public List<WeaponInstance> weapons = new();

    [Header("References")]
    public Transform leftGun;
    public Transform rightGun;
    private PlayerStats playerStats;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        EnsureDefaultWeapon();
    }

    void Update()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            TickWeapon(weapons[i]);
        }
    }

    // ========================= CORE LOOP =========================

    void TickWeapon(WeaponInstance weapon)
    {
        if (weapon.data == null) return;

        weapon.cooldown -= Time.deltaTime;

        if (weapon.cooldown > 0f) return;

        Fire(weapon);

        weapon.cooldown = weapon.data.FireRate(playerStats.fireRateMultiplier, weapon.level);
    }

    void Fire(WeaponInstance weapon)
    {
        Vector3 dir = transform.up;

        switch (weapon.data.type)
        {
            case WeaponType.Default:
                Spawn(leftGun.position, dir, weapon);
                Spawn(rightGun.position, dir, weapon);
                break;

            case WeaponType.Rocket:
            default:
                Spawn(transform.position, dir, weapon);
                break;
        }
    }

    void Spawn(Vector3 pos, Vector3 dir, WeaponInstance weapon)
    {
        GameObject proj = Instantiate(weapon.data.projectilePrefab, pos, Quaternion.identity);

        float dmg = weapon.data.GetDamage(weapon.level, playerStats.damageMultiplier);
        float aoe = weapon.data.GetAOE(weapon.level, playerStats.aoeSizeMultiplier);

        proj.GetComponent<Bullet>().Init(weapon.data, dir, dmg, aoe);
    }

    // ========================= PUBLIC API =========================

    public void AddWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null) return;

        // Prevent duplicates → upgrade level instead
        foreach (var w in weapons)
        {
            if (w.data == newWeapon)
            {
                LevelUp(w);
                return;
            }
        }

        weapons.Add(new WeaponInstance { data = newWeapon, level = 1 });
    }

    public void LevelUp(WeaponInstance weapon)
    {
        weapon.level = Mathf.Clamp(weapon.level + 1, 1, 5);
    }

    // ========================= DEFAULT WEAPON =========================

    void EnsureDefaultWeapon()
    {
        if (weapons.Count == 0 || weapons[0].data == null || weapons[0].data.type != WeaponType.Default)
        {
            Debug.LogWarning("First weapon must be the Default Green Laser. Fixing automatically.");

            WeaponData defaultWeapon = FindDefaultWeaponInProject();

            if (defaultWeapon != null)
            {
                weapons.Insert(0, new WeaponInstance { data = defaultWeapon, level = 1 });
            }
        }
    }

    WeaponData FindDefaultWeaponInProject()
    {
        var all = Resources.LoadAll<WeaponData>("");

        foreach (var w in all)
        {
            if (w.type == WeaponType.Default)
                return w;
        }

        Debug.LogError("No WeaponData with type Default found in Resources folder.");
        return null;
    }
}

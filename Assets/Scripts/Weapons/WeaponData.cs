using UnityEngine;

public enum WeaponType
{
    Default,
    Rocket,
    Spread,
    Beam,
    Orbital
}

[CreateAssetMenu(menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public string weaponName;
    public WeaponType type;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Base Stats")]
    public float baseDamage = 1f;
    public float baseFireRate = 1f; // shots per second
    public float baseAOE = 1f;
    public float projectileSpeed = 10f;

    [Header("Scaling Per Level")]
    [Tooltip("Damage multiplier added per level (0.2 = +20% per level)")]
    public float damagePerLevel = 0.2f;

    [Tooltip("Fire rate multiplier added per level")]
    public float fireRatePerLevel = 0.15f;

    [Tooltip("AOE multiplier added per level")]
    public float aoePerLevel = 0.15f;

    [Header("Limits")]
    public int maxLevel = 5;

    // ========================= RUNTIME HELPERS =========================

    public float GetDamage(int level, float playerMultiplier)
    {
        float levelMult = 1f + damagePerLevel * (level - 1);
        return baseDamage * levelMult * playerMultiplier;
    }

    public float GetAOE(int level, float playerMultiplier)
    {
        float levelMult = 1f + aoePerLevel * (level - 1);
        return baseAOE * levelMult * playerMultiplier;
    }

    /// <summary>
    /// Returns cooldown time between shots.
    /// </summary>
    public float FireRate(float playerFireRateMultiplier, int level = 1)
    {
        float levelMult = 1f + fireRatePerLevel * (level - 1);
        float shotsPerSecond = baseFireRate * levelMult * playerFireRateMultiplier;

        if (shotsPerSecond <= 0f) return 999f;

        return 1f / shotsPerSecond;
    }
}
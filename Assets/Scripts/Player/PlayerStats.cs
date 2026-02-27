using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Experience")]
    public int playerLevel = 1;
    public int experiencePoints = 0;
    public int xpThreshold = 10;

    [Header("Stat Multipliers")]
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float healthMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float aoeSizeMultiplier = 1f;


    private DamageFlash damageFlash;
    public HealthBar healthBar;
    public HealthBar xpBar;
    
    public LevelUpUI levelUpUI;
    public List<WeaponData> availableWeapons;
    public Dictionary<WeaponData, int> weaponLevels = new Dictionary<WeaponData, int>();
    public Dictionary<PassiveType, int> passiveLevels = new Dictionary<PassiveType, int>();

    private void Awake()
    {
        currentHP = maxHP;
        damageFlash = GetComponent<DamageFlash>();
        healthBar.SetMaxHealth(maxHP);
        healthBar.SetHealth(currentHP);
        xpBar.SetMaxHealth(xpThreshold);
        xpBar.SetHealth(0);

        // Initialize passive levels
        foreach (PassiveType p in System.Enum.GetValues(typeof(PassiveType)))
        {
            passiveLevels[p] = 0;
        }
    }

    public void GainXP(int amount)
    {
        experiencePoints += amount;
        xpBar.SetHealth(experiencePoints);
        if (experiencePoints >= xpThreshold)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Generate 3 random options
        List<UpgradeOption> options = GenerateUpgradeOptions();
        levelUpUI.ShowLevelUpOptions(options, OnUpgradeSelected);
    }

    private void OnUpgradeSelected(int index)
    {
        UpgradeOption selected = levelUpUI.currentOptions[index];
        ApplyUpgrade(selected);

        // Now subtract XP and increase level
        experiencePoints -= xpThreshold;
        playerLevel++;
        xpThreshold = CalculateXPThreshold(playerLevel);
        xpBar.SetMaxHealth(xpThreshold);
        xpBar.SetHealth(experiencePoints);

        // Check if can level up again
        if (experiencePoints >= xpThreshold)
        {
            LevelUp();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0f);
        healthBar.SetHealth(currentHP);
        if (currentHP <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            if (damageFlash != null)
            {
                damageFlash.CallDamageFlash();
            }
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public int CalculateXPThreshold(int level)
    {
        return 10 + (level - 1) * 10; // You can tweak this formula
    }

    private List<UpgradeOption> GenerateUpgradeOptions()
    {
        List<UpgradeOption> options = new List<UpgradeOption>();

        // For simplicity, always offer 2 weapons and 1 passive, or randomize
        // But to make it balanced, perhaps 50/50 chance

        for (int i = 0; i < 3; i++)
        {
            UpgradeOption option = new UpgradeOption();
            if (Random.value < 0.5f)
            {
                // Weapon
                option.type = UpgradeType.Weapon;
                option.weapon = availableWeapons[Random.Range(0, availableWeapons.Count)];
            }
            else
            {
                // Passive
                option.type = UpgradeType.Passive;
                option.passive = (PassiveType)Random.Range(0, System.Enum.GetValues(typeof(PassiveType)).Length);
            }
            options.Add(option);
        }

        return options;
    }

    private void ApplyUpgrade(UpgradeOption option)
    {
        if (option.type == UpgradeType.Weapon)
        {
            if (!weaponLevels.ContainsKey(option.weapon))
            {
                weaponLevels[option.weapon] = 0;
                GetComponent<WeaponController>().AddWeapon(option.weapon);
            }

            weaponLevels[option.weapon]++;
            if (weaponLevels[option.weapon] > 5) weaponLevels[option.weapon] = 5;
        }
        else
        {
            passiveLevels[option.passive]++;
            if (passiveLevels[option.passive] > 3) passiveLevels[option.passive] = 3;

            RecalculatePassives();
        }
    }

    private void RecalculatePassives()
    {
        // Reset to base values
        damageMultiplier = 1f;
        fireRateMultiplier = 1f;
        speedMultiplier = 1f;
        healthMultiplier = 1f;
        aoeSizeMultiplier = 1f;

        // Damage
        damageMultiplier = 1f + 0.1f * passiveLevels[PassiveType.Damage];

        // Fire rate
        fireRateMultiplier = 1f + 0.1f * passiveLevels[PassiveType.FireRate];

        // Speed
        speedMultiplier = 1f + 0.1f * passiveLevels[PassiveType.Speed];

        // Health
        healthMultiplier = 1f + 0.1f * passiveLevels[PassiveType.Health];

        float newMaxHP = 100f * healthMultiplier;
        maxHP = newMaxHP;
        currentHP = Mathf.Min(currentHP, maxHP);
        healthBar.SetMaxHealth(maxHP);
        healthBar.SetHealth(currentHP);
    }


}

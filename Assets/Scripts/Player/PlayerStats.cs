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
                // Add to loadout if not already
                GetComponent<WeaponController>().AddWeapon(option.weapon);
            }
            weaponLevels[option.weapon]++;
            // Cap at 5 levels
            if (weaponLevels[option.weapon] > 5) weaponLevels[option.weapon] = 5;
        }
        else
        {
            passiveLevels[option.passive]++;
            // Cap at 3 levels
            if (passiveLevels[option.passive] > 3) passiveLevels[option.passive] = 3;

            // Apply passive effect
            float bonus = 0.1f * passiveLevels[option.passive]; // 10% per level
            switch (option.passive)
            {
                case PassiveType.Damage:
                    damageMultiplier += bonus;
                    break;
                case PassiveType.FireRate:
                    fireRateMultiplier += bonus;
                    break;
                case PassiveType.Health:
                    healthMultiplier += bonus;
                    maxHP *= (1 + bonus);
                    healthBar.SetMaxHealth(maxHP);
                    break;
                case PassiveType.Speed:
                    speedMultiplier += bonus;
                    break;
            }
        }
    }
}

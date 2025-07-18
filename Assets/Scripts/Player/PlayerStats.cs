using UnityEngine;

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

    private void Awake()
    {
        currentHP = maxHP;
        damageFlash = GetComponent<DamageFlash>();
        healthBar.SetMaxHealth(maxHP);
        healthBar.SetHealth(currentHP);
        xpBar.SetMaxHealth(xpThreshold);
        xpBar.SetHealth(0);
    }

    public void GainXP(int amount)
    {
        experiencePoints += amount;
        xpBar.SetHealth(experiencePoints);
        while (experiencePoints >= xpThreshold)
        {
            experiencePoints -= xpThreshold;
            playerLevel++;
            xpThreshold = CalculateXPThreshold(playerLevel);
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
}

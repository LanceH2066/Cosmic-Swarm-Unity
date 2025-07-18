using UnityEngine;

public enum WeaponType { Default, Rocket, Laser, Flak }

[CreateAssetMenu(fileName = "WeaponData", menuName = "CosmicSwarm/Weapon")]
public class WeaponData : ScriptableObject
{
    public WeaponType type;
    public int level = 1;
    public float baseDamage = 5f;
    public float baseFireRate = 1f;
    public float baseAoeSize = 1f;
    public float baseProjSpeed = 30f;
    public Sprite projectileSprite;

    public float Damage(float multiplier) => baseDamage * multiplier;
    public float FireRate(float multiplier) => baseFireRate * multiplier;
    public float AoeSize(float multiplier) => baseAoeSize * multiplier;
    public float ProjSpeed(float multiplier) => baseProjSpeed * multiplier;
}

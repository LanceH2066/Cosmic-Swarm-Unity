# Cosmic Swarm AI Coding Guidelines

## Project Overview
Cosmic Swarm is a 2D top-down shooter where the player survives increasingly difficult waves of enemies for 30 minutes. Core gameplay involves player movement, automatic weapon firing, enemy AI, XP collection, and stat progression.

## Architecture Patterns
- **Component-Based Design**: Use Unity's component system with `GetComponent<>()` for dependencies. Avoid singletons; prefer direct references or tag-based lookups.
- **Tag-Based Object Finding**: Use `GameObject.FindGameObjectWithTag()` for runtime object discovery (e.g., "Player", "Enemy").
- **ScriptableObject Configuration**: Weapon stats and data use `ScriptableObject` assets created via `CreateAssetMenu`.
- **Stat Multipliers**: Player stats (damage, speed, health) use multiplier floats applied to base values.
- **Coroutine Effects**: Visual effects like damage flashes and spawn animations use `StartCoroutine()`.

## Key Components
- **EnemySpawner** (`Assets/Scripts/Game/EnemySpawner.cs`): Manages enemy waves with exponential difficulty scaling.
- **PlayerStats** (`Assets/Scripts/Player/PlayerStats.cs`): Handles health, XP, leveling, and stat multipliers.
- **WeaponController** (`Assets/Scripts/Player/WeaponController.cs`): Fires weapons automatically based on fire rate and loadout.
- **EnemyBehavior** (`Assets/Scripts/Enemy/EnemyBehavior.cs`): Simple AI that moves toward player and deals contact damage.

## Coding Conventions
- **Math Operations**: Use `Mathf` for all math (e.g., `Mathf.Clamp01()`, `Mathf.Lerp()`).
- **Instantiation**: Use `Instantiate(prefab, position, Quaternion.identity)` for spawning; set up via `Init()` methods.
- **Damage System**: Call `TakeDamage(float)` on stats components; handle death in `Die()` method.
- **UI Updates**: Health/XP bars update via `SetHealth()` and `SetMaxHealth()` on `HealthBar` components.
- **Time Management**: Use `Time.deltaTime` in `Update()`, `Time.fixedDeltaTime` in `FixedUpdate()`.

## Examples
- **Enemy Death**: `Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity); Instantiate(XpOrbPrefab, transform.position, Quaternion.identity); Destroy(gameObject);`
- **Player Damage**: `currentHP -= amount; if (currentHP <= 0f) Destroy(gameObject);`
- **Weapon Firing**: `GameObject bullet = Instantiate(prefab, position, Quaternion.identity); bullet.GetComponent<Bullet>().Init(data, direction, dmgMult, aoeMult);`

## Build & Run
- Standard Unity build process: Open in Unity Editor, go to File > Build Settings, select platform, Build.
- Run in Editor via Play button; timer starts at scene load.

## Dependencies
- Unity 6000.2.12f1 with URP, 2D features, Cinemachine, Input System.
- No external APIs or services; self-contained game logic.
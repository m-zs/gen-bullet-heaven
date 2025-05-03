using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] public Ability[] startingAbilities;
    public Dictionary<string, Ability> abilities = new();
    private Dictionary<string, float> timeLastUsed = new();

    [SerializeField] Dictionary<AbilityAssignment, AbilityTargetingType[]> abilityTargetingTypeMappings;
    private CharacterManager characterManager;
    [SerializeField] private bool addRandomAbility = false;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();

        abilityTargetingTypeMappings = new()
        {
            { AbilityAssignment.Melee, new[] {
                // AbilityTargetingType.Projectile,
             AbilityTargetingType.Targeted,
            //  AbilityTargetingType.AreaOfEffect
             } },
            // { AbilityAssignment.Spell, new[] { AbilityTargetingType.Projectile, AbilityTargetingType.Targeted, AbilityTargetingType.AreaOfEffect } },
            // { AbilityAssignment.Summon, new[] { AbilityTargetingType.AreaOfEffect, AbilityTargetingType.Projectile, AbilityTargetingType.Targeted } }
        };

        foreach (var ability in startingAbilities)
        {
            AddAbility(ability);
        }

        if (addRandomAbility)
        {
            GenerateAbility();
        }
    }

    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (ability.Value is ProjectileAbility projectileAbility)
            {
                UseProjectileAbility(projectileAbility);
            }

            if (ability.Value is TargetedAbility targetedAbility)
            {
                UseTargetedAbility(targetedAbility);
            }

            if (ability.Value is AreaOfEffectAbility areaOfEffectAbility)
            {
                UseAreaOfEffectAbility(areaOfEffectAbility);
            }
        }
    }

    private void UseProjectileAbility(ProjectileAbility ability)
    {
        if (ability.aimType == AbilityAimType.Closest)
        {
            CharacterManager currentTarget = EnemyScanner.GetNearestEnemy(transform.position, 100f, characterManager.teamId);
            InitializeProjectileAbility(ability, characterManager.transform.position, currentTarget.transform.position);
        }

        if (ability.aimType == AbilityAimType.Random)
        {
            CharacterManager currentTarget = EnemyScanner.GetRandomEnemyOnScreen(mainCamera, characterManager.teamId);
            if (currentTarget != null)
            {
                InitializeProjectileAbility(ability, characterManager.transform.position, currentTarget.transform.position);
            }
        }

        if (ability.aimType == AbilityAimType.Aim)
        {
            Vector2 cursorPosition = GetCursorWorldPosition();
            InitializeProjectileAbility(ability, characterManager.transform.position, cursorPosition);
        }
    }

    private void UseTargetedAbility(TargetedAbility ability)
    {
        if (ability.aimType == AbilityAimType.Closest)
        {
            CharacterManager currentTarget = EnemyScanner.GetNearestEnemy(transform.position, 100f, characterManager.teamId);
            InitializeTargetedAbility(ability, currentTarget);
        }

        if (ability.aimType == AbilityAimType.Random)
        {
            CharacterManager currentTarget = EnemyScanner.GetRandomEnemyOnScreen(mainCamera, characterManager.teamId);
            if (currentTarget != null)
            {
                InitializeTargetedAbility(ability, currentTarget);
            }
        }

        if (ability.aimType == AbilityAimType.Aim)
        {
            CharacterManager currentTarget = EnemyScanner.GetRandomEnemyOnScreen(mainCamera, characterManager.teamId); // todo: add a way to get hover target
            if (currentTarget != null)
            {
                InitializeTargetedAbility(ability, currentTarget);
            }
        }
    }

    private void UseAreaOfEffectAbility(AreaOfEffectAbility ability)
    {
        if (ability.aimType == AbilityAimType.Closest)
        {
            CharacterManager currentTarget = EnemyScanner.GetNearestEnemy(transform.position, 100f, characterManager.teamId);
            InitializeAreaOfEffectAbility(ability, currentTarget.transform.position);
        }

        if (ability.aimType == AbilityAimType.Random)
        {
            CharacterManager currentTarget = EnemyScanner.GetRandomEnemyOnScreen(mainCamera, characterManager.teamId);
            InitializeAreaOfEffectAbility(ability, currentTarget.transform.position);
        }

        if (ability.aimType == AbilityAimType.Aim)
        {
            Vector2 cursorPosition = GetCursorWorldPosition();
            InitializeAreaOfEffectAbility(ability, cursorPosition);
        }
    }

    private bool InitializeProjectileAbility(ProjectileAbility ability, Vector3 from, Vector3 to)
    {
        if (ability == null || IsOnCooldown(ability.abilityName))
        {
            return false;
        }

        timeLastUsed[ability.abilityName] = Time.time;
        GameObject projectileObj = new("PlayerProjectile");
        Projectile projectile = projectileObj.AddComponent<Projectile>();

        if (to != null)
        {
            projectile.Use(from, to);
        }
        else
        {
            projectile.Use(from);
        }
        return true;
    }

    private bool InitializeTargetedAbility(TargetedAbility ability, CharacterManager target)
    {
        if (ability == null || IsOnCooldown(ability.abilityName) || target == null)
        {
            return false;
        }

        timeLastUsed[ability.abilityName] = Time.time;
        GameObject targetedAbilityObj = new("TargetedAbility");
        Targeted targetedAbility = targetedAbilityObj.AddComponent<Targeted>();
        targetedAbility.Use(target);

        return true;
    }

    private bool InitializeAreaOfEffectAbility(AreaOfEffectAbility ability, Vector3 center)
    {
        if (ability == null || IsOnCooldown(ability.abilityName) || center == null)
        {
            return false;
        }

        timeLastUsed[ability.abilityName] = Time.time;
        GameObject aoe = new("Aoe");
        AreaOfEffect aoeComponent = aoe.AddComponent<AreaOfEffect>();
        aoeComponent.radius = ability.radius;
        aoeComponent.Use(center);

        return true;
    }

    private Vector2 GetCursorWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public bool AddAbility(Ability ability)
    {
        if (ability == null)
        {
            return false;
        }

        abilities.Add(ability.abilityName, ability);
        return true;
    }


    private bool IsOnCooldown(string abilityName)
    {
        return timeLastUsed.ContainsKey(abilityName) && Time.time - timeLastUsed[abilityName] < abilities[abilityName].cooldown;
    }

    private void GenerateAbility()
    {
        AbilityAssignment abilityAssignment = GetRandomEnumValue<AbilityAssignment>();
        AbilityTargetingType abilityTargetingType = abilityTargetingTypeMappings[abilityAssignment][Random.Range(0, abilityTargetingTypeMappings[abilityAssignment].Length)];
        DamageType damageType = GetRandomEnumValue<DamageType>();
        AbilityActivationType abilityActivationType = GetRandomEnumValue<AbilityActivationType>();
        AbilityAimType abilityAimType = GetRandomEnumValue<AbilityAimType>();

        Debug.Log($"Ability Assignment: {abilityAssignment}");
        Debug.Log($"Ability Targeting Type: {abilityTargetingType}");
        Debug.Log($"Damage Type: {damageType}");
        Debug.Log($"Ability Activation Type: {abilityActivationType}");
        Debug.Log($"Ability Aim Type: {abilityAimType}");

        Ability ability = null;

        if (abilityTargetingType == AbilityTargetingType.Projectile)
        {
            ability = ScriptableObject.CreateInstance<ProjectileAbility>();
        }
        else if (abilityTargetingType == AbilityTargetingType.Targeted)
        {
            ability = ScriptableObject.CreateInstance<TargetedAbility>();
        }
        else if (abilityTargetingType == AbilityTargetingType.AreaOfEffect)
        {
            ability = ScriptableObject.CreateInstance<AreaOfEffectAbility>();
            (ability as AreaOfEffectAbility).radius = 100f;
        }

        if (ability)
        {
            ability.abilityName = System.Guid.NewGuid().ToString();
            ability.abilityAssignment = abilityAssignment;
            ability.abilityTargetingType = abilityTargetingType;
            ability.damageType = damageType;
            ability.activationType = abilityActivationType;
            ability.cooldown = 1f;
            ability.damage = 10f;
            ability.aimType = abilityAimType;
            AddAbility(ability);
        }
    }

    private T GetRandomEnumValue<T>()
    {
        return (T)System.Enum.GetValues(typeof(T)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(T)).Length));
    }
}

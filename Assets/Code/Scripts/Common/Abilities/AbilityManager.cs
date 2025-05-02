using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] public Ability[] startingAbilities;
    private Dictionary<string, Ability> abilities = new();
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

        if (!addRandomAbility) return;

        abilityTargetingTypeMappings = new()
        {
            { AbilityAssignment.Melee, new[] {
                AbilityTargetingType.Projectile,
            //  AbilityTargetingType.Targeted,
            //  AbilityTargetingType.AreaOfEffect
             } },
            // { AbilityAssignment.Spell, new[] { AbilityTargetingType.Projectile, AbilityTargetingType.Targeted, AbilityTargetingType.AreaOfEffect } },
            // { AbilityAssignment.Summon, new[] { AbilityTargetingType.AreaOfEffect, AbilityTargetingType.Projectile, AbilityTargetingType.Targeted } }
        };

        foreach (var ability in startingAbilities)
        {
            AddAbility(ability);
        }
        GenerateAbility();
    }

    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (ability.Value.aimType == AbilityAimType.Closest)
            {
                CharacterManager currentTarget = EnemyScanner.GetNearestEnemy(transform.position, 100f, characterManager.teamId);
                UseAbility(ability.Key, characterManager.transform.position, currentTarget.transform.position);
            }

            if (ability.Value.aimType == AbilityAimType.Random)
            {
                CharacterManager currentTarget = EnemyScanner.GetRandomEnemyOnScreen(mainCamera, characterManager.teamId);
                if (currentTarget != null)
                {
                    UseAbility(ability.Key, characterManager.transform.position, currentTarget.transform.position);
                }
            }

            if (ability.Value.aimType == AbilityAimType.Aim)
            {
                Vector2 cursorPosition = GetCursorWorldPosition();
                UseAbility(ability.Key, characterManager.transform.position, cursorPosition);
            }
        }
    }

    private Vector2 GetCursorWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public List<CharacterManager> GetEnemiesInRange(float range)
    {
        return EnemyScanner.GetEnemiesInRadius(transform.position, range, characterManager.teamId);
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

    public bool UseAbility(string name, Vector3 initiator, Vector3 target)
    {
        if (name == null)
        {
            return false;
        }

        if (IsOnCooldown(name))
        {
            return false;
        }

        if (abilities.TryGetValue(name, out Ability ability))
        {
            timeLastUsed[ability.abilityName] = Time.time;

            GameObject projectileObj = new("PlayerProjectile");
            Projectile projectile = projectileObj.AddComponent<Projectile>();

            if (target != null)
            {
                projectile.Use(initiator, target);
            }
            else
            {
                projectile.Use(initiator);
            }
            return true;
        }

        return false;
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

        Ability ability = ScriptableObject.CreateInstance<Ability>();
        ability.abilityName = System.Guid.NewGuid().ToString();
        ability.abilityClass = gameObject.AddComponent<Projectile>();
        ability.abilityAssignment = abilityAssignment;
        ability.abilityTargetingType = abilityTargetingType;
        ability.damageType = damageType;
        ability.activationType = abilityActivationType;
        ability.cooldown = 1f;
        ability.damage = 10f;
        ability.aimType = abilityAimType;

        AddAbility(ability);
    }

    private T GetRandomEnumValue<T>()
    {
        return (T)System.Enum.GetValues(typeof(T)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(T)).Length));
    }
}

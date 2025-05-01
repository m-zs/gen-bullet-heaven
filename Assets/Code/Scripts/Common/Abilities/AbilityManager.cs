using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] public Ability[] startingAbilities;
    private Dictionary<string, Ability> abilities = new();
    private Dictionary<string, float> timeLastUsed = new();

    [SerializeField] Dictionary<AbilityAssignment, AbilityTargetingType[]> abilityTargetingTypeMappings;
    private CharacterManager characterManager;

    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
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
            UseAbility(ability.Key, characterManager, null);
        }
    }

    public bool AddAbility(Ability ability)
    {
        if (ability == null)
        {
            Debug.LogError("Ability is null");
            return false;
        }

        abilities.Add(ability.abilityName, ability);
        return true;
    }

    public bool UseAbility(string name, CharacterManager initiator, CharacterManager target)
    {
        if (name == null)
        {
            Debug.LogError("Ability name is null");
            return false;
        }

        if (IsOnCooldown(name))
        {
            return false;
        }

        if (abilities.TryGetValue(name, out Ability ability))
        {
            Debug.Log($"Using ability: {name}");
            timeLastUsed[ability.abilityName] = Time.time;

            GameObject projectileObj = new("PlayerProjectile");
            Projectile projectile = projectileObj.AddComponent<Projectile>();
            projectile.Use(initiator.transform.position);
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

        Debug.Log($"Ability Assignment: {abilityAssignment}");
        Debug.Log($"Ability Targeting Type: {abilityTargetingType}");
        Debug.Log($"Damage Type: {damageType}");
        Debug.Log($"Ability Activation Type: {abilityActivationType}");

        Ability ability = ScriptableObject.CreateInstance<Ability>();
        ability.abilityName = System.Guid.NewGuid().ToString();
        ability.abilityClass = gameObject.AddComponent<Projectile>();
        ability.abilityAssignment = abilityAssignment;
        ability.abilityTargetingType = abilityTargetingType;
        ability.damageType = damageType;
        ability.activationType = abilityActivationType;
        ability.cooldown = 1f;
        ability.damage = 10f;

        AddAbility(ability);
    }

    private T GetRandomEnumValue<T>()
    {
        return (T)System.Enum.GetValues(typeof(T)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(T)).Length));
    }
}

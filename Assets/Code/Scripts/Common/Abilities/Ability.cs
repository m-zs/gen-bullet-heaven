using UnityEngine;

public enum AbilityAssignment
{
    Melee,
    // Spell,
    // Summon
}

public enum AbilityAimType
{
    Closest,
    Random,
    Aim
}

public enum AbilityTargetingType
{
    Projectile,
    // AreaOfEffect,
    // Targeted,
}

public enum MinionAbilityTargetingType
{
    Projectile,
    Area,
}


public enum CharacterResource
{
    Mana,
    Energy,
    None
}

public enum DamageType
{
    Physical,
    Magic
}

public enum AbilityActivationType
{
    Instant,
    // Toggle,
    // Duration,
    // Passive
}

public enum TriggerEventInitiator
{
    OnHit,
    OnDeath,
    OnKill,
    OnDamageTaken,
    OnHeal,
    OnCriticalHit,
    OnBlock,
    OnEvade,
    OnAttack,
    OnStep,
    OnMinionDeath,
    OnMinionSpawn
}

public enum CommonStatModifier
{
    AttackDamage,
    SpellDamage,
    MovementSpeed,
    CooldownReduction,
    DurationIncrease,
    MaxHealth,
    Health,
    AreaOfEffect
}

public enum ProjectileStatModifier
{
    Speed,
    Color,
    Trail,
    Chain,
    Pierce,
    Split
}

public enum MeleeStatModifier
{

}

public enum SpellStatModifier
{

}

public enum SummonStatModifier
{
    MaxMinions,
    MinionDamage,
    MinionSpeed,
    MinionHealth,
    MinionAreaOfEffect
}

public interface IAbility
{
    void Use(Vector3 origin);
    void Use(Vector3 origin, Vector3 direction);
}

[CreateAssetMenu(menuName = "Abilities/Create New Ability")]
public class Ability : ScriptableObject
{
    [SerializeField] public string abilityName;
    [SerializeField] public float cooldown = 0;
    [SerializeField] public float damage = 0;
    [SerializeField] public float cost = 0;
    [SerializeField] public AbilityAssignment abilityAssignment;
    [SerializeField] public AbilityTargetingType abilityTargetingType;
    [SerializeField] public DamageType damageType;
    [SerializeField] public AbilityActivationType activationType;
    [SerializeField] public CharacterResource resourceUsed = CharacterResource.None;
    [SerializeField] public IAbility abilityClass;
    [SerializeField] public AbilityAimType aimType;
}

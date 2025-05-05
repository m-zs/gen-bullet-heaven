using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(CharacterEffects))]
public class CharacterManager : MonoBehaviour, IDamageable
{
    public CharacterAttributes characterAttributes;
    public virtual AbilityManager abilityManager { get; private set; }
    public int teamId = 0;
    private CharacterEffects characterEffects;

    protected virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
        characterEffects = GetComponent<CharacterEffects>();
    }

    public void TakeDamage(float damage)
    {
        characterAttributes.currentHealth -= damage;
        characterEffects.Flash();

        if (characterAttributes.currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool IsDead => characterAttributes.currentHealth <= 0;
    public float CurrentHealth => characterAttributes.currentHealth;
    public float MaxHealth => characterAttributes.maxHealth;
}

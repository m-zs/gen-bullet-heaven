using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(AbilityManager))]
public class CharacterManager : MonoBehaviour, IDamageable
{
    public CharacterAttributes characterAttributes;
    public virtual AbilityManager abilityManager { get; private set; }
    public int teamId = 0;

    protected virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
    }

    public void TakeDamage(float damage)
    {
        characterAttributes.currentHealth -= damage;
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

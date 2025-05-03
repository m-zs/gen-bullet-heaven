using UnityEngine;

public interface IDamageable
{
  void TakeDamage(float damage);
  bool IsDead { get; }
  float CurrentHealth { get; }
  float MaxHealth { get; }
}
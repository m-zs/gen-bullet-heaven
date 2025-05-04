using UnityEngine;

public class AbilityPrefabs : MonoBehaviour
{
  public static AbilityPrefabs Instance { get; private set; }

  [Header("Projectiles Prefabs")]
  [SerializeField] public Projectile[] projectilePrefabs;

  [Header("Targeted Prefabs")]
  [SerializeField] public Targeted[] targetedPrefabs;

  [Header("Area of Effect Prefabs")]
  [SerializeField] public AreaOfEffect[] aoePrefabs;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public Projectile GetRandomProjectile()
  {
    if (projectilePrefabs == null || projectilePrefabs.Length == 0) return null;
    return projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
  }

  public Targeted GetRandomTargeted()
  {
    if (targetedPrefabs == null || targetedPrefabs.Length == 0) return null;
    return targetedPrefabs[Random.Range(0, targetedPrefabs.Length)];
  }

  public AreaOfEffect GetRandomAoe()
  {
    if (aoePrefabs == null || aoePrefabs.Length == 0) return null;
    return aoePrefabs[Random.Range(0, aoePrefabs.Length)];
  }
}
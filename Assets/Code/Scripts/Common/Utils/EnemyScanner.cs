using UnityEngine;
using System.Collections.Generic;

public static class EnemyScanner
{
  private static readonly ContactFilter2D enemyFilter;
  private static readonly List<Collider2D> results = new List<Collider2D>();
  private static readonly int enemyLayerMask;

  static EnemyScanner()
  {
    enemyLayerMask = LayerMask.GetMask("Default");

    enemyFilter = new ContactFilter2D
    {
      useLayerMask = true,
      layerMask = enemyLayerMask,
      useTriggers = true
    };
  }

  public static List<CharacterManager> GetEnemiesInRadius(Vector2 center, float radius, int teamId)
  {
    results.Clear();
    Physics2D.OverlapCircle(center, radius, enemyFilter, results);

    var enemies = new List<CharacterManager>();
    foreach (var collider in results)
    {
      if (collider.TryGetComponent<CharacterManager>(out var enemy))
      {
        if (enemy.teamId != teamId)
        {
          enemies.Add(enemy);
        }
      }
    }

    return enemies;
  }

  public static CharacterManager GetNearestEnemy(Vector2 center, float maxRadius, int teamId)
  {
    results.Clear();
    Physics2D.OverlapCircle(center, maxRadius, enemyFilter, results);

    CharacterManager nearestEnemy = null;
    float nearestDistance = float.MaxValue;

    foreach (var collider in results)
    {
      if (collider.TryGetComponent<CharacterManager>(out var enemy))
      {
        float distance = Vector2.Distance(center, collider.transform.position);
        if (distance < nearestDistance && distance <= maxRadius && enemy.teamId != teamId)
        {
          nearestDistance = distance;
          nearestEnemy = enemy;
        }
      }
    }

    return nearestEnemy;
  }

  public static List<CharacterManager> GetEnemiesInViewport(Camera camera, int teamId)
  {
    Vector2 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
    Vector2 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

    Vector2 center = (bottomLeft + topRight) * 0.5f;
    float radius = Vector2.Distance(bottomLeft, topRight) * 0.5f;

    return GetEnemiesInRadius(center, radius, teamId);
  }

  public static CharacterManager GetRandomEnemyOnScreen(Camera camera, int teamId)
  {
    var enemies = GetEnemiesInViewport(camera, teamId);
    if (enemies.Count == 0)
    {
      return null;
    }
    return enemies[Random.Range(0, enemies.Count)];
  }
}

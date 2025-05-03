using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : AbilityManager
{
  protected override Vector2 GetAimPosition()
  {
    return GetCursorWorldPosition();
  }

  protected override CharacterManager GetAimTarget()
  {
    // Find a way to get the hover target
    return EnemyScanner.GetNearestEnemy(transform.position, 100f, characterManager.teamId);
  }

  private Vector2 GetCursorWorldPosition()
  {
    Vector3 mousePos = Input.mousePosition;
    mousePos.z = -mainCamera.transform.position.z;
    return mainCamera.ScreenToWorldPoint(mousePos);
  }
}

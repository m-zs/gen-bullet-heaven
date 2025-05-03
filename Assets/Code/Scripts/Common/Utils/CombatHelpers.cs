public static class CombatHelpers
{
  public static bool IsEnemy(CharacterManager a, CharacterManager b)
  {
    return a.teamId != b.teamId;
  }
}

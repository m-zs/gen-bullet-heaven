using UnityEngine;

public class Targeted : MonoBehaviour, ITargetedAbility
{
  [SerializeField] private float damage = 50f;
  private float duration = 1f;
  private float timeUsed = 0f;
  private GameObject textObj;
  public CharacterManager owner;

  private void Update()
  {
    if (Time.time - timeUsed >= duration)
    {
      Destroy(textObj, 0.5f);
      Destroy(gameObject);
    }
  }

  public void Use(CharacterManager target)
  {
    if (target == null) return;
    timeUsed = Time.time;
    transform.position = target.transform.position;

    textObj = new GameObject("TargetText");
    textObj.transform.position = target.transform.position;
    TextMesh textMesh = textObj.AddComponent<TextMesh>();
    textMesh.text = "X";
    textMesh.fontSize = 20;
    textMesh.color = Color.red;
    textMesh.alignment = TextAlignment.Center;
    textMesh.anchor = TextAnchor.MiddleCenter;

    if (target.GetComponent<IDamageable>() != null)
    {
      target.GetComponent<IDamageable>().TakeDamage(damage);
    }
  }
}
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterEffects : MonoBehaviour
{
  private SpriteRenderer spriteRenderer;
  private Color originalColor;
  [SerializeField] private float flashDuration = 0.25f;
  private float flashTimer = 0f;
  private bool isFlashing = false;

  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    originalColor = spriteRenderer.color;
  }

  private void Update()
  {
    if (isFlashing)
    {
      flashTimer -= Time.deltaTime;
      if (flashTimer <= 0)
      {
        isFlashing = false;
        spriteRenderer.color = originalColor;
      }
    }
  }

  public void Flash()
  {
    spriteRenderer.color = Color.white;
    flashTimer = flashDuration;
    isFlashing = true;
  }

  public void SetFlashDuration(float duration)
  {
    flashDuration = duration;
  }
}
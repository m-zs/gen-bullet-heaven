using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class AreaOfEffect : MonoBehaviour, IAreaOfEffectAbility
{
    private SpriteRenderer spriteRenderer;
    private float duration = 1f;
    private float timeUsed = 0f;
    public float radius = 100f;
    private CircleCollider2D circleCollider;
    public CharacterManager owner;
    [SerializeField] private float damage = 100f;

    private Color[] possibleColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.one * (radius / 5f);
    }

    private void Update()
    {
        if (Time.time - timeUsed >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterManager character))
        {
            if (!CombatHelpers.IsEnemy(character, owner))
            {
                return;
            }
        }

        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Hit damageable");
            damageable.TakeDamage(damage);
        }
    }

    private void CreateRandomShape()
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] colors = new Color[32 * 32];

        // Choose a random shape type
        int shapeType = Random.Range(0, 3);
        Color shapeColor = possibleColors[Random.Range(0, possibleColors.Length)];

        switch (shapeType)
        {
            case 0: // Circle
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        float dx = x - 16;
                        float dy = y - 16;
                        float distance = Mathf.Sqrt(dx * dx + dy * dy);
                        colors[y * 32 + x] = distance < 14 ? shapeColor : Color.clear;
                    }
                }
                break;

            case 1: // Square
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        colors[y * 32 + x] = (x > 8 && x < 24 && y > 8 && y < 24) ? shapeColor : Color.clear;
                    }
                }
                break;

            case 2: // Triangle
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        float normalizedX = (x - 16) / 16f;
                        float normalizedY = (y - 16) / 16f;
                        colors[y * 32 + x] = (Mathf.Abs(normalizedX) + normalizedY < 1) ? shapeColor : Color.clear;
                    }
                }
                break;
        }

        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = sprite;
    }

    public void Spawn(Vector3 center)
    {
        transform.position = center;
        CreateRandomShape();
    }

    public void Use(Vector3 center)
    {
        timeUsed = Time.time;
        Spawn(center);
    }
}

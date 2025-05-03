using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour, IProjectileAbility
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool destroyOnHit = true;
    private CircleCollider2D circleCollider;
    public CharacterManager owner;

    private Color[] possibleColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 direction;
    private bool isSpawned;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
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

    public void Spawn(Vector3 origin, Vector3 direction)
    {
        transform.position = origin;
        this.direction = direction.normalized;
        isSpawned = true;
        CreateRandomShape();
    }

    private void Update()
    {
        if (!isSpawned) return;
        Vector3 movement = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        transform.position += movement;
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
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Use(Vector3 origin)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Spawn(origin, new Vector3(randomDirection.x, randomDirection.y, 0));
    }

    public void Use(Vector3 origin, Vector3 direction)
    {
        Spawn(origin, direction - origin);
    }
}

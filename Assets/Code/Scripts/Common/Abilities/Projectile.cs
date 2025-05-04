using UnityEngine;

public class Projectile : MonoBehaviour, IProjectileAbility
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool destroyOnHit = true;
    private Collider2D projectileCollider;
    [HideInInspector] public CharacterManager owner;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private bool isSpawned;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        projectileCollider = GetComponent<Collider2D>();
        if (GetComponent<Collider2D>() != null)
        {
            projectileCollider = GetComponent<Collider2D>();
            projectileCollider.isTrigger = true;
        }
    }

    public void Spawn(Vector3 origin, Vector3 direction)
    {
        transform.position = origin;
        this.direction = direction.normalized;
        isSpawned = true;

        // Left to right based
        float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

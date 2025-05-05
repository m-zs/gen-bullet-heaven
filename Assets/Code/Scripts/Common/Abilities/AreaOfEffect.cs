using UnityEngine;

public class AreaOfEffect : MonoBehaviour, IAreaOfEffectAbility
{
    private float duration = 2f;
    private float timeUsed = 0f;
    public float radius = 1f;
    public CharacterManager owner;
    [SerializeField] private float damage = 100f;
    private Collider2D aoeCollider;

    private void Awake()
    {
        aoeCollider = GetComponent<Collider2D>();
        if (aoeCollider != null)
        {
            aoeCollider.isTrigger = true;
        }
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
            damageable.TakeDamage(damage);
        }
    }

    public void Spawn(Vector3 center)
    {
        transform.position = center;
    }

    public void Use(Vector3 center)
    {
        timeUsed = Time.time;
        Spawn(center);
    }
}

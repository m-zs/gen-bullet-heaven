using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth = 100f;

    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public float spellDamage = 10f;
    [SerializeField] public float cooldownReduction = 0.1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(AbilityManager))]
public class CharacterManager : MonoBehaviour
{
    public CharacterAttributes characterAttributes;
    public AbilityManager abilityManager;

    public virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
        abilityManager = GetComponent<AbilityManager>();
    }
}

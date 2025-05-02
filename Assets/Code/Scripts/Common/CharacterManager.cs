using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(AbilityManager))]
public class CharacterManager : MonoBehaviour
{
    public CharacterAttributes characterAttributes;
    public AbilityManager abilityManager;
    public int teamId = 0;

    public virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
        abilityManager = GetComponent<AbilityManager>();
    }
}

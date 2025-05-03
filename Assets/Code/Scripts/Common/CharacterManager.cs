using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
[RequireComponent(typeof(AbilityManager))]
public class CharacterManager : MonoBehaviour
{
    public CharacterAttributes characterAttributes;
    public virtual AbilityManager abilityManager { get; private set; }
    public int teamId = 0;

    protected virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
    }
}

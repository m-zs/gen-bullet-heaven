using UnityEngine;

[RequireComponent(typeof(CharacterAttributes))]
public class CharacterManager : MonoBehaviour
{
    public CharacterAttributes characterAttributes;

    public virtual void Start()
    {
        characterAttributes = GetComponent<CharacterAttributes>();
    }

}

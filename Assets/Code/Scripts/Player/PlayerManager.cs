using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerManager : CharacterManager
{
    private PlayerMovement playerMovement;

    public override void Start()
    {
        base.Start();
        teamId = 1;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerMovement.Move(characterAttributes.movementSpeed);
    }
}

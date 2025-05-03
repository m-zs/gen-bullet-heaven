using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAbilityManager))]
public class PlayerManager : CharacterManager
{
    private PlayerMovement playerMovement;
    public new PlayerAbilityManager abilityManager { get; private set; }

    protected override void Start()
    {
        base.Start();
        teamId = 1;

        playerMovement = GetComponent<PlayerMovement>();
        abilityManager = GetComponent<PlayerAbilityManager>();
    }

    private void Update()
    {
        playerMovement.Move(characterAttributes.movementSpeed);
    }
}

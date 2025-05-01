using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerManager : CharacterManager
{
    private PlayerMovement playerMovement;

    public override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerMovement.Move(characterAttributes.movementSpeed);
    }
}

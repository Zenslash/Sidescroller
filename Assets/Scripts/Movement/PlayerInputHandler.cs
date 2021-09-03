using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var playersMovement = FindObjectsOfType<PlayerMovement>();
        var index = playerInput.playerIndex;
        playerMovement = playersMovement.FirstOrDefault(player => player.GetPlayerIndex() == index);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(playerInput.playerIndex);
        playerMovement.SetInputVector(context.ReadValue<Vector2>());
    }
}

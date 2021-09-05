using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovements playerMovements;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var playersMovement = FindObjectsOfType<PlayerMovements>();
        var index = playerInput.playerIndex;
        playerMovements = playersMovement.FirstOrDefault(player => player.GetPlayerIndex() == index);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerMovements != null)
        {
            playerMovements.SetInputVector(context.ReadValue<Vector2>());
        }
    }

    public void OnDisplace(InputAction.CallbackContext context)
    {
        if (playerMovements != null) //context.phase == InputActionPhase.Started)
        {
            if(context.started){
                playerMovements.SetDisplaceInput(true);
            }

            if (context.canceled)
            {
                playerMovements.SetDisplaceInput(false);
            }
            
        }
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract triggered");
    }
}

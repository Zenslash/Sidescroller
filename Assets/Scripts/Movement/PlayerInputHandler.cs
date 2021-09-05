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
    private PlayerMovements playerMovement;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var playersMovement = FindObjectsOfType<PlayerMovements>();
        var index = playerInput.playerIndex;
        playerMovement = playersMovement.FirstOrDefault(player => player.GetPlayerIndex() == index);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        playerMovement.SetInputVector(context.ReadValue<Vector2>());

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log("OnCrouch triggered");
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract triggered");
    }
}

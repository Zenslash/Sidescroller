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
        //Debug.Log("OnMove triggered");
        Debug.Log(context.ReadValue<Vector2>());
        playerMovement.SetInputVector(new Vector2(context.ReadValue<Vector2>().x, 0));

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log("OnCrouch triggered");
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract triggered");
    }
    
    public void OnStairsMoveUp(InputAction.CallbackContext context)
    {
        playerMovement.SetStairsButton(context.ReadValueAsButton());
    }

    public void OnStairsMoveDown(InputAction.CallbackContext context)
    {
        Debug.Log("OnStairMoveDown triggered");
    }
    
}

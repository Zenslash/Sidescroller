using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class PlayerInputHandler : MonoBehaviour
{
    const string KEYMOUSE = "Keyboard and mouse";
    const string GAMEPAD = "Gamepad";


    private PlayerInput playerInput;
    private PlayerMovements playerMovements;
    private PlayerStatsManager playerStatsManager;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var playersManagers = FindObjectsOfType<PlayerStatsManager>();
        var index = playerInput.playerIndex;
        playerStatsManager = playersManagers.FirstOrDefault(player => player.Movements.GetPlayerIndex() == index);  //playersMovement.FirstOrDefault(player => player.GetPlayerIndex() == index);
        playerMovements = playerStatsManager.Movements;
        
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
        if (playerMovements != null)
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


    public void OnAiming(InputAction.CallbackContext context)
    {
        playerStatsManager.Attack.Aim(context.performed);
        
    }

    public void OnSight(InputAction.CallbackContext context)
    {
        Vector3 direction = Vector2.zero;
        switch (playerInput.currentControlScheme)
        {
            case GAMEPAD:
                direction = new Vector3(100, 100) * context.ReadValue<Vector2>();           
                break;
            case KEYMOUSE:
                //TODO ZIS SHIET
                break;
                
        }
        playerStatsManager.Attack.Sight = direction;
    }

}

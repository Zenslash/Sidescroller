using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Mirror;



public class PlayerInputHandler : NetworkBehaviour
{
    const string KEYMOUSE = "Keyboard and mouse";
    const string GAMEPAD = "Gamepad";


    private PlayerInput playerInput;
    private PlayerMovements playerMovements;
    private PlayerStatsManager playerStatsManager;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerMovements = playerStatsManager.Movements;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerStatsManager != null)
            {
                playerStatsManager.Movements.IsRunning = context.performed;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerMovements != null)
            {
                playerMovements.SetInputVector(context.ReadValue<Vector2>());
            }
        }
    }

    public void OnDisplace(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerMovements != null)
            {
                if (context.started)
                {
                    playerMovements.SetDisplaceInput(true);
                }

                if (context.canceled)
                {
                    playerMovements.SetDisplaceInput(false);
                }
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract triggered");
    }


    public void OnAiming(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerInput != null)
            {
                playerStatsManager.Attack.Aim(context.performed);
            }
        }
    }

    public void OnSight(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerInput != null)
            {
                Vector3 direction = Vector2.zero;

                switch (playerInput.currentControlScheme)
                {
                    case GAMEPAD:
                        direction = new Vector3(100, 100) * context.ReadValue<Vector2>();
                        break;
                    case KEYMOUSE:
                        //TODO ZIS
                        break;
                }

                playerStatsManager.Attack.Sight = direction;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            if (playerInput != null)
            {
                if (context.performed)
                    playerStatsManager.Attack.Fire();
            }
        }
    }

}

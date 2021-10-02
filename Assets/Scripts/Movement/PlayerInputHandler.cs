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

    private PlayerStatsManager playerStatsManager;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
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
            playerStatsManager.Movements.InputVector = context.ReadValue<Vector2>();
        }

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(hasAuthority)
        {
            playerStatsManager.Movements.IsCrouching = context.performed;
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
                        //Debug.Log(context.ReadValue<Vector2>());
                        //direction = new Vector2(100, 100) * context.ReadValue<Vector2>();
                        direction = new Vector2(100 * (float)Math.Round((double)context.ReadValue<Vector2>().x, 1), 100 * (float)Math.Round((double)context.ReadValue<Vector2>().y, 2));
                        //Debug.Log(context.ReadValue<Vector2>() + " " + direction);
                        break;
                    case KEYMOUSE:
                        //TODO ZIS
                        break;
                }

                playerStatsManager.Attack.Sight = direction;
                playerStatsManager.Look.InputVector = direction;
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

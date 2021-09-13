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

    private PlayerStatsManager playerStatsManager;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        playerStatsManager.Movements.IsRunning = context.performed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y != 0)
        { 
            playerStatsManager.Movements.SetDisplaceInput(context.ReadValue<Vector2>());    
        }
        playerStatsManager.Movements.InputVector = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        playerStatsManager.Movements.IsCrouching = context.performed;
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerInput != null)
        {
            if (context.performed)
                playerStatsManager.Attack.Fire();
        }
    }

}

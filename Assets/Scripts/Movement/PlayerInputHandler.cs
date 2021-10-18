using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;



public class PlayerInputHandler : NetworkBehaviour
{
    const string KEYMOUSE = "Keyboard and mouse";
    const string GAMEPAD = "Gamepad";


    private PlayerInput playerInput;
    private Plane mousePlane;
    private Camera mainCamera;
    private PlayerStatsManager playerStatsManager;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        mainCamera = Camera.main;
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
          if (context.ReadValue<Vector2>().y != 0)
          {
              playerStatsManager.Movements.SetDisplaceInput(context.ReadValue<Vector2>());
          } 
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
                        direction = new Vector2(100 * context.ReadValue<Vector2>().x, 100 * context.ReadValue<Vector2>().y);
                        //Debug.Log(context.ReadValue<Vector2>() + " " + direction);
                        break;
                    case KEYMOUSE:
                        float distance;
                        mousePlane = new Plane(Vector3.forward, -playerStatsManager.transform.position.z);
                        Ray ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());
                        if(mousePlane.Raycast(ray,out distance))
                        {
                            direction = ray.GetPoint(distance);
                        }
                        direction = direction - playerStatsManager.transform.position;
                        direction = direction.normalized * 100;
                        Debug.Log(direction+" "+ context.ReadValue<Vector2>());
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

    public void OnInventory(InputAction.CallbackContext context)
    {
        Debug.Log("InventoryPress");
   
    }

}

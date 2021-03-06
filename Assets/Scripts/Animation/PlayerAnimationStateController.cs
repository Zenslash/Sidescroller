using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Mirror;

public class PlayerAnimationStateController : NetworkBehaviour
{
    #region Components
    private PlayerStatsManager playerStatsManager;
    private Animator animator;
    private float standWeight;
    private float crouchWeight;
    [Tooltip("Time it take for player to change from standing to crouching and backwards")]
    [SerializeField][Range(0,1)] private float crouchTime = 0.06f;
    
    #endregion
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }
    
    void Update()
    {
        if(hasAuthority)
        {
            UpdateAnimations();
        }
    }

    [Client]
    private void UpdateAnimations()
    {
        #region refactor
        standWeight = animator.GetLayerWeight(1); 
        crouchWeight = animator.GetLayerWeight(2); 
        if (playerStatsManager.Movements.IsCrouching)
        {
            crouchWeight = Mathf.Lerp(crouchWeight, 1f, crouchTime);
            crouchWeight = crouchWeight > 0.9999 ? 1 : crouchWeight;
            standWeight = 1 - crouchWeight;
        }
        else
        {
            standWeight = Mathf.Lerp(standWeight, 1f, crouchTime);
            standWeight = standWeight > 0.9999 ? 1 : standWeight;
            crouchWeight = 1 - standWeight;
        }
        #endregion

      animator.SetLayerWeight(1, standWeight);
      animator.SetLayerWeight(2, crouchWeight);
      animator.SetBool("isWalkingBackwards", playerStatsManager.Movements.IsWalkingBackwards);
      animator.SetBool("isRunning", playerStatsManager.Movements.IsRunning);
      animator.SetInteger("Input", Mathf.RoundToInt(playerStatsManager.Movements.InputVector.x));
      animator.SetFloat("Speed", Math.Abs(playerStatsManager.Movements.PlayerVelocity.magnitude));
      if (playerStatsManager.Movements.InputVector.x == 0 && !playerStatsManager.Movements.IsDisplacing)
      {
            animator.speed = 1; 
      }
      else if (playerStatsManager.Movements.IsDisplacing)
      {
          animator.speed = 1;
      }
      else 
      { 
          animator.speed = Mathf.Abs(playerStatsManager.Movements.PlayerVelocity.x) / playerStatsManager.Movements.GetMaxSpeed;
      }
    }
}

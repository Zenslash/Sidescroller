using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationStateController : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    private Animator animator;

    private float standWeight;
    private float crouchWeight;
    [SerializeField][Range(0,1)] private float crouchTime;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }


    public void CrouchTransition()
    {
       
    }
    void Update()
    {
        standWeight = animator.GetLayerWeight(1);
        crouchWeight = animator.GetLayerWeight(2);
        if (playerStatsManager.Movements.IsCrouching)
        {
            crouchWeight = Mathf.Lerp(crouchWeight, 1f, crouchTime);
                crouchWeight = crouchWeight > 0.99 ? 1 : crouchWeight;
                standWeight = 1 - crouchWeight;
        }
        else
        {

                standWeight = Mathf.Lerp(standWeight, 1f, crouchTime);
                standWeight = standWeight > 0.99 ? 1 : standWeight;
                crouchWeight = 1 - standWeight;
        }
        
        animator.SetLayerWeight(1, standWeight);
        animator.SetLayerWeight(2, crouchWeight);
        animator.SetBool("isWalkingBackwards", playerStatsManager.Movements.IsWalkingBackwards);
        animator.SetBool("isRunning", playerStatsManager.Movements.IsRunning);
        animator.SetInteger("Input", Mathf.RoundToInt(playerStatsManager.Movements.InputVector.x));
        animator.SetFloat("Speed", Math.Abs(playerStatsManager.Movements.GetPlayerVelocity.x));
        if (playerStatsManager.Movements.InputVector.x == 0)
        {
            animator.speed = 1;
        }
        else
        {
            animator.speed = Mathf.Abs(playerStatsManager.Movements.GetPlayerVelocity.x) / playerStatsManager.Movements.GetMaxSpeed;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    private PlayerMovements playerMovements;
    private PlayerStatsManager playerStatsManager;
    private Animator animator;
    private bool isIdle;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovements = GetComponent<PlayerMovements>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isAiming", playerStatsManager.Attack.IsAiming);
        animator.SetBool("isRunning", playerMovements.IsRunning);
        animator.SetInteger("Input", Mathf.RoundToInt(playerMovements.GetInputVector().x));
        animator.SetFloat("Speed", Math.Abs(playerMovements.GetPlayerVelocity.x));
        if (playerMovements.GetInputVector().x == 0)
        {
            animator.speed = 1;
        }
        else
        {
            animator.speed = Mathf.Abs(playerMovements.GetPlayerVelocity.x) / playerMovements.GetMaxSpeed;
        }

    }

    /*private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("test");
        if (animator != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            RaycastHit hit;
            Debug.Log(AvatarIKGoal.LeftFoot);
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistnceToGround + 1f))
            {
                Vector3 footPosition = hit.point;
                footPosition.y -= DistnceToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                //animator.SetIKRotation(AvatarIKGoal.LeftFoot, footPosition);
            }


        }
    }*/
}

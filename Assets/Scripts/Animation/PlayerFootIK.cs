using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    private Animator animator;
    private PlayerStatsManager playerStatsManager;
    [Range (0,1f)]
    public float DistnceToGround;

    private GameObject rightFootTarget;
    private GameObject leftFootTarget;
    public LayerMask LayerMask;

    public GameObject RightFootTarget
    {
        set => rightFootTarget = value;
    }

    public GameObject LeftFootTarget
    {
        set => leftFootTarget = value;
    }
    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            if (playerStatsManager.Movements.IsOnLadder)
            {
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.transform.position);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.transform.position);
                return;
            }

            
            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistnceToGround + 1f, LayerMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += DistnceToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistnceToGround + 1f, LayerMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += DistnceToGround;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }


        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    #region Components
    private Animator animator;
    private PlayerStatsManager playerStatsManager;
    [Range (0,1f)]
    [Tooltip("Foot offset, less is lower")]
    [SerializeField] private float distanceToGround;

    private GameObject rightFootTarget;
    private GameObject leftFootTarget;
    
    [Tooltip("Layers for which foot IK works")]
    [SerializeField]private LayerMask layerMask;

    /// <summary>
    /// sets new target for right foot
    /// </summary>
    public GameObject RightFootTarget
    {
        set => rightFootTarget = value;
    }
    /// <summary>
    /// sets new target for left foot
    /// </summary>
    public GameObject LeftFootTarget
    {
        set => leftFootTarget = value;
    }
    #endregion
    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        animator = GetComponentInParent<Animator>();
    }

    private void SetWeights(float RFW, float LFW, float RFRW, float LFRW){
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LFW);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, LFRW);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, RFW);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, RFRW);
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }

        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            SetWeights(1f, 1f, 1f,1f);
            if (playerStatsManager.Movements.IsOnLadder && rightFootTarget != null && leftFootTarget != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.transform.position);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.transform.position);
                return;
            }
            GroundCheck();
        }
    }
}
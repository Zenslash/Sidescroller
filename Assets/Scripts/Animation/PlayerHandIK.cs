using System;
using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using UnityEngine;

public class PlayerHandIK : MonoBehaviour
{
    #region Components
    private Animator animator;
    [Header("Targets")]
    [Tooltip("IK target for right hand")]
    [SerializeField] private GameObject rightHandTarget;
    [Tooltip("IK target for left hand")]
    [SerializeField] private GameObject leftHandTarget;
    [Tooltip("Target for left hand elbow")]
    [SerializeField] private GameObject leftHandHint;
    [Tooltip("Target for right hand elbow")]
    [SerializeField] private GameObject rightHandHint;
    [Header("Weights")]
    [SerializeField] [Range(0, 1)] private float leftHandIKWeight = 0f;
    [SerializeField] [Range(0, 1)] private float rightHandIKWeight = 0f;

    public GameObject RightHandTarget
    {
        set => rightHandTarget = value;
    }
    public GameObject LeftHandTarget
    {
        set => leftHandTarget = value;
    }
    #endregion
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandIKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandIKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
        if (rightHandTarget != null)
        {
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHandHint.transform.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.transform.rotation);
        }

        if (leftHandTarget != null)
        {
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftHandHint.transform.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.transform.rotation);
        }
        
        
    }
}

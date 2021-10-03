using System;
using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using UnityEngine;

public class PlayerHandIK : MonoBehaviour
{
    private Animator animator;

    public GameObject RightHandTarget;
    public GameObject LeftHandTarget;
    public GameObject LeftHandHint;
    public GameObject RightHandHint;
    [SerializeField] [Range(0,1)] private float LeftHandIKWeight = 0f;
    [SerializeField] [Range(0,1)] private float RightHandIKWeight = 0f; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, LeftHandIKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, RightHandIKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftHandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftHandIKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandIKWeight);
        if (RightHandTarget != null)
        {
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightHandHint.transform.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.transform.rotation);
        }

        if (LeftHandTarget != null)
        {
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftHandHint.transform.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.transform.rotation);
        }
        
        
    }
}

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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1f);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
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

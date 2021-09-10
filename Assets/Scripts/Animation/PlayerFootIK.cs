using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootIK : MonoBehaviour
{
    private Animator animator;
    [Range (0,1f)]
    public float DistnceToGround;

    public LayerMask LayerMask;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
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
    }
}
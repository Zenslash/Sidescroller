using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    private StairsTrigger stairsTrigger;
    [SerializeField] private GameObject rightHandTarget;
    [SerializeField] private GameObject leftHandTarget;
    [SerializeField] private GameObject rightFootTarget;
    [SerializeField] private GameObject leftFootTarget;
    private GameObject closestHandPointR;
    private GameObject closestHandPointL;
    private GameObject closestFootPointR;
    private GameObject closestFootPointL;
    
    [SerializeField]private GameObject[] pointsIKRight;
    [SerializeField]private GameObject[] pointsIKLeft;
   
    [SerializeField] [Range(0, 1)] private float handMoveTime = 0.05f;
    [SerializeField] [Range(0, 1)] private float handPosOffset = 0.1f;
    [SerializeField] [Range(0, 1)] private float legMoveTime = 0.05f;
    [SerializeField] [Range(0, 1)] private float legPosOffset = 0.1f;
    [SerializeField] [Range(0, 1)] private float desiredPosMoveTime = 0.1f;

    [SerializeField] [Range(0, 10)] private float handYOffset;
    [SerializeField] [Range(-1, 1)] private float footYOffset;

    public PlayerStatsManager PlayerStatsManager
    {
        set => playerStatsManager = value;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerStatsManager == null) return;
        Gizmos.DrawWireSphere(new Vector3(playerStatsManager.Movements.PlayerLocalTransform.position.x,playerStatsManager.Movements.PlayerLocalTransform.position.y + footYOffset, playerStatsManager.Movements.PlayerLocalTransform.position.z), 0.5f);
        Gizmos.DrawWireSphere(new Vector3(playerStatsManager.Movements.PlayerLocalTransform.position.x,playerStatsManager.Movements.PlayerLocalTransform.position.y + handYOffset, playerStatsManager.Movements.PlayerLocalTransform.position.z), 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(closestHandPointR.transform.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rightHandTarget.transform.position, 0.1f);
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(desiredPos, 0.1f);
    }

    private void Awake()
    {
        closestHandPointL = pointsIKLeft[0];
        closestHandPointR = pointsIKRight[0];
        closestFootPointL = pointsIKLeft[0];
        closestFootPointR = pointsIKRight[0];
        stairsTrigger = GetComponentInChildren<StairsTrigger>();
    }

    private Vector3 lerp3(Vector3 closestPoint, Vector3 limbPosition, float moveTime, float offset)
    {
        Vector3 desiredPos = limbPosition;
        desiredPos =  Vector3.Lerp(desiredPos, closestPoint, desiredPosMoveTime);
        if (Mathf.Abs(closestPoint.y - desiredPos.y) > 0.1)
        {
            desiredPos.z -= offset;
        }
        
        return Vector3.Lerp(limbPosition, desiredPos, moveTime);
    }

    private void Update()
    {
        if (playerStatsManager == null) return;
        if(!playerStatsManager.Movements.IsOnLadder) return;
        playerStatsManager.HandIK.RightHandTarget = rightHandTarget;
        playerStatsManager.HandIK.LeftHandTarget = leftHandTarget;
        playerStatsManager.FootIK.RightFootTarget = rightFootTarget;
        playerStatsManager.FootIK.LeftFootTarget = leftFootTarget;
        var correctedPlayerHandPosition = playerStatsManager.Movements.PlayerLocalTransform.position;
        var correctedPlayerFootPosition = correctedPlayerHandPosition;
        correctedPlayerFootPosition.y += footYOffset;
        correctedPlayerHandPosition.y += handYOffset;
        foreach (var point in pointsIKRight)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerHandPosition) < Vector3.Distance(closestHandPointR.transform.position, correctedPlayerHandPosition))
            {
                closestHandPointR = point;
            }
        }
        foreach (var point in pointsIKRight)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerFootPosition) < Vector3.Distance(closestFootPointR.transform.position, correctedPlayerFootPosition))
            {
                closestFootPointR = point;
            }
        }
        foreach (var point in pointsIKLeft)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerHandPosition) < Vector3.Distance(closestHandPointL.transform.position, correctedPlayerHandPosition))
            {
                closestHandPointL = point;
            }
        }
        foreach (var point in pointsIKLeft)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerFootPosition) < Vector3.Distance(closestFootPointL.transform.position, correctedPlayerFootPosition))
            {
                closestFootPointL = point;
            }
        }
        rightHandTarget.transform.position = 
            lerp3(closestHandPointR.transform.position, rightHandTarget.transform.position, handMoveTime, handPosOffset);
        leftHandTarget.transform.position =
            lerp3(closestHandPointL.transform.position, leftHandTarget.transform.position, handMoveTime, handPosOffset);
        rightFootTarget.transform.position = 
            lerp3(closestFootPointR.transform.position, rightFootTarget.transform.position, legMoveTime, legPosOffset);
        leftFootTarget.transform.position = 
            lerp3(closestFootPointL.transform.position, leftFootTarget.transform.position, legMoveTime, legPosOffset);
    }
}

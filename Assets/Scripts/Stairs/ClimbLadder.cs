using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    #region Components
    private PlayerStatsManager playerStatsManager;
    private StairsTrigger stairsTrigger;
    private GameObject closestHandPointR;
    private GameObject closestHandPointL;
    private GameObject closestFootPointR;
    private GameObject closestFootPointL;
    
    [Header("Targets")]
    [SerializeField] private GameObject rightHandTarget;
    [SerializeField] private GameObject leftHandTarget;
    [SerializeField] private GameObject rightFootTarget;
    [SerializeField] private GameObject leftFootTarget;
    [Header("Points")]
    [SerializeField]private GameObject[] pointsIKRight;
    [SerializeField]private GameObject[] pointsIKLeft;
    [Header("Hand settings")]
    
    [Tooltip("Speed of hand chasing desired position. less is slower")]
    [SerializeField] [Range(0, 1)] private float handMoveSpeed = 0.05f;
    
    [Tooltip("Offset that leg hand travel back when moving bars")]
    [SerializeField] [Range(0, 1)] private float handPosOffset = 0.1f;
    
    [Tooltip("Offset that defines what position the next bar will be found")]
    [SerializeField] [Range(0, 10)] private float handYOffset;
    
    [Header("Leg settings")]
    [Tooltip("Speed of leg chasing desired position. less is slower")]
    [SerializeField] [Range(0, 1)] private float legMoveSpeed = 0.05f;
    
    [Tooltip("Offset that leg will travel back when moving bars")]
    [SerializeField] [Range(0, 1)] private float legPosOffset = 0.1f;
    
    [Tooltip("Offset that defines what position the next bar will be found")]
    [SerializeField] [Range(-1, 1)] private float footYOffset;
    [Space]
    [Tooltip("Speed of desired position moved. less is slower")]
    [SerializeField] [Range(0, 1)] private float desiredPosMoveTime = 0.1f;

    
    

    private Vector3 correctedHandPosition;
    private Vector3 correctedFootPosition;

    public PlayerStatsManager PlayerStatsManager
    {
        set => playerStatsManager = value;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (playerStatsManager == null) return;
        Gizmos.color = Color.white;
        var position = playerStatsManager.Movements.PlayerLocalTransform.position;
        Gizmos.DrawWireSphere(new Vector3(position.x,position.y + footYOffset, position.z), 0.5f); //Foot offset sphere
        Gizmos.DrawWireSphere(new Vector3(position.x,position.y + handYOffset, position.z), 0.5f); //Hand offset sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(closestHandPointR.transform.position, 0.1f); //Closest bar of ladder
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rightHandTarget.transform.position, 0.1f); //Current target position of right hand
    }
    private void Awake()
    {
        closestHandPointL = pointsIKLeft[0];
        closestHandPointR = pointsIKRight[0];
        closestFootPointL = pointsIKLeft[0];
        closestFootPointR = pointsIKRight[0];
        stairsTrigger = GetComponentInChildren<StairsTrigger>();
    }
    
    
    /// <summary>
    /// Creates an arc between point A and point B with the height equals offset. Arc is pointed the opposite way of stairs target.
    /// </summary>
    /// <param name="pointA"> start position </param>
    /// <param name="pointB"> target position </param>
    /// <param name="moveSpeed"> Offset move speed</param>
    /// <param name="offset"> Height of the arc</param>
    /// <returns> Vector3. Position on arc</returns>
    private Vector3 ArcLerp(Vector3 pointA, Vector3 pointB, float moveSpeed, float offset)
    {
        var desiredPos = pointB;
        desiredPos =  Vector3.Lerp(desiredPos, pointA, desiredPosMoveTime);
        if (Mathf.Abs(pointA.y - desiredPos.y) > 0.1)
        {
            desiredPos -= stairsTrigger.TargetPosition.transform.forward*offset;
        }
        
        return Vector3.Lerp(pointB, desiredPos, moveSpeed);
    }

    public void AssignTargets()
    {
        if (!playerStatsManager.Movements.IsDisplaced) return;
        playerStatsManager.HandIK.RightHandTarget = rightHandTarget;
        playerStatsManager.HandIK.LeftHandTarget = leftHandTarget;
        playerStatsManager.FootIK.RightFootTarget = rightFootTarget;
        playerStatsManager.FootIK.LeftFootTarget = leftFootTarget;
    }

    private void DismissTargets()
    {
        if (!playerStatsManager.Movements.IsDisplaced || !playerStatsManager.Movements.IsDisplacing) return;
        playerStatsManager.HandIK.RightHandTarget = null;
        playerStatsManager.HandIK.LeftHandTarget = null;
        playerStatsManager.FootIK.RightFootTarget = null;
        playerStatsManager.FootIK.LeftFootTarget = null;
    }
    
    
    /// <summary>
    /// Updates limb position every update.
    /// </summary>
    private void CorrectHandFootPosition()
    {
        correctedHandPosition = playerStatsManager.Movements.PlayerLocalTransform.position;
        correctedFootPosition = correctedHandPosition;
        correctedFootPosition.y += footYOffset;
        correctedHandPosition.y += handYOffset;
    }
    /// <summary>
    /// Finds closest point to limb position
    /// </summary>
    /// <param name="points">Array of points to search </param>
    /// <param name="closestPoint">Current closest point</param>
    /// <param name="correctedPosition">Position from which to search</param>
    /// <returns>GameObject. Closest point to corrected position</returns>
    private GameObject FindClosestPoint(GameObject[] points, GameObject closestPoint, Vector3 correctedPosition)
    {
        foreach (var point in points)
        {
            if(Vector3.Distance(point.transform.position, correctedPosition) < Vector3.Distance(closestPoint.transform.position, correctedPosition))
            { 
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private void Update()
    {
        if (playerStatsManager == null)  return;
        DismissTargets();
        if(!playerStatsManager.Movements.IsOnLadder) return;
        CorrectHandFootPosition();
        closestHandPointR = FindClosestPoint(pointsIKRight, closestHandPointR, correctedHandPosition);
        closestHandPointL = FindClosestPoint(pointsIKLeft, closestHandPointL, correctedHandPosition);
        closestFootPointR = FindClosestPoint(pointsIKRight, closestFootPointR, correctedFootPosition);
        closestFootPointL = FindClosestPoint(pointsIKLeft, closestFootPointL, correctedFootPosition);
        rightHandTarget.transform.position = 
            ArcLerp(closestHandPointR.transform.position, rightHandTarget.transform.position, handMoveSpeed, handPosOffset);
        rightHandTarget.transform.rotation = closestHandPointR.transform.rotation;
        leftHandTarget.transform.position =
            ArcLerp(closestHandPointL.transform.position, leftHandTarget.transform.position, handMoveSpeed, handPosOffset);
        leftHandTarget.transform.rotation = closestHandPointL.transform.rotation;
        rightFootTarget.transform.position = 
            ArcLerp(closestFootPointR.transform.position, rightFootTarget.transform.position, legMoveSpeed, legPosOffset);
        leftFootTarget.transform.position = 
            ArcLerp(closestFootPointL.transform.position, leftFootTarget.transform.position, legMoveSpeed, legPosOffset);
    }
}

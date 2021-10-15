using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    private StairsTrigger stairsTrigger;
    [SerializeField]private GameObject[] pointsIKRight;
    [SerializeField]private GameObject[] pointsIKLeft;
    private GameObject closestHandPointR;
    private GameObject closestHandPointL;
    private GameObject closestFootPointR;
    private GameObject closestFootPointL;

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

    private void Awake()
    {
        closestHandPointL = pointsIKLeft[0];
        closestHandPointR = pointsIKRight[0];
        closestFootPointL = pointsIKLeft[0];
        closestFootPointR = pointsIKRight[0];
        stairsTrigger = GetComponentInChildren<StairsTrigger>();
    }

    private void Update()
    {
        if (playerStatsManager == null) return;
        if(!playerStatsManager.Movements.IsOnLadder) return;
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
        
        
        Debug.Log("cp.n " + closestHandPointR.name);
        playerStatsManager.HandIK.RightHandTarget = closestHandPointR;
        playerStatsManager.HandIK.LeftHandTarget = closestHandPointL;
        playerStatsManager.FootIK.LeftFootTarget = closestFootPointL;
        playerStatsManager.FootIK.RightFootTarget = closestFootPointR;
        Debug.Log("psm.h.rht " + playerStatsManager.HandIK.RightHandTarget);
    }
}

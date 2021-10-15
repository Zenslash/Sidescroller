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
    private GameObject closestPointR;
    private GameObject closestPointL;

    [SerializeField] [Range(0, 50)] private float handYOffset;

    public PlayerStatsManager PlayerStatsManager
    {
        set => playerStatsManager = value;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerStatsManager == null) return;
        Gizmos.DrawWireSphere(new Vector3(playerStatsManager.Movements.PlayerLocalTransform.position.x,playerStatsManager.Movements.PlayerLocalTransform.position.y + handYOffset, playerStatsManager.Movements.PlayerLocalTransform.position.z), 0.5f);
    }

    private void Awake()
    {
        closestPointL = pointsIKLeft[0];
        closestPointR = pointsIKRight[0];
        stairsTrigger = GetComponentInChildren<StairsTrigger>();
    }

    private void Update()
    {
        if (playerStatsManager == null) return;
        if(!playerStatsManager.Movements.IsOnLadder) return;
        var correctedPlayerPosition = playerStatsManager.Movements.PlayerLocalTransform.position;
        correctedPlayerPosition.y += handYOffset;
        foreach (var point in pointsIKRight)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerPosition) < Vector3.Distance(closestPointR.transform.position, correctedPlayerPosition))
            {
                closestPointR = point;
            }
        }
        foreach (var point in pointsIKLeft)
        {
            if(Vector3.Distance(point.transform.position, correctedPlayerPosition) < Vector3.Distance(closestPointL.transform.position, correctedPlayerPosition))
            {
                closestPointL = point;
            }
        }
        
        
        Debug.Log("cp.n " + closestPointR.name);
        playerStatsManager.HandIK.RightHandTarget = closestPointR;
        playerStatsManager.HandIK.LeftHandTarget = closestPointL;
        Debug.Log("psm.h.rht " + playerStatsManager.HandIK.RightHandTarget);
    }
}

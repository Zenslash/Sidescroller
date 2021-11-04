using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    private ClimbLadder climbLadder;
    [Tooltip("Position to which player moves")]
    [SerializeField] private GameObject targetPosition;
    [Tooltip("Sets if script used for ladder or stairs")]
    [SerializeField] private bool ladderTrigger = false;
    

    public GameObject TargetPosition => targetPosition;

    public void Awake()
    {
        climbLadder = GetComponentInParent<ClimbLadder>();
    }

    public void OnTriggerEnter(Collider other)
    {
        playerStatsManager = other.GetComponentInParent<PlayerStatsManager>();
        if(climbLadder != null) 
            climbLadder.PlayerStatsManager = playerStatsManager;
    }

    private void OnTriggerStay(Collider other)
    {
        if (ladderTrigger)
        {
            climbLadder.AssignTargets();
        }
        playerStatsManager.Movements.SetDisplacementTarget(targetPosition.transform, ladderTrigger);
    }

    private void OnTriggerExit(Collider other)
    {

        if (climbLadder != null)
        {
            climbLadder.PlayerStatsManager = null;
            
        }
    }
}

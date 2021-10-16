using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    private PlayerStatsManager playerStatsManager;
    [SerializeField] private GameObject targetPosition;
    [SerializeField] private bool ladderTrigger = false;
    [SerializeField] private ClimbLadder climbLadder;

    public void Awake()
    {
        climbLadder = GetComponentInParent<ClimbLadder>();
    }

    public void OnTriggerEnter(Collider other)
    {
        playerStatsManager = other.GetComponentInParent<PlayerStatsManager>();
        climbLadder.PlayerStatsManager = playerStatsManager;
    }

    private void OnTriggerStay(Collider other)
    {
        playerStatsManager.Movements.Displace(targetPosition.transform, ladderTrigger);
    }

    private void OnTriggerExit(Collider other)
    {
        climbLadder.PlayerStatsManager = null;
    }
}

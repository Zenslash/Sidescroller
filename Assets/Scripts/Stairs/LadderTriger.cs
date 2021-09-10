using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTriger : MonoBehaviour
{
    private Transform ladderTransform;
    private PlayerStatsManager playerStatsManager;
    public void Awake()
    {
        ladderTransform = GetComponentInParent<Transform>();
    }

    public void OnTriggerEnter(Collider other)
    {
        playerStatsManager = other.GetComponentInParent<PlayerStatsManager>();
    }

    public void OnTriggerStay(Collider other)
    {
        playerStatsManager.Movements.MoveToLadder(ladderTransform.position.x);
    }
}

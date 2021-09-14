using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTriger : MonoBehaviour
{
    private Transform ladderTransform;
    private PlayerMovements playerMovements;
    public void Awake()
    {
        ladderTransform = GetComponentInParent<Transform>();
    }

    public void OnTriggerEnter(Collider other)
    {
        playerMovements = other.GetComponent<PlayerMovements>();
    }

    public void OnTriggerStay(Collider other)
    {
        playerMovements.MoveToLadder(ladderTransform.position.x);
    }
}

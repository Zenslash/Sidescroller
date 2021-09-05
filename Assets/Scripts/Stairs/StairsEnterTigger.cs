using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsEnterTigger : MonoBehaviour
{

    [SerializeField] private bool isLadder = false;
    private void OnTriggerStay(Collider other)
    {
        if (isLadder)
        {
            var pos = GetComponentInParent<Transform>().position.x;
            other.GetComponent<PlayerMovements>().MoveToLadder(pos);
        }
        else
        {
            other.GetComponent<PlayerMovements>().MoveToStairs();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLadder)
        {
            Debug.Log("LadderExit");
        }
    }
}

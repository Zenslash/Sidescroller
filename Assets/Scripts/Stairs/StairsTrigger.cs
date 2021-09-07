using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    private PlayerMovements playerMovements;
    
    private void OnTriggerEnter(Collider other)
    {
        playerMovements = other.GetComponent<PlayerMovements>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerMovements != null)
        {
            playerMovements.MoveToStairs();
        }
}


}

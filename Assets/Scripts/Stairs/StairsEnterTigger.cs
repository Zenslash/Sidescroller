using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsEnterTigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<PlayerMovement>().MoveToStairs();
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerMovement>().SetStairsButton(false);
    }
}

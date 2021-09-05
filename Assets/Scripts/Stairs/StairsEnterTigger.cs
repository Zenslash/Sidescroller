using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsEnterTigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<PlayerMovements>().MoveToStairs();
    }
}

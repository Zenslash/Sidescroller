using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnStairsTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerMovement>().MoveFromStairs();
    }

}

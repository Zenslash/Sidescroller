using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    protected float moveSpeed = 1f;

    public float MoveSpeed
    {
        get => moveSpeed;
    }
}

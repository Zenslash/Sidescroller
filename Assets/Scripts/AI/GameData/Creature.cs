using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class Creature : NetworkBehaviour
{
    protected float moveSpeed = 1f;

    public float MoveSpeed
    {
        get => moveSpeed;
    }
}

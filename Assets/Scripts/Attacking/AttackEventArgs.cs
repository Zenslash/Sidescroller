using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventArgs 
{
    public Vector3 AttackDirection;
    public float RecoilPower;

    public AttackEventArgs(Vector3 attackDirection, float recoilPower)
    {
        AttackDirection = attackDirection;
        RecoilPower = recoilPower;
    }
}

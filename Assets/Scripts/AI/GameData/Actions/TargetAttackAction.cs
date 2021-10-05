using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttackAction : GoapAction
{
    private void Awake()
    {
        AddPrecondition("hasTarget", true);
        AddEffect("targetDestroyed", true);
    }

    protected override void Reset()
    {
        
    }

    public override bool IsDone()
    {
        return false;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        Debug.Log("TargetAttackAction performed");

        return true;
    }

    public override bool RequiresInRange()
    {
        return false;
    }
}

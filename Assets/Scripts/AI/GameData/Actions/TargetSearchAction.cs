   
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSearchAction : GoapAction
{
    private void Awake()
    {
        AddPrecondition("hasTarget", false);
        AddPrecondition("hasTargetSeenRecently", true);
        AddEffect("hasTarget", true);
        AddEffect("hasTargetSeenRecently", true);
    }

    protected override void Reset()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override bool IsDone()
    {
         //If target was finded
        return false;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        //Search
        
        //If after delay didnt find
         //Go to wandering

        return true;
    }

    public override bool RequiresInRange()
    {
        return false;
    }
}

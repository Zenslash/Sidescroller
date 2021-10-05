using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : SmartCreature
{
    [Header("Base Enemy")]
    [SerializeField] protected bool isRanged = false;
    [SerializeField] protected float meleeAttackRange;
    [SerializeField] protected float rangedAttackRange;

    protected HealthComponent target;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldState = new HashSet<KeyValuePair<string, object>>();
        
        worldState.Add(new KeyValuePair<string, object>("hasTarget", (target != null && target.IsAlive())));
        worldState.Add(new KeyValuePair<string, object>("hasRanged", isRanged));

        return worldState;
    }

    public override HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();

        goals.Add(new KeyValuePair<string, object>("targetDestroyed", true));

        return goals;
    }

    public override void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        // Make sure the world state has changed before running
        // the same goal again, or else it will just fail.
    }

    public override void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        Debug.Log ("<color=green>Plan found</color> "+ GoapAgent.PrettyPrint(actions));
    }

    public override void ActionsFinished()
    {
        // Everything is done, we completed our actions for this gool
        Debug.Log ("<color=blue>Actions completed</color>");
    }

    public override void PlanAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.PrettyPrint(aborter));
    }

    public override bool MoveAgent(GoapAction nextAction)
    {
        if (Vector3.Distance(transform.position, nextAction.target.transform.position) <= 0.1f)
        {
            return true;
        }
        
        //Update Per Frame?
        //Maybe we should update it not so often?
        navMeshAgent.SetDestination(nextAction.target.transform.position);
        return false;
    }
}

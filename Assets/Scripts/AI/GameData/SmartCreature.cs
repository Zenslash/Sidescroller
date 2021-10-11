using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class SmartCreature : Creature, IGoap
{

    protected NavMeshAgent      navMeshAgent;
    protected GoapAgent         goap;
    protected SmartCreatureView view;

    public abstract HashSet<KeyValuePair<string, object>> GetWorldState();
    public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

    public abstract void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    public abstract void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

    public abstract void ActionsFinished();

    public abstract void PlanAborted(GoapAction aborter);

    public abstract bool MoveAgent(GoapAction nextAction);
}

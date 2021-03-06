using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 * 
 * It also provides an interface for the planner to give 
 * feedback to the Agent and report success/failure.
 */
public interface IGoap
{
    
    /**
     * The starting state of the Agent and the world.
     * Supply what states are needed for actions to run
     */
    HashSet<KeyValuePair<string, object>> GetWorldState();

    /**
     * Give the planner a new goal so it can figure out
     * the actions needed to fulfill it
     */
    HashSet<KeyValuePair<string, object>> CreateGoalState();

    /**
     * No sequence of actions could be found for the supplied goal.
     * You will need to try another goal
     */
    void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    /**
     * A plan was found for the supplied goal
     * These are the actions the Agent will perform, in order
     */
    void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

    /**
     * All actions are complete and the goal was reached
     */
    void ActionsFinished();

    /**
     * One of the actions aborted the plan
     * The actions is returned
     */
    void PlanAborted(GoapAction aborter);

    /**
     * Called during Update. Move the agent towards the target in order
     * for the next action to be able to perform.
     * Return true if Agent is at the target and the next action can perform
     * Return false if it not there yet
     */
    bool MoveAgent(GoapAction nextAction);
}

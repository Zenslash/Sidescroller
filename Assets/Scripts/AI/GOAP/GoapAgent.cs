using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class GoapAgent : NetworkBehaviour
{

    private FSM stateMachine;

    private FSM.FSMState idleState;             //find somethind to do
    private FSM.FSMState moveToState;           //moves to a target
    private FSM.FSMState performActionState;    //perform action

    private HashSet<GoapAction> availableActions;
    private Queue<GoapAction>   currentActions;
    private GoapAction          performingAction;

    private IGoap dataProvider;                 //this is the implementing class that provides our world data and
                                                //listens to feedback on planning

    private GoapPlanner planner;

    public delegate void ActionDelegate(GoapAction action);

    public ActionDelegate OnActionChanged;
    
    public GoapAction PerformingAction
    {
        get => performingAction;
        set
        {
            performingAction = value;
            OnActionChanged.Invoke(performingAction);
        }
    }

    private void Start()
    {
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        FindDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();
        stateMachine.PushState(idleState);
        LoadActions();
    }

    private void Update()
    {
        stateMachine.Update(this.gameObject);
    }

    public void AddAction(GoapAction a)
    {
        availableActions.Add(a);
    }

    public GoapAction GetAction(Type action)
    {
        foreach (GoapAction g in availableActions)
        {
            if (g.GetType().Equals(action))
            {
                return g;
            }
        }
        return null;
    }

    public void RemoveAction(GoapAction action)
    {
        availableActions.Remove(action);
    }

    private bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    [Server]
    private void CreateIdleState()
    {
        idleState = (fsm, gameObj) =>
        {
            //GOAP Planning
            
            //get the world state and the goal we want to plan for
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
            HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();
            
            //Plan
            Queue<GoapAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                //we have a plan
                currentActions = plan;
                dataProvider.PlanFound(goal, plan);
                
                fsm.PopState();
                fsm.PushState(performActionState);
            }
            else
            {
                //we couldn't get a plan
                Debug.Log("<color=orange>Failed Plan:</color>" + PrettyPrint(goal));
                dataProvider.PlanFailed(goal);
                fsm.PopState();
                fsm.PushState(idleState);
            }
        };
    }

    [Server]
    private void CreateMoveToState()
    {
        //Move to the target state
        moveToState = (fsm, o) =>
        {
            GoapAction action = currentActions.Peek();
            if (action.RequiresInRange() && action.target == null)
            {
                Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed." +
                          " You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.PopState(); //Move
                fsm.PopState(); //Perform
                fsm.PushState(idleState);
                return;
            }
            
            //get the agent to move itself
            if (dataProvider.MoveAgent(action))
            {
                fsm.PopState();
            }
        };
    }

    [Server]
    private void CreatePerformActionState()
    {
        performActionState = (fsm, gameObj) =>
        {
            //perform action

            if (!HasActionPlan())
            {
                //no actions to perform
                Debug.Log("<color=red>Done actions</color>");
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.IsDone())
            {
                action.OnExit();
                //the action is done. Remove it so we can perform the next one
                currentActions.Dequeue();
            }

            if (HasActionPlan())
            {
                //perform the next action
                action = currentActions.Peek();
                bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                if (inRange)
                {
                    //we are in range, so perform action
                    PerformingAction = action;
                    bool success = action.Perform(gameObj);

                    if (!success)
                    {
                        //action failed, we need to plan again
                        fsm.PopState();
                        fsm.PushState(idleState);
                        dataProvider.PlanAborted(action);
                    }
                }
                else
                {
                    //we need to move there first
                    fsm.PushState(moveToState);
                }
            }
            else
            {
                //no actions left, move to the Plan state
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
            }
        };
    }

    [Server]
    private void FindDataProvider()
    {
        Component[] components = gameObject.GetComponents(typeof(Component));
        foreach (Component comp in components)
        {
            if (comp is IGoap)
            {
                dataProvider = (IGoap) comp;
                return;
            }
        }
    }

    [Server]
    private void LoadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction action in actions)
        {
            availableActions.Add(action);
        }
        Debug.Log("Found actions: " + PrettyPrint(actions));
    }
    
    public static string PrettyPrint(HashSet<KeyValuePair<string,object>> state) {
        String s = "";
        foreach (KeyValuePair<string,object> kvp in state) {
            s += kvp.Key + ":" + kvp.Value.ToString();
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(Queue<GoapAction> actions) {
        String s = "";
        foreach (GoapAction a in actions) {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }

    public static string PrettyPrint(GoapAction[] actions) {
        String s = "";
        foreach (GoapAction a in actions) {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(GoapAction action) {
        String s = ""+action.GetType().Name;
        return s;
    }
}

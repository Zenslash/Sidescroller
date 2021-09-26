using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

/**
 * Plans what actions can be completed to fulfill goal
 */
public class GoapPlanner
{

    /**
     * Plan what sequence of actions can fulfill the goal
     * Returns null if actions couldn't be found or list of actions
     * that must be performed, in order, to fulfill the goal
     */
    public Queue<GoapAction> Plan(GameObject agent,
                                    HashSet<GoapAction> availableActions,
                                    HashSet<KeyValuePair<string, object>> worldState,
                                    HashSet<KeyValuePair<string, object>> goal)
    {
        //reset the actions so we can start fresh with them
        foreach (GoapAction a in availableActions)
        {
            a.DoReset();
        }
        
        //check what actions can run using CheckProceduralPrecondition
        HashSet<GoapAction> usableActions = new HashSet<GoapAction>();
        foreach (GoapAction action in availableActions)
        {
            if (action.CheckProceduralPrecondition(agent))
            {
                usableActions.Add(action);
            }
        }
        
        //we now have all actions that can run, stored in usableActions
        
        //build up the tree and record the leaf nodes that provide a solution to the goal
        List<Node> leaves = new List<Node>();
        
        //build graph
        Node start = new Node(null, 0, worldState, null);
        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("We dont have a plan for " + agent.name);
            return null;
        }
        
        //get the cheapest leaf
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }
        
        //get its node and work back through the parents
        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }

            n = n.parent;
        }
        
        //we now have action list in correct order

        Queue<GoapAction> queue = new Queue<GoapAction>();
        foreach (GoapAction action in result)
        {
            queue.Enqueue(action);
        }

        return queue;
    }

    /**
     * Return true if at least one solution founded
     * The possible paths are stored in leaves list. Each leaf has a
     * 'cost' value where the lowest cost will be the best action sequence
     */
    private bool BuildGraph(Node parent,
                                List<Node> leaves,
                                HashSet<GoapAction> usableActions,
                                HashSet<KeyValuePair<string, object>> goal)
    {
        bool foundOne = false;
        
        //go through each action available at this node and see if we can use it here
        foreach (GoapAction action in usableActions)
        {
            //if the parent state has the conditions for this action's preconditions, we can use it here
            if (InState(action.Preconditions, parent.state))
            {
                //apply the action's effects to the parent state
                HashSet<KeyValuePair<string, object>> currentState = PopulateState(parent.state, action.Effects);

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (InState(goal, currentState))
                {
                    //we reach the goal
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    HashSet<GoapAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundOne = true;
                    }
                }
            }
        }
        
        return foundOne;
    }

    /**
     * Create a subset of actions excluding removeMe. Create a new set
     */
    private HashSet<GoapAction> ActionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
    {
        HashSet<GoapAction> result = new HashSet<GoapAction>();
        foreach (var action in actions)
        {
            if (!action.Equals(removeMe))
            {
                result.Add(action);
            }
        }

        return result;
    }

    /**
     * Check that all items in 'test' matches items in 'state'
     * Return true if all matched
     * Otherwise - false
     */
    private bool InState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
    {
        bool allMatch = true;
        foreach (KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (t.Equals(s))
                {
                    match = true;
                }
            }

            if (!match)
            {
                allMatch = false;
                break;
            }
        }

        return allMatch;
    }
    
    /**
     * Apply the stateChange to currentState
     */
    private HashSet<KeyValuePair<string, object>> PopulateState(HashSet<KeyValuePair<string, object>> currentState,
                                                                    HashSet<KeyValuePair<string, object>> stateChange)
    {
        HashSet<KeyValuePair<string, object>> resultState = new HashSet<KeyValuePair<string, object>>();
        
        //Copy KVP's into result state
        foreach (KeyValuePair<string, object> kvp in currentState)
        {
            resultState.Add(new KeyValuePair<string, object>(kvp.Key, kvp.Value));
        }

        foreach (KeyValuePair<string, object> s in stateChange)
        {
            //if the key exist in current state - update value
            bool exists = false;

            foreach (KeyValuePair<string,object> r in resultState)
            {
                if (r.Equals(s))
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                resultState.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(s.Key);});
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(s.Key, s.Value);
                resultState.Add(updated);
            }
            else
            {
                resultState.Add(new KeyValuePair<string, object>(s.Key, s.Value));
            }
        }

        return resultState;
    }

    /**
     * Used for building up the graph and holding the running costs of actions 
     */
    private class Node
    {
        public Node parent;
        public float cost;
        public HashSet<KeyValuePair<string, object>> state;
        public GoapAction action;

        public Node(Node parent, float cost, HashSet<KeyValuePair<string, object>> state, GoapAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = state;
            this.action = action;
        }
    }
}

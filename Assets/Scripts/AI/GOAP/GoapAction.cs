using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class GoapAction : MonoBehaviour
{

    private HashSet<KeyValuePair<string, object>> preconditions;
    private HashSet<KeyValuePair<string, object>> effects;

    private bool inRange = false;

    /**
     * The cost of performing action
     * Figure out a weight that suits the action
     * Changing it will affect what actions are chosen during planning
     */
    public float cost = 1f;
    
    /**
     * An action often has to perform on object. This is that object. Can be null
     */
    public GameObject target;

    public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get => preconditions;
    }

    public HashSet<KeyValuePair<string, object>> Effects
    {
        get => effects;
    }

    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();
    }

    public void DoReset()
    {
        inRange = false;
        target = null;
        Reset();
    }

    /**
     * Reset any variables that need to be reset before next planning happens
     */
    protected abstract void Reset();

    /**
     * Is action done?
     */
    public abstract bool IsDone();

    /**
     * Procedurally check if this action can run. Not all actions
     * will need this, but some might
     */
    public abstract bool CheckProceduralPrecondition(GameObject agent);

    /**
     * Run the action.
     * Return True if action successfully performed
     * And return False if something happened and it can no longer perform.
     * In this case the action queue should clear out and the goal cannot be reached
     */
    public abstract bool Perform(GameObject agent);

    /**
     * Does this action need to be within range of target game object?
     * If not then the moveTo state will not need to run for this action
     */
    public abstract bool RequiresInRange();

    /**
     * Are we in range of a target?
     * The moveTo state will set this and it gets reset each time this action is performed
     */
    public bool IsInRange()
    {
        return inRange;
    }

    public void SetInRange(bool value)
    {
        this.inRange = value;
    }

    public void AddPrecondition(string key, object val)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, val));
    }

    public void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in preconditions)
        {
            if (kvp.Key.Equals(key))
            {
                remove = kvp;
            }
        }

        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            preconditions.Remove(remove);
        }
    }

    public void AddEffect(string key, object val)
    {
        effects.Add(new KeyValuePair<string, object>(key, val));
    }

    public void RemoveEffect(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            if (kvp.Key.Equals(key))
            {
                remove = kvp;
            }
        }

        if (!default(KeyValuePair<string, object>).Equals(remove))
        {
            effects.Remove(remove);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAction : GoapAction
{
    [Header("Wander Extension")]
    [SerializeField] private float      wanderDistance;

    [SerializeField] private float      timeoutDelay;

    private Vector3             wanderOffset;
    private NavMeshAgent        navMeshAgent;
    private AITargetingSystem   targetingSystem;
    /**
     * Can we set a new path for navmeshAgent?
     */
    private bool            isLocked;

    private void Start()
    {
        AddPrecondition("hasTarget", false);
        AddEffect("hasTarget", true);

        navMeshAgent = GetComponent<NavMeshAgent>();
        targetingSystem = GetComponent<AITargetingSystem>();
        wanderOffset = new Vector3(wanderDistance, 0f, 0f);
    }

    public WanderAction()
    {
        Debug.Log("Perform wander action");
    }
    
    protected override void Reset()
    {
        
    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }

    public override bool IsDone()
    {
        return targetingSystem.HasTarget;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return target != null;
    }

    public override bool Perform(GameObject agent)
    {
        //Wander
        if (navMeshAgent == null || isLocked)
        {
            return true;
        }
        else if (navMeshAgent != null && !isLocked)
        {
            StartCoroutine(Wander());
        }

        return true;
    }

    private IEnumerator Wander()
    {
        isLocked = true;
        Vector3 targetPos = (transform.position.x < target.transform.position.x)
            ? target.transform.position + wanderOffset
            : target.transform.position - wanderOffset;

        navMeshAgent.SetDestination(targetPos);
        
        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(timeoutDelay);
        isLocked = false;
        
        yield return null;
    }
    
    //OnTriggerEnter

    public override bool RequiresInRange()
    {
        //We must be in range of pivot
        return false;
    }
}

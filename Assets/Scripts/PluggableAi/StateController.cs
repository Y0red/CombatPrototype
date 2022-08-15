using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public CharacterStats enemyStats;

    public Transform eyes;

    public State currentState;
    public State remainState;


    public List<Transform> waypointsAll;
    public AttackDifination attack;
    public int nextWaypoint;

    [HideInInspector]public NavMeshAgent navMeshAgent;
  [HideInInspector]public Transform chaseTarget;
    [HideInInspector]public float stateTimeElapsed;
    public float aggroRange = 10;

    bool aiActive;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    public void SetuupAi(List<Transform> waypoints)
    {
        waypointsAll = waypoints;
        aiActive = true;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }
    private void Update()
    {
        currentState.UpdateState(this);
       
    }

    private void OnDrawGizmos()
    {
        if(currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGezmoColor;
            Gizmos.DrawWireSphere(eyes.position, aggroRange);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }
    
    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyPatrol))]
public class EnemyAI : MonoBehaviour
{
    private EnemyPatrol  _enemyPatrol;
    internal NavMeshAgent agent;
    
    private int _currentPatrolIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _enemyPatrol = GetComponent<EnemyPatrol>();
        
        _enemyPatrol.SetAgent(agent);
    }
    
    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    public EnemyState UpdateIdle(float distanceToTarget, float chaseRange, EnemyState state, NavMeshAgent agent)
    {
        if (distanceToTarget <= chaseRange)
        {
            state = EnemyState.Chase;
            if (agent != null)
            {
                agent.isStopped = false;
            }
        }
        return state;
    }

    public EnemyState UpdateChase(float distanceToTarget,float attackRange, float chaseRange, NavMeshAgent agent, EnemyState state, Transform target)
    {
        
        if (distanceToTarget <= attackRange)
        {
            state = EnemyState.Attack;
            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
        else if(distanceToTarget > chaseRange)
        {
            state = EnemyState.Idle;
            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
        else
        {
            if (agent != null && !agent.isStopped)
            {
                Debug.Log("иду");
                agent.SetDestination(target.position);
            }
            else if (agent != null && agent.isStopped)
            {
                Debug.Log("тоже иду");
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }
        
        return state;
    }

    public EnemyState UpdateAttack(float distanceToTarget,float attackRange, EnemyState state, NavMeshAgent agent)
    {
        Debug.Log("бьют");
        if (distanceToTarget > attackRange)
        {
            state = EnemyState.Chase;
            if (agent != null)
            {
                Debug.Log("снова иду");
                agent.isStopped = false;
            }
        }
        return state;
    }

    public EnemyState UpdateDead(EnemyState state)
    {
        state = EnemyState.Dead;
        return state;
    }
    
    public void SetPatrolPoints(List<Vector3> points)
    {
        
        _enemyPatrol.SetPoints(points);
    }
}

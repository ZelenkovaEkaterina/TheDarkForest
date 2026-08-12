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
    private EnemyPatrol _enemyPatrol;
    private NavMeshAgent _agent;
    protected EnemyState _currentState = EnemyState.Idle;
    protected bool _isInitialized = false;

    /*public NavMeshAgent Agent => _agent;
    public EnemyState CurrentState => _currentState;*/

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyPatrol = GetComponent<EnemyPatrol>();
        
        if (_enemyPatrol != null)
            _enemyPatrol.SetAgent(_agent);
            
        _isInitialized = true;
    }

    protected void Update()
    {
        if (!_isInitialized) return;
        
        // Если мы в состоянии Idle, обновляем патрулирование
        if (_currentState == EnemyState.Idle && _enemyPatrol != null)
        {
            // _enemyPatrol.UpdatePatrol();
        }
    }

    protected void UpdateIdle(float distanceToTarget, float chaseRange)
    {
        if (distanceToTarget <= chaseRange)
        {
            _currentState = EnemyState.Chase;
            ResumeMovement();
        }
    }

    protected void UpdateChase(float distanceToTarget, float attackRange, 
                                       float chaseRange, Transform target)
    {
        if (target == null)
        {
            _currentState = EnemyState.Idle;
            StopMovement();
            return;
        }

        if (distanceToTarget <= attackRange)
        {
            _currentState = EnemyState.Attack;
            StopMovement();
            Debug.Log("бьют");
        }
        else if (distanceToTarget > chaseRange)
        {
            _currentState = EnemyState.Idle;
            StopMovement();
        }
        else
        {
            ResumeMovement();
            _agent.SetDestination(target.position);
        }
    }

    protected void UpdateAttack(float distanceToTarget, float attackRange)
    {
        if (distanceToTarget > attackRange)
        {
            _currentState = EnemyState.Chase;
            ResumeMovement();
        }
    }

    protected void StopMovement()
    {
        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
    }

    protected void ResumeMovement()
    {
        if (_agent != null)
            _agent.isStopped = false;
    }

    public void SetPatrolPoints(Queue<Vector3> points)
    {
        if (_enemyPatrol != null)
            _enemyPatrol.SetPoints(points);
    }
}

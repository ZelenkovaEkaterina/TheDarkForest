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
[RequireComponent(typeof(EnemyHealthComponent))]
public class EnemyAI : MonoBehaviour
{
    private EnemyHealthComponent _enemyHealthComponent;
    private EnemyPatrol _enemyPatrol;
    private NavMeshAgent _agent;
    protected EnemyState _currentState;
    protected bool _isInitialized = false;

    

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyPatrol = GetComponent<EnemyPatrol>();
        _enemyHealthComponent = GetComponent<EnemyHealthComponent>();
        
        if (_enemyPatrol != null)
            _enemyPatrol.SetAgent(_agent);
            
        _isInitialized = true;
    }
    
    protected void Update()
    {
        if (!_isInitialized) return;
    }

    private void OnEnable()
    {
        _enemyHealthComponent.OnDeath += Death;
    }

    private void OnDisable()
    {
        _enemyHealthComponent.OnDeath -= Death;
    }

    private void Death()
    {
        //_currentState = EnemyState.Dead;
        //gameObject.SetActive(false);
        StopAllCoroutines();
    }

    protected void UpdateIdle(float distanceToTarget, float chaseRange)
    {
        if (distanceToTarget <= chaseRange)
        {
            _currentState = EnemyState.Chase;
            _enemyPatrol.IsActive = false;
            ResumeMovement();
        }
    }

    protected void UpdateChase(float distanceToTarget, float attackRange, 
                                       float chaseRange, Transform target)
    {
        if (target == null)
        {
            _currentState = EnemyState.Idle;
            _agent.speed = 3.5f;
            _enemyPatrol.IsActive = true;
            StopMovement();
            return;
        }

        if (distanceToTarget <= attackRange)
        {
            _currentState = EnemyState.Attack;
            _enemyPatrol.IsActive = false;
            StopMovement();
            
        }
        else if (distanceToTarget > chaseRange)
        {
            _currentState = EnemyState.Idle;
            _enemyPatrol.IsActive = true;
            StopMovement();
        }
        else
        {
            ResumeMovement();
            _agent.SetDestination(target.position);
            _agent.speed = 5f;
        }
    }

    protected void UpdateAttack(float distanceToTarget, float attackRange)
    {
        if (distanceToTarget > attackRange)
        {
            _currentState = EnemyState.Chase;
            _enemyPatrol.IsActive = false;
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

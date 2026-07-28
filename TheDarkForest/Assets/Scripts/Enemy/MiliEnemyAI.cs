using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(EnemyController))]
    [RequireComponent(typeof(EnemyAI))]
    
    public class MiliEnemyAI : MonoBehaviour
    {
       [Header("Settings")]
        public float chaseRange = 10f;
        public float attackRange = 2f;

        [Header("References")]
        private NavMeshAgent agent;
        private Transform target;
        private EnemyState state = EnemyState.Idle;
        private float attackTimer = 0f;
        private bool hasTarget = false;
        

        private EnemyController _enemyController;
        private EnemyAI _enemyAI;
        

        protected void Awake()
        {
            //agent = _enemyAI.agent;
            _enemyController = GetComponent<EnemyController>();
            _enemyAI = GetComponent<EnemyAI>();
        }

        private void Start()
        {
            agent = _enemyAI.agent;
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _enemyController.OnChase += HandleChase;
        }

        private void OnDisable()
        {
            _enemyController.OnChase -= HandleChase;
        }
        
        private void FixedUpdate()
        {
            if (!hasTarget || target == null || !target.gameObject.activeSelf)
            {
                state = EnemyState.Idle;
                //if (agent != null) agent.isStopped = true;
                return;
            }
            
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Debug.DrawLine(transform.position, target.position, Color.red);
            
            switch (state)
            {
                case EnemyState.Idle:
                    state = _enemyAI.UpdateIdle(distanceToTarget, chaseRange, state, agent);
                    break;

                case EnemyState.Chase:
                    state = _enemyAI.UpdateChase(distanceToTarget, attackRange, chaseRange, agent, state, target);
                    break;

                case EnemyState.Attack:
                    state = _enemyAI.UpdateAttack(distanceToTarget, attackRange, state, agent);
                    break;
            }
        }

        private void HandleChase(Transform t)
        {
            target = t;
            hasTarget = true;
            state = EnemyState.Chase;
            
            if (agent != null)
            {
                agent.isStopped = false;
            }
        }

        
    }
}
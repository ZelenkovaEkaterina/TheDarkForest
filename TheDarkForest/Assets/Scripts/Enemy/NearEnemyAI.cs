using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyController))]
    public class NearEnemyAI : MonoBehaviour
    {
       [Header("Settings")]
        public float chaseRange = 15f;
        public float attackRange = 2f;
        public float updateInterval = 0.1f;

        [Header("References")]
        protected NavMeshAgent agent;
        protected Transform target;
        protected EnemyState state = EnemyState.Idle;
        protected float attackTimer = 0f;
        protected bool hasTarget = false;

        private Coroutine _aiCoroutine;

        private EnemyController _enemyController;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _enemyController = GetComponent<EnemyController>();
        }

        private void Start()
        {
            if (_aiCoroutine == null)
                StartAI();
        }

        private void OnEnable()
        {
            _enemyController.OnChase += HandleChase;
            StartAI();
        }

        private void OnDisable()
        {
            _enemyController.OnChase -= HandleChase;
            StopAI();
        }

        private void StartAI()
        {
            if (_aiCoroutine != null)
                StopCoroutine(_aiCoroutine);
            
            _aiCoroutine = StartCoroutine(StateUpdate());
        }

        private void StopAI()
        {
            if (_aiCoroutine != null)
            {
                StopCoroutine(_aiCoroutine);
                _aiCoroutine = null;
            }
        }

        private IEnumerator StateUpdate()
        {
            var wait = new WaitForSeconds(updateInterval);

            while (enabled && gameObject.activeInHierarchy)
            {
                if (!hasTarget || target == null || !target.gameObject.activeSelf)
                {
                    state = EnemyState.Idle;
                    if (agent != null) agent.isStopped = true;
                    yield return wait;
                    continue;
                }

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                switch (state)
                {
                    case EnemyState.Idle:
                        UpdateIdle(distanceToTarget);
                        break;

                    case EnemyState.Chase:
                        UpdateChase(distanceToTarget);
                        break;

                    case EnemyState.Attack:
                        UpdateAttack(distanceToTarget);
                        break;
                }

                yield return wait;
            }

            _aiCoroutine = null;
        }

        protected virtual void UpdateIdle(float distanceToTarget)
        {
            if (distanceToTarget <= chaseRange)
            {
                state = EnemyState.Chase;
                if (agent != null)
                {
                    agent.isStopped = false;
                }
                Debug.Log("иду");
            }
        }

        protected virtual void UpdateChase(float distanceToTarget)
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
            else
            {
                if (agent != null && !agent.isStopped)
                {
                    agent.SetDestination(target.position);
                }
                else if (agent != null && agent.isStopped)
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                }
            }
        }

        protected virtual void UpdateAttack(float distanceToTarget)
        {
            if (distanceToTarget > attackRange)
            {
                state = EnemyState.Chase;
                if (agent != null)
                {
                    agent.isStopped = false;
                }
            }

            Debug.Log("бьют");
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
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

    public class EnemyAI : MonoBehaviour
    {
        [Header("Settings")] public float chaseRange = 15f;
        public float attackRange = 2f;

        [Header("References")] protected NavMeshAgent agent;
        protected Transform target;
        protected EnemyState state = EnemyState.Idle;

        protected float attackTimer = 0f;
        protected bool hasTarget = false;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                //Debug.LogError($"[Mob] На объекте {gameObject.name} отсутствует компонент NavMeshAgent!");
            }
        }

        public void SetTarget(Transform t)
        {
            target = t;
            hasTarget = true;
        }

        public void StartChase()
        {
            if (!hasTarget || target == null) return;
            state = EnemyState.Chase;

            if (agent != null)
            {
                agent.isStopped = false;
            }
        }

        public void ResetState()
        {
            target = null;
            hasTarget = false;
            state = EnemyState.Idle;
            attackTimer = 0f;

            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }

        private void Update()
        {
            if (!hasTarget || target == null || !target.gameObject.activeSelf)
            {
                state = EnemyState.Idle;
                if (agent != null) agent.isStopped = true;
                return;
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

            //Debug.Log("бьют");
        }

    }
}
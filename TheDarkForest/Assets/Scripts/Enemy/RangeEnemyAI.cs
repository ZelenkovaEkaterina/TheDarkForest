using Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyController))]
    public class RangeEnemyAI : NearEnemyAI
    {
        [Header("Ranged Settings")] 
        public float stopRange = 5f; //дистанция, на которой моб останавливается
        public float retreatRange = 3f;//дистанция, на которой моб отступает от игрока
        public float retreatSpeed = 0.8f;//скорость отступления
        public float retreatDistance = 3f;//дистанция отступления

        private float originalSpeed;

        protected override void Awake()
        {
            base.Awake();
            originalSpeed = agent.speed;
        }

        protected override void UpdateChase(float distanceToTarget)
        {
            if (distanceToTarget <= retreatRange)
            {
                if (agent != null)
                {
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }
                    
                    agent.speed = originalSpeed * retreatSpeed;
                    
                    Vector3 directionAway = (transform.position - target.position).normalized;
                    Vector3 retreatPoint = transform.position + directionAway * retreatDistance;

                    // проверяем, что точка на NavMesh
                    if (NavMesh.SamplePosition(retreatPoint, out NavMeshHit hit, retreatDistance * 2f,
                            NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                        Debug.Log(
                            $"{gameObject.name} отступает к {hit.position}, дистанция до игрока: {distanceToTarget}");
                    }
                    else
                    {
                        // если точка недоступна, отступаем в случайном направлении
                        Vector3 randomDirection = Random.insideUnitSphere * retreatDistance;
                        randomDirection += transform.position;

                        if (NavMesh.SamplePosition(randomDirection, out hit, retreatDistance * 2f, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                            Debug.Log($"{gameObject.name} отступает случайно к {hit.position}");
                        }
                    }
                }
                
                if (state == EnemyState.Attack)
                {
                    state = EnemyState.Chase;
                }

                return;
            }
            
            if (agent.speed != originalSpeed)
            {
                agent.speed = originalSpeed;
            }
            
            if (distanceToTarget <= stopRange)
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }

                if (state != EnemyState.Attack)
                {
                    state = EnemyState.Attack;
                    Debug.Log($"{gameObject.name} остановился для стрельбы, дистанция: {distanceToTarget}");
                }

                return;
            }
            
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }

            agent.SetDestination(target.position);
            Debug.Log($"{gameObject.name} преследует игрока, дистанция: {distanceToTarget}");
        }

        protected override void UpdateAttack(float distanceToTarget)
        {
            if (distanceToTarget <= retreatRange)
            {
                state = EnemyState.Chase;
                return;
            }
            
            if (distanceToTarget > stopRange)
            {
                state = EnemyState.Chase;
                if (agent != null)
                {
                    agent.isStopped = false;
                }

                return;
            }
            
            PerformRangedAttack();
        }

        private void PerformRangedAttack()
        {
            // Логика ranged атаки
            Debug.Log($"{gameObject.name} стреляет в {target.name}!");
        }
    }
}
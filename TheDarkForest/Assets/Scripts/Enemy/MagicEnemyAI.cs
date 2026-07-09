using Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    /// <summary>
    /// Моб, который останавливается на определённой дистанции (например, лучник)
    /// </summary>
    public class MagicEnemyAI : EnemyAI
    {
        [Header("Ranged Settings")] [Tooltip("Дистанция, на которой моб останавливается")]
        public float stopRange = 8f;

        [Tooltip("Дистанция, на которой моб начинает отступать, если игрок слишком близко")]
        public float retreatRange = 3f;

        [Tooltip("Скорость отступления (множитель скорости)")]
        public float retreatSpeedMultiplier = 0.8f;

        [Tooltip("Как далеко отступать")] public float retreatDistance = 5f;

        private float originalSpeed;

        protected override void Awake()
        {
            base.Awake();
            if (agent != null)
            {
                originalSpeed = agent.speed;
            }
        }

        protected override void UpdateChase(float distanceToTarget)
        {
            // ===== 1. ПРОВЕРКА НА ОТСТУПЛЕНИЕ =====
            if (distanceToTarget <= retreatRange)
            {
                // Включаем агента, если он остановлен
                if (agent != null)
                {
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }

                    // Увеличиваем скорость отступления (опционально)
                    agent.speed = originalSpeed * retreatSpeedMultiplier;

                    // Вычисляем точку для отступления
                    Vector3 directionAway = (transform.position - target.position).normalized;
                    Vector3 retreatPoint = transform.position + directionAway * retreatDistance;

                    // Проверяем, что точка на NavMesh
                    if (NavMesh.SamplePosition(retreatPoint, out NavMeshHit hit, retreatDistance * 2f,
                            NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                        Debug.Log(
                            $"{gameObject.name} отступает к {hit.position}, дистанция до игрока: {distanceToTarget:F2}");
                    }
                    else
                    {
                        // Если точка недоступна, отступаем в случайном направлении
                        Vector3 randomDirection = Random.insideUnitSphere * retreatDistance;
                        randomDirection += transform.position;

                        if (NavMesh.SamplePosition(randomDirection, out hit, retreatDistance * 2f, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                            Debug.Log($"{gameObject.name} отступает случайно к {hit.position}");
                        }
                    }
                }

                // Если мы в состоянии атаки, возвращаемся в Chase
                if (state == EnemyState.Attack)
                {
                    state = EnemyState.Chase;
                }

                return; // Выходим, чтобы не проверять другие условия
            }

            // ===== 2. ВОССТАНАВЛИВАЕМ СКОРОСТЬ (если отступали) =====
            if (agent != null && agent.speed != originalSpeed)
            {
                agent.speed = originalSpeed;
            }

            // ===== 3. ПРОВЕРКА НА ОСТАНОВКУ (дистанция стрельбы) =====
            if (distanceToTarget <= stopRange)
            {
                if (agent != null && !agent.isStopped)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }

                if (state != EnemyState.Attack)
                {
                    state = EnemyState.Attack;
                    Debug.Log($"{gameObject.name} остановился для стрельбы, дистанция: {distanceToTarget:F2}");
                }

                return;
            }

            // ===== 4. ДВИЖЕНИЕ К ЦЕЛИ (если игрок далеко) =====
            if (agent != null)
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }

                agent.SetDestination(target.position);
                Debug.Log($"{gameObject.name} преследует игрока, дистанция: {distanceToTarget:F2}");
            }
        }

        protected override void UpdateAttack(float distanceToTarget)
        {
            // ===== 1. Если игрок слишком близко - отступаем =====
            if (distanceToTarget <= retreatRange)
            {
                state = EnemyState.Chase;
                // В следующем кадре сработает UpdateChase() и начнёт отступление
                return;
            }

            // ===== 2. Если игрок убежал дальше stopRange - догоняем =====
            if (distanceToTarget > stopRange)
            {
                state = EnemyState.Chase;
                if (agent != null)
                {
                    agent.isStopped = false;
                }

                return;
            }

            // ===== 3. Иначе стоим и атакуем =====
            // Здесь логика атаки
            PerformRangedAttack();
        }

        private void PerformRangedAttack()
        {
            // Логика ranged атаки
            Debug.Log($"{gameObject.name} стреляет в {target.name}!");
        }

        /*// Опционально: визуализация диапазонов в Scene View
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stopRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, retreatRange);
        }*/
    }
}
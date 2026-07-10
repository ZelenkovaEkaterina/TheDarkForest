using Enemy;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Моб, который патрулирует между точками и преследует игрока при обнаружении
/// </summary>
public class PatrolMob : EnemyAI
{
    [Header("Patrol Settings")]
    [Tooltip("Точки патруля (если не заданы, использует случайные точки в зоне)")]
    public Transform[] patrolPoints;
    
    [Tooltip("Радиус для случайного патруля (если points не заданы)")]
    public float patrolRadius = 6f;
    
    [Tooltip("Время ожидания в точке (сек)")]
    public float waitTimeAtPoint = 2f;
    
    [Tooltip("Скорость патруля (множитель от базовой скорости)")]
    public float patrolSpeedMultiplier = 0.6f;
    
    [Tooltip("Дистанция, на которой моб замечает игрока во время патруля")]
    public float detectionRange = 4f;
    
    [Header("Return Settings")]
    [Tooltip("Возвращаться ли к патрулю после потери игрока")]
    public bool returnToPatrol = true;
    
    [Tooltip("Дистанция, на которой моб теряет интерес к игроку")]
    public float lostInterestRange = 10f;

    // Приватные переменные для патруля
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private Vector3 lastPatrolPoint;
    private float originalSpeed;
    private bool isReturningToPatrol = false;

    protected override void Awake()
    {
        base.Awake();
        
        if (agent != null)
        {
            originalSpeed = agent.speed;
            agent.speed = originalSpeed * patrolSpeedMultiplier;
        }
        
        // Если точки не заданы, создаем случайные
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolPoints = GenerateRandomPatrolPoints(4);
        }
        
        // Начинаем патруль
        StartPatrol();
    }

    protected override void Update()
    {
        // Если игрок рядом во время патруля - переключаемся на Chase
        if (state == EnemyState.Idle && hasTarget && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            if (distanceToTarget <= detectionRange)
            {
                state = EnemyState.Chase;
                if (agent != null)
                {
                    agent.speed = originalSpeed; // Возвращаем полную скорость
                    agent.isStopped = false;
                }
                isWaiting = false;
                Debug.Log($"{gameObject.name}: Обнаружил игрока! Переход в режим преследования.");
                return;
            }
        }
        
        
        base.Update();
    }

    protected override void UpdateIdle(float distanceToTarget)
    {
        // В состоянии Idle - патрулируем
        UpdatePatrol();
        
        // Проверяем, не заметил ли игрока
        if (hasTarget && target != null && distanceToTarget <= detectionRange)
        {
            state = EnemyState.Chase;
            if (agent != null)
            {
                agent.speed = originalSpeed;
                agent.isStopped = false;
            }
            isWaiting = false;
        }
    }

    protected override void UpdateChase(float distanceToTarget)
    {
        // Если игрок убежал слишком далеко - теряем интерес
        if (distanceToTarget > lostInterestRange)
        {
            if (returnToPatrol)
            {
                Debug.Log($"{gameObject.name}: Потерял игрока. Возвращаюсь к патрулю.");
                ReturnToPatrol();
            }
            else
            {
                // Просто останавливаемся
                state = EnemyState.Idle;
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }
            }
            return;
        }

        // Если игрок в зоне атаки - атакуем
        if (distanceToTarget <= attackRange)
        {
            state = EnemyState.Attack;
            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            return;
        }

        // Преследуем игрока
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = originalSpeed;
            agent.SetDestination(target.position);
        }
    }

    protected override void UpdateAttack(float distanceToTarget)
    {
        // Если игрок убежал из зоны атаки - продолжаем преследовать
        if (distanceToTarget > attackRange)
        {
            state = EnemyState.Chase;
            if (agent != null)
            {
                agent.isStopped = false;
            }
            return;
        }
        
        // Если игрок убежал слишком далеко - теряем интерес
        if (distanceToTarget > lostInterestRange)
        {
            if (returnToPatrol)
            {
                ReturnToPatrol();
            }
            else
            {
                state = EnemyState.Idle;
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }
            }
            return;
        }

        // Здесь ваша логика атаки
        PerformAttack();
    }

    // ===== ЛОГИКА ПАТРУЛЯ =====

    private void StartPatrol()
    {
        state = EnemyState.Idle;
        isWaiting = false;
        waitTimer = 0f;
        
        if (agent != null)
        {
            agent.speed = originalSpeed * patrolSpeedMultiplier;
            agent.isStopped = false;
        }
        
        // Идем к первой точке
        MoveToNextPatrolPoint();
    }

    private void UpdatePatrol()
    {
        if (agent == null || patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Если ждем в точке
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                MoveToNextPatrolPoint();
            }
            return;
        }

        // Если достигли точки или путь завершен
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
                agent.isStopped = true;
                Debug.Log($"{gameObject.name}: Достиг точки патруля, жду {waitTimeAtPoint} сек.");
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Выбираем следующую точку
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        
        if (targetPoint == null)
        {
            Debug.LogWarning($"{gameObject.name}: Точка патруля {currentPatrolIndex} не назначена!");
            return;
        }

        lastPatrolPoint = targetPoint.position;
        
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPoint.position);
            Debug.Log($"{gameObject.name}: Иду к точке патруля {currentPatrolIndex}: {targetPoint.position}");
        }
    }

    private void ReturnToPatrol()
    {
        state = EnemyState.Idle;
        isReturningToPatrol = true;
        
        if (agent != null)
        {
            agent.speed = originalSpeed * patrolSpeedMultiplier;
            agent.isStopped = false;
            
            // Возвращаемся к последней точке патруля
            agent.SetDestination(lastPatrolPoint);
        }
        
        Debug.Log($"{gameObject.name}: Возвращаюсь к патрулю.");
    }

    // ===== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====

    /// <summary>
    /// Генерирует случайные точки патруля
    /// </summary>
    private Transform[] GenerateRandomPatrolPoints(int count)
    {
        Transform[] points = new Transform[count];
        
        for (int i = 0; i < count; i++)
        {
            GameObject pointObj = new GameObject($"PatrolPoint_{i}_{gameObject.name}");
            pointObj.transform.parent = transform.parent;
            
            // Генерируем случайную точку
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randomPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Находим точку на NavMesh
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                pointObj.transform.position = hit.position;
            }
            else
            {
                pointObj.transform.position = transform.position;
            }
            
            points[i] = pointObj.transform;
        }
        
        return points;
    }

    /// <summary>
    /// Сброс патруля (вызывается при возврате в пул)
    /// </summary>
    public void ResetPatrol()
    {
        isWaiting = false;
        waitTimer = 0f;
        isReturningToPatrol = false;
        StartPatrol();
    }

    // ===== ЛОГИКА АТАКИ =====

    private void PerformAttack()
    {
        // Ваша логика атаки
        Debug.Log($"{gameObject.name}: Атакует {target?.name}!");
        
        // Пример: нанесение урона через интерфейс
        if (target != null)
        {
            /*var damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10f);
            }*/
        }
    }

    // ===== ВИЗУАЛИЗАЦИЯ =====

    /*private void OnDrawGizmosSelected()
    {
        // Зона обнаружения
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Зона потери интереса
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, lostInterestRange);
        
        // Точки патруля
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            foreach (var point in patrolPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.5f);
                }
            }
            
            // Соединяем точки линиями
            if (patrolPoints.Length > 1)
            {
                Gizmos.color = new Color(0, 0.5f, 1, 0.5f);
                for (int i = 0; i < patrolPoints.Length - 1; i++)
                {
                    if (patrolPoints[i] != null && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
                // Замыкаем круг
                if (patrolPoints.Length > 2)
                {
                    Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
                }
            }
        }
    }*/
}
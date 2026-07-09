using UnityEngine;
using UnityEngine.AI;

public enum MobState
{
    Idle,
    Chase,
    Attack,
    Dead
}

public class Mob : MonoBehaviour
{
    [Header("Settings")]
    public float chaseRange = 15f;
    public float attackRange = 2f;

    [Header("References")]
    private NavMeshAgent agent;
    private Transform target;
    private MobState state = MobState.Idle;

    private float attackTimer = 0f;
    private bool hasTarget = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"[Mob] На объекте {gameObject.name} отсутствует компонент NavMeshAgent!");
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
        state = MobState.Chase;
        
        // ВКЛЮЧАЕМ движение, когда начинаем преследование
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    public void ResetState()
    {
        target = null;
        hasTarget = false;
        state = MobState.Idle;
        attackTimer = 0f;

        if (agent != null)
        {
            agent.isStopped = true;  // Останавливаем
            agent.ResetPath();
        }
    }

    private void Update()
    {
        if (!hasTarget || target == null || !target.gameObject.activeSelf)
        {
            state = MobState.Idle;
            if (agent != null) agent.isStopped = true;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (state)
        {
            case MobState.Idle:
                if (distanceToTarget <= chaseRange)
                {
                    state = MobState.Chase;
                    
                    // ВКЛЮЧАЕМ движение при переходе из Idle в Chase
                    if (agent != null)
                    {
                        agent.isStopped = false;
                    }
                }
                break;

            case MobState.Chase:
                if (distanceToTarget <= attackRange)
                {
                    state = MobState.Attack;
                    if (agent != null) 
                    {
                        agent.isStopped = true;  // Останавливаем для атаки
                        agent.ResetPath();
                    }
                }
                else
                {
                    //  Проверяем, что агент включен, и только тогда двигаемся
                    if (agent != null && !agent.isStopped)
                    {
                        agent.SetDestination(target.position);
                    }
                    else if (agent != null && agent.isStopped)
                    {
                        // Если вдруг stopped = true, включаем обратно
                        agent.isStopped = false;
                        agent.SetDestination(target.position);
                    }
                }
                break;

            case MobState.Attack:
                if (distanceToTarget > attackRange)
                {
                    state = MobState.Chase;
                    if (agent != null)
                    {
                        
                        agent.isStopped = false;  //  Включаем обратно
                    }
                }
                Debug.Log("бьют");
                break;
        }
    }
}
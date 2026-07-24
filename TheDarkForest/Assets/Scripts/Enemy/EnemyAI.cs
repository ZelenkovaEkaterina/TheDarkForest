using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead
}
public class EnemyAI : MonoBehaviour
{
    public void UpdateIdle(float distanceToTarget, float chaseRange, EnemyState state, NavMeshAgent agent)
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

    public void UpdateChase(float distanceToTarget,float attackRange, NavMeshAgent agent, EnemyState state, Transform target)
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

    public void UpdateAttack(float distanceToTarget,float attackRange, EnemyState state, NavMeshAgent agent)
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
}

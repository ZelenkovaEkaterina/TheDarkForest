using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class MeleeEnemyAnimationsController : MonoBehaviour
    {
        private EnemyPatrol _enemyPatrol;
        private EnemyCombatAI _enemyCombat;
        private Animator _animator;
        private MeleeEnemyAI _meleeEnemyAI;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemyPatrol = GetComponent<EnemyPatrol>();
            _enemyCombat = GetComponent<EnemyCombatAI>();
            _meleeEnemyAI = GetComponent<MeleeEnemyAI>();
        }

        private void Update()
        {
            UpdateAnim();
        }

        private void UpdateAnim()
        {
            switch (_meleeEnemyAI.CurrentState)
            {
                case EnemyState.Idle:
                    if (gameObject.GetComponent<NavMeshAgent>().isStopped == false)
                    {
                        _animator.SetBool("Patrol", true);
                        return;
                    }
                    _animator.SetBool("Patrol", false);
                    _animator.SetBool("Chase", false);
                    _animator.SetBool("Attack", false);
                    break;
                case EnemyState.Chase:
                    _animator.SetBool("Chase", true);
                    break;
                case EnemyState.Attack:
                    _animator.SetBool("Attack",true);
                    break;
            }
            
        }

    }
}

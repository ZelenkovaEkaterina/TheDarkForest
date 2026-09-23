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
            Debug.Log(_meleeEnemyAI.CurrentState);
        }

        private void UpdateAnim()
        {
            _animator.SetBool("Patrol", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Chase", false);
            
            switch (_meleeEnemyAI.CurrentState)
            {
                case EnemyState.Idle:
                    if (gameObject.GetComponent<NavMeshAgent>().isStopped == false)
                    {
                        _animator.SetBool("Patrol", true);
                        //Debug.Log(gameObject.GetComponent<NavMeshAgent>().isStopped);
                    }
                    else
                    {
                        _animator.SetBool("Patrol", false);
                        //Debug.Log(gameObject.GetComponent<NavMeshAgent>().isStopped);
                    }
                    
                    
                    //_animator.SetBool("Attack", false);
                    break;
                case EnemyState.Chase:
                    _animator.SetBool("Chase", true);
                    break;
                case EnemyState.Attack:
                    _animator.SetBool("Attack", true);
                    break;
            }
            
        }

    }
}

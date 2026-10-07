using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class RangeAnimationsController : MeleeEnemyAnimationsController
    {
        private RangeEnemyAI _rangeEnemyAI;

        protected override void Start()
        {
            base.Start();
            _rangeEnemyAI = GetComponent<RangeEnemyAI>();
        }

        private void Update()
        {
            UpdateAnim();
        }

        private void UpdateAnim()
        {
            _animator.SetBool("Patrol", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Chase", false);
            
            switch (_rangeEnemyAI.CurrentState)
            {
                case EnemyState.Idle:
                    Debug.Log(_enemyPatrol.Agent.isStopped);
                    if (gameObject.GetComponent<NavMeshAgent>().isStopped == false)
                    {
                        _animator.SetBool("Patrol", true);
                        //Debug.Log(gameObject.GetComponent<NavMeshAgent>().isStopped);
                    }
                    else
                    {
                        _animator.SetBool("Patrol", false);
                        
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

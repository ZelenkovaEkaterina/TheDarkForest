using System;
using System.Collections;
using UnityEngine;

public class EnemyCombatAI : EnemyAI
{
        public event Action<EnemyState> OnState; 
        [SerializeField] protected float chaseRange = 10f;
        [SerializeField] protected float attackRange = 2f;
        
        [SerializeField] private float attackCooldown = 1.5f;
        private bool _canAttack = true;

        private Transform _target;
        protected Transform Target => _target;
        private bool _hasTarget = false;
        private EnemyController _enemyController;

        protected override void Awake()
        {
            base.Awake();
            _enemyController = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            if (_enemyController != null)
            {
                _enemyController.OnChase += HandleChase;
                _enemyController.OnTargetDead += HandleTargetDie;
            }
        }

        private void OnDisable()
        {
            if (_enemyController != null)
            {
                _enemyController.OnChase -= HandleChase;
                _enemyController.OnTargetDead -= HandleTargetDie;
            }
        }

        private void FixedUpdate()
        {
            
            if (!_isInitialized) return;
            
            if (!_hasTarget || _target == null || !_target.gameObject.activeSelf)
            {
                if (_currentState != EnemyState.Idle)
                {
                    _currentState = EnemyState.Idle;
                    StopMovement();
                }
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, _target.position);
            
            switch (_currentState)
            {
                case EnemyState.Idle:
                    UpdateIdle(distanceToTarget, chaseRange);
                    GetState(_currentState);
                    break;

                case EnemyState.Chase:
                    UpdateChase(distanceToTarget, attackRange, chaseRange, _target);
                    GetState(_currentState);
                    break;

                case EnemyState.Attack:
                    UpdateAttack(distanceToTarget, attackRange);
                    GetState(_currentState);
                    if (_canAttack)
                    {
                        PerformAttack();
                        StartCoroutine(AttackCooldownRoutine());
                    }
                    break;
            }
        }

        private void HandleChase(Transform target)
        {
            _target = target;
            _hasTarget = true;
            _currentState = EnemyState.Chase;
            ResumeMovement();
            _canAttack = true;
            StopAllCoroutines();
        }

        private void HandleTargetDie()
        {
            _target = null;
        }

        protected virtual void PerformAttack()
        {
            if (_target == null) return;

            Vector3 direction = (_target.position - transform.position).normalized;

            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        private IEnumerator AttackCooldownRoutine()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }

        public void GetState(EnemyState ss)
        {
            OnState?.Invoke(ss);
        }
}

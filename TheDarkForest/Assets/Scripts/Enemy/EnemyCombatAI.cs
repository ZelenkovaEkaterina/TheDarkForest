using System.Collections;
using UnityEngine;

public class EnemyCombatAI : EnemyAI
{
    [Header("Settings")]
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
                _enemyController.OnChase += HandleChase;
        }

        private void OnDisable()
        {
            if (_enemyController != null)
                _enemyController.OnChase -= HandleChase;
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
            Debug.DrawLine(transform.position, _target.position, Color.red);
            
            switch (_currentState)
            {
                case EnemyState.Idle:
                    UpdateIdle(distanceToTarget, chaseRange);
                    break;

                case EnemyState.Chase:
                    UpdateChase(distanceToTarget, attackRange, chaseRange, _target);
                    break;

                case EnemyState.Attack:
                    UpdateAttack(distanceToTarget, attackRange);
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
            _canAttack =  true;
            StopAllCoroutines();
        }

        protected virtual void PerformAttack(){}

        private IEnumerator AttackCooldownRoutine()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
}

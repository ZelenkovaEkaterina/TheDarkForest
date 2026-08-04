using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(EnemyController))]
   
    public class RangeEnemyAI : EnemyAI
    {
       [Header("Settings")]
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 6f;

        private Transform _target;
        private bool _hasTarget = false;
        private EnemyController _enemyController;

        protected override void Awake()
        {
            base.Awake();
            _enemyController = GetComponent<EnemyController>();
            
            if (_enemyController == null)
                Debug.LogError($"EnemyController не найден на {gameObject.name}");
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

            // Проверяем цель
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

            // Обновляем состояние
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
                    break;
            }
        }

        private void HandleChase(Transform target)
        {
            _target = target;
            _hasTarget = true;
            _currentState = EnemyState.Chase;
            ResumeMovement();
        }
    }
}
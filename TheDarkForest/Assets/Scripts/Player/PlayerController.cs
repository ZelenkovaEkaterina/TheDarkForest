using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Attack,
        Run,
        Dead
    }
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovementComponent _movementComponent;
        private PlayerHealthComponent _healthComponent;
        private PlayerState _currentState;
        private PlayerState _previousState;
        
        public event Action<bool> OnFire;
        public event Action<bool> OnRun;
        public event Action<bool> OnDeath;

        private void Awake()
        {
            _movementComponent = GetComponent<PlayerMovementComponent>();
            _healthComponent = GetComponent<PlayerHealthComponent>();
        }

        private void OnEnable()
        {
            _healthComponent.OnDeathEvent += SetDeath;
        }

        private void OnDisable()
        {
            _healthComponent.OnDeathEvent -= SetDeath;
        }

        private void SetDeath(bool obj)
        {
            OnDeath?.Invoke(true);
        }

        private void FixedUpdate()
        {
            if (_movementComponent == null) return;

            _currentState = _movementComponent.State;

            if (_currentState != _previousState)
            {
                if (_previousState == PlayerState.Attack) OnFire?.Invoke(false);
                if (_previousState == PlayerState.Run) OnRun?.Invoke(false);
                
                if (_currentState == PlayerState.Attack) OnFire?.Invoke(true);
                if (_currentState == PlayerState.Run) OnRun?.Invoke(true);

                _previousState = _currentState;
            }
        }
    }
}


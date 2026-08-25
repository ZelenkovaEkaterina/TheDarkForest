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
        private PlayerState _currentState;
        private PlayerState _previousState;
        
        public event Action<bool> OnFire;
        public event Action<bool> OnRun;

        private void Awake()
        {
            _movementComponent = GetComponent<PlayerMovementComponent>();
        }

        private void FixedUpdate()
        {
            Debug.Log(_currentState);
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
            
            if (_currentState == PlayerState.Dead)
            {
            }
        }
    }
}


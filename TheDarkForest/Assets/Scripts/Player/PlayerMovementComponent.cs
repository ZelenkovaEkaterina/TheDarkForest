using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        public event Action<PlayerState> OnStateChanged;
        
        [SerializeField] private float _rotateSpeed = 12f;
        [SerializeField] private float _arriveTolerance = 0.15f;

        private NavMeshAgent _agent;
        private Transform _moveTarget;

        public PlayerState State => _state;
        private PlayerState _state = PlayerState.Idle;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateRotation();
            
            if (_state == PlayerState.Attack ||
                _state == PlayerState.Interact ||
                _state == PlayerState.Dead) return;

            SetState(IsMoving ? PlayerState.Run : PlayerState.Idle);
        }

        public bool IsMoving =>
            _agent.hasPath && !_agent.pathPending &&
            _agent.remainingDistance > _agent.stoppingDistance + _arriveTolerance;
        
        

        public void MoveTo(Vector3 point)
        {
            if (State == PlayerState.Dead) return;
            
            _moveTarget = null;
            _agent.stoppingDistance = 0f;
            _agent.isStopped = false;
            _agent.SetDestination(point);
        }

        public void MoveToTarget(Transform target, float stopDistance)
        {
            if (State == PlayerState.Dead) return;
            
            if (target == null) return;
            _moveTarget = target;
            _agent.stoppingDistance = stopDistance;
            _agent.isStopped = false;
            _agent.SetDestination(target.position);
        }

        public void Stop()
        {
            _moveTarget = null;
            if (_agent.isOnNavMesh) _agent.ResetPath();
            _agent.isStopped = true;
        }

        private void UpdateRotation()
        {
            Vector3 dir = _agent.desiredVelocity;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f) return;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                _rotateSpeed * Time.deltaTime);
        }

        private void UpdateState()
        {
            if (_state == PlayerState.Attack ||
                _state == PlayerState.Interact ||
                _state == PlayerState.Dead) return;
            
            bool moving = IsMoving || _agent.pathPending;
            SetState(moving ? PlayerState.Run : PlayerState.Idle);
        }
        
        public void SetState(PlayerState next)
        {
            if (_state == next) return;
            _state = next;
            OnStateChanged?.Invoke(_state);
        }
    }
}

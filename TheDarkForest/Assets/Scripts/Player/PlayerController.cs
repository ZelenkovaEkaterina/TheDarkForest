using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Interact,
        Attack,
        Run,
        Dead
    }
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovementComponent _movement;
        private PlayerHealthComponent _health;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
            _health = GetComponent<PlayerHealthComponent>();
        }

        private void OnEnable()
        {
            _health.OnDeathEvent += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnDeathEvent -= HandleDeath;
        }

        private void HandleDeath(bool _)
        {
            _movement.SetState(PlayerState.Dead);
        }
    }
}


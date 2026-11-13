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
            _health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= HandleDeath;
        }

        private void HandleDeath(HealthSystem s)
        {
            _movement.SetState(PlayerState.Dead);
        }
    }
}


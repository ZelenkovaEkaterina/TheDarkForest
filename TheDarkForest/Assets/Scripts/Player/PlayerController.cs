using System;
using UnityEngine;
using UnityEngine.AI;



namespace Player
{
    public enum PlayerState
    {
        Idle,
        Attack,
        Dead
    }
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private PlayerMovementComponent playerMovementComponent;
        private PlayerState state = PlayerState.Idle;

        private void Awake()
        {
            _agent = playerMovementComponent.agent;
        }

        /*private void FixedUpdate()
        {
            switch (state)
            {
                case PlayerState.Idle:
                    _agent.isStopped = false;
                    break;

                case PlayerState.Attack:
                    _agent.isStopped = false;
                    break;
            }
        }*/
    }
}


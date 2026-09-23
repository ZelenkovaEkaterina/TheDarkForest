using System;
using Player;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementComponent _movement;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_movement == null) _movement = GetComponent<PlayerMovementComponent>();
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _movement.OnStateChanged += HandleStateChanged;
        HandleStateChanged(_movement.State);          
    }

    private void OnDisable()
    {
        _movement.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(PlayerState state)
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Interact", false);
        _animator.SetBool("Fire", false);

        switch (state)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Run:
                _animator.SetBool("Run", true);
                break;

            case PlayerState.Attack:
                _animator.SetBool("Fire", true);
                break;

            case PlayerState.Interact:
                _animator.SetBool("Run", true);
                break;

            case PlayerState.Dead:
                _animator.SetTrigger("Death");
                break;
        }
        
        
    }
}

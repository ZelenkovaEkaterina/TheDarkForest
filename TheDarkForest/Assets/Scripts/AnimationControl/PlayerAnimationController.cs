using System;
using Player;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private static readonly int RunHash      = Animator.StringToHash("Run");
    private static readonly int FireHash     = Animator.StringToHash("Fire");
    private static readonly int InteractHash = Animator.StringToHash("Interact");
    private static readonly int DeadHash     = Animator.StringToHash("Dead");

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
        HandleStateChanged(_movement.State);          // синхронизация на старте
    }

    private void OnDisable()
    {
        _movement.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(PlayerState state)
    {
        // Сначала сбрасываем булы, потом ставим нужный
        _animator.SetBool(RunHash, false);
        _animator.SetBool(InteractHash, false);

        switch (state)
        {
            case PlayerState.Idle:
                // уже сброшено
                break;

            case PlayerState.Run:
                _animator.SetBool(RunHash, true);
                break;

            case PlayerState.Attack:
                _animator.SetTrigger(FireHash);   // trigger — одноразовая анимация
                break;

            case PlayerState.Interact:
                _animator.SetBool("Run", true);
                break;

            case PlayerState.Dead:
                _animator.SetBool(DeadHash, true);
                break;
        }
        
        
    }
}

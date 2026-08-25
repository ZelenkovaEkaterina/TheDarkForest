using System;
using Player;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animator;
    
    private void Awake()
    {
        _playerController =  GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerController.OnRun += HandleRun;
        _playerController.OnFire += HandleFire;
        _playerController.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _playerController.OnRun -= HandleRun;
        _playerController.OnFire -= HandleFire;
        _playerController.OnDeath -= HandleDeath;
    }

    private void HandleDeath(bool obj)
    {
        _animator.SetBool("Dead", obj);
        Debug.Log("ded");
    }

    private void HandleFire(bool obj)
    {
        _animator.SetBool("Fire", obj);
    }

    private void HandleRun(bool obj)
    {
        _animator.SetBool("Run", obj);
    }
}

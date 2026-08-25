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
    }

    private void OnDisable()
    {
        _playerController.OnRun -= HandleRun;
        _playerController.OnFire -= HandleFire;
    }

    private void HandleFire(bool obj)
    {
        _animator.SetBool("Fire", obj);
        Debug.Log("Fire");
    }

    private void HandleRun(bool obj)
    {
        _animator.SetBool("Run", obj);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action OnGameOver;
    public event Action OnExit;
    public event Action OnPlayerDead;
    [SerializeField] private PlayerHealthComponent _playerHealthComponent;
    [SerializeField] private bool bIsDead = false;

    [SerializeField] private GameOver _gameOver;
    
    private void OnEnable()
    {
        _playerHealthComponent.OnDeathEvent += HandleDead;
        _gameOver.OnGameOver += SetOver;
    }

    private void OnDisable()
    {
        _playerHealthComponent.OnDeathEvent -= HandleDead;
        _gameOver.OnGameOver -= SetOver;
    }

    private void SetOver()
    {
        OnExit?.Invoke();
    }

    private void HandleDead(bool obj)
    {
        bIsDead = obj;
        
        OnPlayerDead?.Invoke();
        StartCoroutine(Pause());
        //_playerHealthComponent.gameObject.SetActive(false);
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }
    
    
}

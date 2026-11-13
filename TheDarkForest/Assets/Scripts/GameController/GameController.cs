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
        if (_playerHealthComponent != null)
            _playerHealthComponent.OnDeath += HandlePlayerDeath;

        if (_gameOver != null)
            _gameOver.OnGameOver += SetOver;
    }

    private void OnDisable()
    {
        if (_playerHealthComponent != null)
            _playerHealthComponent.OnDeath -= HandlePlayerDeath;

        if (_gameOver != null)
            _gameOver.OnGameOver -= SetOver;
    }

    private void SetOver()
    {
        OnExit?.Invoke();
    }

    private void HandlePlayerDeath(HealthSystem system)
    {
        if (system is not PlayerHealthComponent) return;

        if (bIsDead) return;
        bIsDead = true;

        OnPlayerDead?.Invoke();
        StartCoroutine(GameOverAfterDelay(3f));
    }

    private IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }
    
    
}

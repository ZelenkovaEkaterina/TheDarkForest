using System;
using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public event Action OnGameOver;

    private float _waitingGameOver = 3f;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        other.gameObject.SetActive(false);
        
        StartCoroutine(WaitForGameOver());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(_waitingGameOver);
        OnGameOver?.Invoke();
    }
}

using System;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public event Action OnGameOver;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        OnGameOver?.Invoke();
    }
}

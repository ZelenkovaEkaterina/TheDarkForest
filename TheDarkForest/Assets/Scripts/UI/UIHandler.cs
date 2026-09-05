using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    
    [SerializeField] private Slider _healthSlider;

    private void Awake()
    {
        //_healthSlider.value = _player.GetComponent<PlayerHealthComponent>().MaxHealth;
    }

    private void Update()
    {
     
    }

    private void OnEnable()
    {
        Debug.Log("UIHandler OnEnable - subscribing");
        _player.GetComponent<PlayerHealthComponent>().OnTakeDamageEvent += HandleHealthSlider;
    }
    private void OnDisable()
    {
        _player.GetComponent<PlayerHealthComponent>().OnTakeDamageEvent -= HandleHealthSlider;
    }

    private void HandleHealthSlider()
    {
        
        var health = _player.GetComponent<PlayerHealthComponent>();
        Debug.Log(health.CurrentHealth);
        _healthSlider.value = health.CurrentHealth; 
    }
}

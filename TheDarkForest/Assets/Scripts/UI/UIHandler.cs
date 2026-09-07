using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private PlayerHealthComponent _healthComponent;
    
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Button _healButton;

    private void Awake()
    {
        if (_player == null)
            return;

        _healthComponent = _player.GetComponent<PlayerHealthComponent>();
        if (_healthComponent == null)
            return;
        
        _healthSlider.maxValue = _healthComponent.MaxHealth;
        _healthSlider.value = _healthComponent.CurrentHealth;
    }

    private void OnEnable()
    {
        _healthComponent.OnHealthChanged += UpdateHealthSlider;
    }

    private void OnDisable()
    {
        _healthComponent.OnHealthChanged -= UpdateHealthSlider;
    }

    // Этот метод будет вызываться из инспектора через UnityEvent
    public void UpdateHealthSlider()
    {
        Debug.Log("1");
        if (_healthComponent != null && _player != null)
        {
            _healthSlider.value = _healthComponent.CurrentHealth;
        }
    }
}

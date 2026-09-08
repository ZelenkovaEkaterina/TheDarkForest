using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private PlayerHealthComponent _healthComponent;
    private PlayerManaComponent _manaComponent;
    
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Button _healButton;

    [SerializeField] private Slider _manaSlider;
    [SerializeField] private Button _manaButton;

    private void Awake()
    {
        if (_player == null)
            return;

        _healthComponent = _player.GetComponent<PlayerHealthComponent>();
        _manaComponent = _player.GetComponent<PlayerManaComponent>();
        
        if (_healthComponent == null)
            return;
        
        _healthSlider.maxValue = _healthComponent.MaxHealth;
        _healthSlider.value = _healthComponent.CurrentHealth;
        
        _manaSlider.maxValue = _manaComponent.MaxMana;
        _manaSlider.value = _manaComponent.CurrentMana;
    }

    private void OnEnable()
    {
        _healthComponent.OnHealthChanged += UpdateHealthSlider;
        _manaComponent.OnManaChanged += UpdateManaSlider;
    }

    private void OnDisable()
    {
        _healthComponent.OnHealthChanged -= UpdateHealthSlider;
        _manaComponent.OnManaChanged -= UpdateManaSlider;
    }

    private void UpdateManaSlider()
    {
        if (_manaComponent != null && _player != null)
        {
            _manaSlider.value = _manaComponent.CurrentMana;
        }
    }

    private void UpdateHealthSlider()
    {
        if (_healthComponent != null && _player != null)
        {
            _healthSlider.value = _healthComponent.CurrentHealth;
        }
    }
}

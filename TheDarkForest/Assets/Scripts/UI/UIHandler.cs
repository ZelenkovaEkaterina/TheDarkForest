using System;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.Rendering;
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

    [SerializeField] private GameObject _golem;
    private EnemyHealthComponent _golemHealthComponent;
    
    [SerializeField] private Slider _golemSlider;
    [SerializeField] private GameObject _golemZone;
    private GolemAgroZone _golemZoneComponent;

    private void Awake()
    {
        if (_player != null)
        {
            _healthComponent = _player.GetComponent<PlayerHealthComponent>();
            _manaComponent  = _player.GetComponent<PlayerManaComponent>();
        }

        if (_golem != null)
        {
            _golemHealthComponent = _golem.GetComponent<EnemyHealthComponent>();
            _golemZoneComponent = _golemZone.GetComponent<GolemAgroZone>();
            _golemSlider.transform.parent.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (_healthComponent == null)
            return;
        
        _healthSlider.maxValue = _healthComponent.MaxHealth;
        _healthSlider.value = _healthComponent.CurrentHealth;
        
        _manaSlider.maxValue = _manaComponent.MaxMana;
        _manaSlider.value = _manaComponent.CurrentMana;

        if (_golemHealthComponent == null) return;
        _golemSlider.maxValue = _golemHealthComponent.MaxHealth;
        _golemSlider.value = _golemHealthComponent.CurrentHealth;
    }

    private void OnEnable()
    {
        _healthComponent.OnHealthChanged += UpdateHealthSlider;
        _manaComponent.OnManaChanged += UpdateManaSlider;
        
        _golemHealthComponent.OnHealthChanged += UpdateHealthSlider;
        _golemZoneComponent.OnEnterPlayer += ShowPanel;
        _golemZoneComponent.OnExitPlayer += HidePanel;
    }

    private void OnDisable()
    {
        _healthComponent.OnHealthChanged -= UpdateHealthSlider;
        _manaComponent.OnManaChanged -= UpdateManaSlider;
        
        _golemHealthComponent.OnHealthChanged -= UpdateHealthSlider;
        _golemZoneComponent.OnEnterPlayer -= ShowPanel;
        _golemZoneComponent.OnExitPlayer -= HidePanel;
    }

    private void UpdateManaSlider(int mana)
    {
        if (_manaComponent != null && _player != null)
        {
            _manaSlider.value = mana;
        }
    }

    private void UpdateHealthSlider()
    {
        if (_healthComponent != null && _player != null)
        {
            _healthSlider.value = _healthComponent.CurrentHealth;
        }
        
        if (_golemHealthComponent != null && _golem != null)
        {
            _golemSlider.value = _golemHealthComponent.CurrentHealth;
        }

        if (_golemSlider.value == 0)
        {
            HidePanel();
        }
    }

    private void HidePanel()
    {
        _golemSlider.transform.parent.gameObject.SetActive(false);
    }

    private void ShowPanel()
    {
        _golemSlider.transform.parent.gameObject.SetActive(true);
    }
}

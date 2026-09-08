using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerManaComponent : MonoBehaviour
{
    public event Action OnManaChanged;
    public event Action<Type> OnManaBottleUse;
  
    
    private PlayerMovementComponent _player;
    [SerializeField] private InventoryDatabase _inventory;
    [SerializeField] private InventoryType _inventoryType;
    
    [SerializeField] private int _maxMana = 100;
    private int _currentMana;
    private int _manaRegenAmount = 5;

    public int MaxMana => _maxMana;
    public int CurrentMana => _currentMana;

    private void Awake()
    {
        _player = GetComponent<PlayerMovementComponent>();
        _currentMana =  _maxMana;
        StartCoroutine(RegenMana());
    }

    private void Update()
    {
       // Debug.Log(_currentMana);
    }

    private void OnEnable()
    {
        _player.OnCast += UseMana;
    }

    private void OnDisable()
    {
        _player.OnCast -= UseMana;
        StopAllCoroutines();
    }

    public void UseMana(int cast)
    {
        _currentMana -= cast;
        OnManaChanged?.Invoke();
       
    }

    public void RegenerateMana()
    {
        if (_currentMana != _maxMana && _inventory.IDB.InvDB[1].Count > 0)
        {
            Debug.Log(_currentMana);
            _currentMana += _inventoryType.Slot.InvSlots[1].Cost; //добавить переменную
            OnManaChanged?.Invoke();
            OnManaBottleUse?.Invoke(Type.Mana);
        }
        return;
    }

    private IEnumerator RegenMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (_currentMana != _maxMana)
            {
                Debug.Log(_currentMana);
                _currentMana += _manaRegenAmount;
                OnManaChanged?.Invoke();
            }
        }
    }
}

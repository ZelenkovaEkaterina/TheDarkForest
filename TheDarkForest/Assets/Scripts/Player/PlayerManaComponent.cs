using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerManaComponent : MonoBehaviour
{
    public event Action<int> OnManaChanged;
    public event Action<Type> OnManaBottleUse;
  

    
    private PlayerMovementComponent _player;
    private PlayerCombat _playerCombat;
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
        _playerCombat = GetComponent<PlayerCombat>();
        _currentMana =  _maxMana;
        StartCoroutine(RegenMana());
    }
    public bool TrySpend(int amount)
    {
        if (_currentMana < amount) return false;
        _currentMana -= amount;
        OnManaChanged?.Invoke(_currentMana);
        return true;
    }
    private void Update()
    {
       // Debug.Log(_currentMana);
    }

    private void OnEnable()
    {
        _playerCombat.OnCast += UseMana;
    }
    private void OnDisable()
    {
        _playerCombat.OnCast -= UseMana;
        StopAllCoroutines();
    }

    public void UseMana(int cast)
    {
        _currentMana -= cast;
        OnManaChanged?.Invoke(_currentMana);
    }

    public void RegenerateMana()
    {
        if (_currentMana != _maxMana && _inventory.IDB.InvDB[1].Count > 0)
        {
            _currentMana += _inventoryType.Slot.InvSlots[1].Cost; //добавить переменную
            OnManaChanged?.Invoke(_currentMana);
            OnManaBottleUse?.Invoke(Type.Mana);
        }
    }

    private IEnumerator RegenMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (_currentMana != _maxMana)
            {
                _currentMana += _manaRegenAmount;
                OnManaChanged?.Invoke(_currentMana);
            }
        }
    }
    
    
}

using System;
using UnityEngine;

public class EnemyDamageableComponent : MonoBehaviour, IDamageble
{
    private GameObject EnemyDamageable;
    public delegate void DamageHandler(int damage, GameObject source);
    
    public DamageHandler OnDamageTaken;
    public DamageHandler OnDamageDealt;
    
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        EnemyDamageable = this.gameObject;
    }

    public void TakeDamage(int damage, GameObject source)
    {
        if (IsDead()) return;
        
        currentHealth -= damage;
        OnDamageTaken?.Invoke(damage, source);
        Debug.Log($"нанесен урон {damage}. Осталось {currentHealth} здоровья");
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}

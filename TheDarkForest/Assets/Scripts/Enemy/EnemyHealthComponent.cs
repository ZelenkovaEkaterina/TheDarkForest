using System;
using Mono.Cecil;
using UnityEngine;


public class EnemyHealthComponent : MonoBehaviour, IDamageable
{
    public event Action<int, GameObject> OnDamageTaken;
    public event Action OnDeath;
    
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, GameObject source)
    {
        if (IsDead()) return;
        
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        OnDamageTaken?.Invoke(damage, source);
        
        if(IsDead()) 
            OnDeath?.Invoke();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void Heal(int amount)
    {
        if (IsDead()) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
}
using System;
using UnityEngine;

public interface IDamageble
{
    void TakeDamage(int damage, GameObject source);
    bool IsDead();
    int CurrentHealth { get; }
    int MaxHealth {get;}
}

public interface IDamageDiller
{
    void DealDamage(IDamageble target, int damage);
    int DamageAmount { get; }
}

public class DamageSystem : MonoBehaviour, IDamageble
{
    public delegate void DamageHandler(int damage, GameObject source);

    public DamageHandler OnDamageTaken;
    public DamageHandler OnDamageDealt;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    //public int health = 100;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, GameObject source)
    {
        if (IsDead()) return;
             
        //currentHealth -= Mathf.Max(0,currentHealth - damage);
        currentHealth -= damage;
        OnDamageTaken?.Invoke(damage, source); 
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
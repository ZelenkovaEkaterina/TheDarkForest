using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    public event Action<int, GameObject> OnDamageTaken;
    public event Action OnDeath; 
    public event Action OnHealthChanged;
    
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;
    

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] private InventoryType _inventoryType;
    
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, GameObject source)
    {
        if (IsDead()) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnDamageTaken?.Invoke(damage, source);
        OnHealthChanged?.Invoke();
        if (IsDead())
        {
            OnDeath?.Invoke();
            Die();            
        }
    }

    public bool IsDead() => currentHealth <= 0;
    
    protected abstract void Die();

    protected virtual void OnDestroy()
    {
        OnDamageTaken = null;
        OnDeath = null;
    }
    public void Heal()
    {
        currentHealth += _inventoryType.Slot.InvSlots[0].Cost;
        OnHealthChanged?.Invoke();
        Debug.Log("heal");
    }
    
}

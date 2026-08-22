using System;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    public event Action<int, GameObject> OnDamageTaken;
    public event Action OnDeath; // вызывается ДО Die(), чтобы подписчики могли среагировать

    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, GameObject source)
    {
        if (IsDead()) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnDamageTaken?.Invoke(damage, source);

        if (IsDead())
        {
            OnDeath?.Invoke(); // сначала оповещаем всех (UI, звуки, эффекты)
            Die();            // затем выполняем специфичную для сущности смерть
        }
    }

    public bool IsDead() => currentHealth <= 0;

    // Абстрактный метод — его обязаны реализовать наследники
    protected abstract void Die();

    protected virtual void OnDestroy()
    {
        // Отписываемся от событий, чтобы избежать утечек
        OnDamageTaken = null;
        OnDeath = null;
    }
}

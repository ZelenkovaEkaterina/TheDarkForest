using System;
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
Debug.Log(currentHealth);
        if (IsDead())
        {
            //gameObject.SetActive(false);
            OnDeath?.Invoke();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            if (projectile.Owner == gameObject) return;
            
            TakeDamage(projectile.Damage, projectile.Owner);
        }
    }
    
    private void OnDestroy()
    {
        OnDamageTaken = null;
        OnDeath = null;
    }
}
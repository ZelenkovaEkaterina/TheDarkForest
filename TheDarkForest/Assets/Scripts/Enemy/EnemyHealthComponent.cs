using System;
using System.Collections;
using UnityEngine;


public class EnemyHealthComponent : HealthSystem
{
    public event Action<Transform> OnDieEvent;
    protected override void Die()
    {
        OnDieEvent?.Invoke(transform);
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Projectile>(out var projectile)) return;
        if (projectile.Owner == gameObject) return; 

        TakeDamage(projectile.Damage, projectile.Owner);
    }
}
using System;
using UnityEngine;


public class EnemyHealthComponent : HealthSystem
{
    protected override void Die()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Projectile>(out var projectile)) return;
        if (projectile.Owner == gameObject) return; // не раним себя

        TakeDamage(projectile.Damage, projectile.Owner);
    }
}
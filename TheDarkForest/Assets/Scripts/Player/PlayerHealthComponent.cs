using System;
using UnityEngine;

public class PlayerHealthComponent : HealthSystem
{
    protected override void Die()
    {
        Debug.Log("ты сдох");
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(5, other.gameObject);
            Debug.Log(currentHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Projectile>(out var projectile)) return;
        if (projectile.Owner == gameObject) return; // не раним себя

        TakeDamage(projectile.Damage, projectile.Owner);
    }
}

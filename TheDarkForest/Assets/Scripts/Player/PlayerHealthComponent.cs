using System;
using Player;
using UnityEngine;

public class PlayerHealthComponent : HealthSystem
{
    public event Action<bool> OnDeathEvent;
    private PlayerState _stateDeath;
    public PlayerState StateDeath => _stateDeath;
    protected override void Die()
    {
        Debug.Log("ты сдох");
        _stateDeath = PlayerState.Dead;
        OnDeathEvent?.Invoke(true);
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
        if (other.TryGetComponent<Projectile>(out var projectile))
        {
            if (projectile.Owner == gameObject) return; // не раним себя

            TakeDamage(projectile.Damage, projectile.Owner);
        }
    }
}

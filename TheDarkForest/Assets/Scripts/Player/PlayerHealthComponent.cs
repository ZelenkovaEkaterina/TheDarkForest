using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthComponent : HealthSystem
{
    public event Action<bool> OnDeathEvent;
    //public UnityEvent OnHealthChanged;
    private PlayerState _stateDeath;
    public PlayerState StateDeath => _stateDeath;

    private int _damage = 5; 
    protected override void Awake()
    {
        base.Awake();
    }

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
            TakeDamage(_damage, other.gameObject);
        }
    }
}

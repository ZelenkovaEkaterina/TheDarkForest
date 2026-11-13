using System;
using UnityEngine;

public class EnemyDeadVFX : MonoBehaviour
{
    [SerializeField] private GameObject _deathVfxPrefab;
    [SerializeField] private float _vfxLifetime = 4f;

    private EnemyHealthComponent _enemyHealth;

    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealthComponent>();
        
        if (_enemyHealth == null) return;
        _enemyHealth.OnDeath += DeadEffect;
    }

    private void OnDisable()
    {
        if (_enemyHealth == null) return;
        _enemyHealth.OnDeath -= DeadEffect;
    }

    private void DeadEffect(HealthSystem system)
    {
        if (_deathVfxPrefab == null) return;

        GameObject vfx = Instantiate(
            _deathVfxPrefab,
            system.transform.position,
            Quaternion.identity
        );

        Destroy(vfx, _vfxLifetime);
    }
}

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
        _enemyHealth.OnDieEvent += DeadEffect;
    }

    private void OnDisable()
    {
        if (_enemyHealth == null) return;
        _enemyHealth.OnDieEvent -= DeadEffect;
    }

    private void DeadEffect(Transform enemyTransform)
    {
        if (_deathVfxPrefab == null) return;

        GameObject vfx = Instantiate(
            _deathVfxPrefab,
            enemyTransform.position,
            Quaternion.identity
        );

        Destroy(vfx, _vfxLifetime);
    }
}

using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage, GameObject source);
    bool IsDead();
    int CurrentHealth { get; }
    int MaxHealth {get;}
}

public interface IDamageDealer
{
    void DealDamage(IDamageable target, int damage);
    int DamageAmount { get; }
}

public class DamageSystem : MonoBehaviour
{
    /*private ProjectilePool<Projectile> _shotPool;
    public ProjectilePool<Projectile> ShotPool => _shotPool;
    [SerializeField] private Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;
    
    public void HandleFire(Vector3 target)
    {
        Projectile shot = _shotPool.Get();
        shot.ReturnToPool += OnDespawn;
        shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        shot.OnSpawn(target);
    }
    
    private void OnDespawn(Projectile obj)
    {
        obj.OnDespawn();
        obj.ReturnToPool -= OnDespawn;
        _shotPool.Return(obj);
    }*/
}
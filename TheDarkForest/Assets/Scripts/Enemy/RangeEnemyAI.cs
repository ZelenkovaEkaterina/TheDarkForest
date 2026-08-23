using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(EnemyController))]
   
    public class RangeEnemyAI : EnemyCombatAI
    {
        private float _waitAttack = 3f;
        
        private ProjectilePool<Projectile> _shotPool;
        private Transform _spawnPoint;
        
        protected override void Awake()
        {
            base.Awake();
            attackRange = 6f;
        }

        private void Start()
        {
            _spawnPoint = gameObject.GetComponentInChildren<Transform>();
        }
        
        public void InitializeProjectilePool(ProjectilePool<Projectile> pool)
        {
            _shotPool = pool;
        }
        protected override void PerformAttack()
        {
            Debug.Log("стреляют");
            if (Target == null || _shotPool == null) return;
            
            Projectile shot = _shotPool.Get();
            if (shot == null) return;

            shot.SetOwner(gameObject);
            shot.IgnoreOwnerCollision(GetComponent<Collider>());
            shot.ReturnToPool += OnDespawn;
            shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            shot.OnSpawn(Target.transform.position);
            
        }
        private void OnDespawn(Projectile obj)
        {
            obj.OnDespawn();
            obj.ReturnToPool -= OnDespawn;
            _shotPool.Return(obj);
        }
        private IEnumerator WaitAttack()
        {
            yield return new WaitForSeconds(_waitAttack);
            PerformAttack();
        }
    }
}
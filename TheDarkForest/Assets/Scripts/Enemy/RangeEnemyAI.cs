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

        private float _heightTarget = 2f;
        
        private EnemyWeapon _enemyWeapon;
        
        protected override void Awake()
        {
            base.Awake();
            attackRange = 6f;
        }

        private void Start()
        {
            _spawnPoint = gameObject.GetComponentInChildren<Transform>();

            _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
            _enemyWeapon.gameObject.SetActive(false);
        }

        public void InitializeProjectilePool(ProjectilePool<Projectile> pool)
        {
            _shotPool = pool;
        }
        protected override void PerformAttack()
        {
            base.PerformAttack();
            if (Target == null || _shotPool == null) return;
            
            Projectile shot = _shotPool.Get();
            if (shot == null) return;

            shot.SetOwner(gameObject);
            shot.IgnoreOwnerCollision(GetComponent<Collider>());
            Debug.Log(GetComponent<Collider>());
            shot.ReturnToPool += OnDespawn;
            shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            Vector3 pos = Target.transform.position;
            pos.y += _heightTarget;
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
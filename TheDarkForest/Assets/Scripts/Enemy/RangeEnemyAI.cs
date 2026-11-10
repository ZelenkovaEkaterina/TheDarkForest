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
        [SerializeField]private float _waitAttack = 3f;
        
        private ProjectilePool<Projectile> _shotPool;
        private Transform _spawnPoint;

        private float _heightTarget = 2f;
        
        private EnemyWeapon _enemyWeapon;
        private Collider _selfCollider;
        
        protected override void Awake()
        {
            base.Awake();
            attackRange = 6f;
            _selfCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
            if (_enemyWeapon != null)
            {
                _enemyWeapon.GetComponent<Renderer>().enabled = false;
                _spawnPoint = _enemyWeapon.transform;
            }
            else
            {
                _spawnPoint = transform;
            }
        }

        public void InitializeProjectilePool(ProjectilePool<Projectile> pool)
        {
            _shotPool = pool;
        }
        protected override void PerformAttack()
        {
            /*base.PerformAttack();
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
            shot.OnSpawn(Target.transform.position);*/
            
            base.PerformAttack();
            if (Target == null || _shotPool == null || _spawnPoint == null) return;

            Projectile shot = _shotPool.Get();
            if (shot == null) return;

            shot.SetOwner(gameObject);
            shot.IgnoreOwnerCollision(_selfCollider);
            
            shot.ReturnToPool -= OnDespawn;
            shot.ReturnToPool += OnDespawn;

            shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);

            Vector3 pos = Target.position;
            pos.y += _heightTarget;
            Debug.Log(attackRange);
            shot.OnSpawn(pos);
            
        }
        private void OnDespawn(Projectile obj)
        {
            obj.OnDespawn();
            obj.ReturnToPool -= OnDespawn;
            _shotPool.Return(obj);
        }

        public void SpawnAttack()
        {
            PerformAttack();
        }
    }
}
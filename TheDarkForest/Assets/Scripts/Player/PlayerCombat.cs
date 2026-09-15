using System;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public event Action<int> OnCast;

        [Header("Refs")] [SerializeField] private Transform _spawnPoint;
        [SerializeField] private LayerMask _enemyMask;
        [SerializeField] private GameObject _gameController;
        [SerializeField] private bool _autoAttack = true;

        [Header("Stats")] [SerializeField] private float _attackRange = 10f;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private float _autoAggroRange = 8f;

        private bool _autoTargetSuppressed;
        
        private ProjectilePool<Projectile> _shotPool;
        private DamageSystem _damageSystem;
        private PlayerMovementComponent _movement;

        private Transform _currentTarget;
        private float _nextFireTime;

        public float AttackRange => _attackRange;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
            if (_gameController != null)
                _damageSystem = _gameController.GetComponent<DamageSystem>();
        }

        public void Init(ProjectilePool<Projectile> shotPool)
        {
            _shotPool = shotPool;
        }

        public void SetTarget(Transform enemy)
        {
            _currentTarget = enemy;
            _autoTargetSuppressed = false;
        }
        public void ClearTarget() => _currentTarget = null;
        
        public void CancelCombat()
        {
            _currentTarget = null;
            _autoTargetSuppressed = true;
        }

        private void Update()
        {
            if (_currentTarget == null && _autoAttack && !_autoTargetSuppressed)
                _currentTarget = FindClosestEnemy(_autoAggroRange);
            
            if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
            {
                _currentTarget = null;
                if (_movement.State == PlayerState.Attack)
                    _movement.SetState(PlayerState.Idle);
                return;
            }

            float dist = Vector3.Distance(transform.position, _currentTarget.position);
            
            if (dist > _attackRange)
            {
                _movement.MoveToTarget(_currentTarget, _attackRange - 0.2f);
                if (_movement.State == PlayerState.Attack)
                    _movement.SetState(PlayerState.Run);
                return;
            }
            
            if (_movement.IsMoving)
                _movement.Stop();

            HandleFire(_currentTarget.position);
        }

        private void HandleFire(Vector3 target)
        {
            if (Time.time < _nextFireTime) return;
            _nextFireTime = Time.time + _fireRate;
            
            Vector3 dir = target - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
            
            if (_shotPool != null && _spawnPoint != null)
            {
                Projectile shot = _shotPool.Get();
                shot.SetOwner(gameObject);
                shot.IgnoreOwnerCollision(GetComponent<Collider>());
                shot.ReturnToPool += OnDespawn;
                shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
                shot.OnSpawn(target);
            }

            OnCast?.Invoke(7);
            _movement.SetState(PlayerState.Attack);
        }

        private void OnDespawn(Projectile obj)
        {
            obj.OnDespawn();
            obj.ReturnToPool -= OnDespawn;
            _shotPool.Return(obj);
        }

        private Transform FindClosestEnemy(float radius)
        {
            var hits = Physics.OverlapSphere(transform.position, radius, _enemyMask);
            Transform best = null;
            float bestSqr = float.MaxValue;
            foreach (var h in hits)
            {
                if (!h.gameObject.activeInHierarchy) continue;
                float d = (h.transform.position - transform.position).sqrMagnitude;
                if (d < bestSqr)
                {
                    bestSqr = d;
                    best = h.transform;
                }
            }

            return best;
        }
    }
}
using System;
using System.Collections;
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
        [SerializeField] private float _fireRate = 2.5f;
        [SerializeField] private float _autoAggroRange = 8f;

        private bool _autoTargetSuppressed;
        
        private ProjectilePool<Projectile> _shotPool;
        private DamageSystem _damageSystem;
        private PlayerMovementComponent _movement;
        private PlayerManaComponent _manaComponent;
        private PlayerWeapon _playerWeapon;
        
        [SerializeField] private SkillsManager _skillManager;
        [SerializeField] private SkillsSettings _settingsData;
        private int _manaCost = 0;
        private int _buff = 1;
        private int _activeSkillId = -1; 
        private bool _hasActiveSkill;

        private Transform _currentTarget;
        private float _nextFireTime;

        public float AttackRange => _attackRange;
        
        public int ManaCost => _manaCost;
        public int Buff => _buff;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
            _manaComponent = GetComponent<PlayerManaComponent>();
            _playerWeapon = GetComponent<PlayerWeapon>();
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
            if (_movement.State == PlayerState.Interact) return;
            
            if (_currentTarget == null && _autoAttack && !_autoTargetSuppressed)
                _currentTarget = FindClosestEnemy(_autoAggroRange);
            
            if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
            {
                _currentTarget = null;
                if (_movement.State == PlayerState.Attack)
                {
                    _playerWeapon.EquipToBack();
                    _movement.SetState(PlayerState.Idle);
                }
                    
                return;
            }

            float dist = Vector3.Distance(transform.position, _currentTarget.position);
            
            if (dist > _attackRange)
            {
                _movement.MoveToTarget(_currentTarget, _attackRange - 0.2f);
                if (_movement.State == PlayerState.Attack)
                {
                    _movement.SetState(PlayerState.Run);
                    _playerWeapon.EquipToBack();
                }
                    
                return;
            }
            
            if (_movement.IsMoving)
                _movement.Stop();

            HandleFire(_currentTarget.position);
            
        }

        private void HandleFire(Vector3 target)
        {
            _playerWeapon.EquipToHand();
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
                
                Vector3 shootDir = (target - _spawnPoint.position).normalized;
                Quaternion shootRot = Quaternion.LookRotation(shootDir);

                shot.transform.SetPositionAndRotation(_spawnPoint.position, shootRot);
                shot.OnSpawn(target);
            }


            if (_manaComponent.CurrentMana >= _manaCost)
            {
                OnCast?.Invoke(_manaCost);
            }
            else
            {
                Debug.Log("не хватает маны");
            }
            
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

        private void OnEnable()
        {
            _skillManager.OnUseSkill += HandlerBuff;
        }

        private void OnDisable()
        {
            _skillManager.OnUseSkill -= HandlerBuff;
        }

        private void HandlerBuff(int skillId, bool  isOn)
        {
            if (skillId == -1 && !isOn)
            {
                _manaCost = 0;
                _buff = 1;
                _activeSkillId = -1;
                _hasActiveSkill = false;
                return;
            }

            if (isOn)
            {
                foreach (var s in _settingsData.SkillsSettingsData.SkilItem)
                    if (s.BowId == skillId)
                    {
                        _manaCost = s.ManaCost;
                        _buff = s.ShotBuff;
                        _activeSkillId = skillId;
                        _hasActiveSkill = true;
                        return;
                    }
            }
            else if (_activeSkillId == skillId)
            {
                _manaCost = 0;
                _buff = 1;
                _activeSkillId = -1;
                _hasActiveSkill = false;
            }
        }
    }
}
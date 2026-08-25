using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {

        
        private PlayerController _playerController;
        private PlayerState _currentPlayerState;
        public PlayerState State => _currentPlayerState;
        
        private NavMeshAgent agent;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private Transform _spawnPoint;

        [SerializeField] private LayerMask groundLayer;
        
        private ProjectilePool<Projectile> _shotPool;
        [SerializeField] private float _fireRate = 0.5f;
        private float _nextFireTime;
        [SerializeField]private float _attackRange = 10f;

        [SerializeField]private GameObject _gameController;
        private DamageSystem  _damageSystem;
        
        private bool _hasAttacked;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _playerController = GetComponent<PlayerController>();
            _currentPlayerState = PlayerState.Idle;
        }

        private void Start()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (mainCamera == null) mainCamera = Camera.main;
            
        }
        
        public void Init (ProjectilePool<Projectile> shotPool)
        {
            _shotPool = shotPool;
        }

        private void Update()
        {
            UpdateState();
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);

                    
                    if (hit.collider.gameObject.activeInHierarchy && hit.collider.CompareTag("Enemy"))
                    {
                        if (hit.collider == null || !hit.collider.gameObject.activeInHierarchy)
                            return;
                        if (distanceToTarget > _attackRange)
                        {
                            agent.SetDestination(hit.transform.position);
                            agent.stoppingDistance = _attackRange;
                        }
                        else
                        {
                            agent.isStopped = true;
                            agent.ResetPath();
                            _currentPlayerState = PlayerState.Attack;
                            HandleFire(hit.transform.position);
                        }
                        return;
                    }
                    
                    agent.isStopped = false;
                    agent.SetDestination(hit.point);
                }
            }
        }
        
        private void HandleFire(Vector3 target)
        {
            if(Time.time < _nextFireTime) return;

            _nextFireTime = Time.time + _fireRate;

            Projectile shot = _shotPool.Get();
            shot.SetOwner(gameObject);
            shot.IgnoreOwnerCollision(GetComponent<Collider>());
            shot.ReturnToPool += OnDespawn;
            shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            shot.OnSpawn(target);
            
            _currentPlayerState = PlayerState.Attack;
            _hasAttacked = true;
            
            Vector3 directionToTarget = (target - transform.position).normalized;
            directionToTarget.y = 0; // игнорируем вертикаль
            if (directionToTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }
        }

        private void OnDespawn(Projectile obj)
        {
            obj.OnDespawn();
            obj.ReturnToPool -= OnDespawn;
            _shotPool.Return(obj);
        }

        private void UpdateState()
        {
            if (_currentPlayerState == PlayerState.Attack)
            {
                if (_hasAttacked)
                {
                    _hasAttacked = false;
                    bool isMoving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance && !agent.pathPending;
                    _currentPlayerState = isMoving ? PlayerState.Run : PlayerState.Idle;
                }
                else
                {
                    _currentPlayerState = PlayerState.Idle;
                }
                return;
            }
            
            bool isMovingNow = agent.hasPath && agent.remainingDistance > agent.stoppingDistance && !agent.pathPending;
            _currentPlayerState = isMovingNow ? PlayerState.Run : PlayerState.Idle;
        }
    }
}


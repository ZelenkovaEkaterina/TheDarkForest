using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        internal event Action<bool> OnFire;
        
        private NavMeshAgent agent;
        [SerializeField] private Camera mainCamera;
        
        //private PlayerDamageComponent playerDamageComponent;
        [SerializeField] private Transform _spawnPoint;

        [SerializeField] private LayerMask groundLayer;
        
        private ProjectilePool<Projectile> _shotPool;
        [SerializeField] private float _fireRate = 0.5f;
        private float _nextFireTime;
        [SerializeField]private float _attackRange = 5f;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            //playerDamageComponent = GetComponent<PlayerDamageComponent>();
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
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    Debug.Log(hit.collider.name);
                    Debug.Log($"Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                    Debug.Log($"Tag: {hit.collider.gameObject.tag}");
                    float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                    /*if (hit.collider.CompareTag("Enemy"))
                    {
                        Debug.Log(distanceToTarget);
                    }*/
                    //Debug.Log(hit.collider.name);
                    
                    if (hit.collider.gameObject.activeInHierarchy && hit.collider.CompareTag("Enemy"))
                    //if (hit.collider.TryGetComponent<EnemyHealthComponent>(out var enemyHealth))
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
                            Debug.Log("выстрел");
                            HandleFire(hit.transform.position);
                        }
                        
                        //playerDamageComponent.AttackEnemy(hit.collider.gameObject);
                        return;
                        
                    }
                
                    // Иначе двигаемся в точку
                    agent.SetDestination(hit.point);
                }
            }
        }
        
        private void HandleFire(Vector3 target)
        {
            if(Time.time < _nextFireTime) return;

            _nextFireTime = Time.time + _fireRate;

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
        }
    }
}


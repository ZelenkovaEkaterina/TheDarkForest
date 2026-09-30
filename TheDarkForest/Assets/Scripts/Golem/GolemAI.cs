using System;
using System.Collections;
using UnityEngine;

public enum GolemState
{
    Idle,
    Attack
}

public class GolemAI : MonoBehaviour
{
    public event Action<GolemState> OnStateChange;
    
    private Transform _playerTransform;
    private float rotationSpeed = 180f;
    
    [SerializeField] private Transform _spawnPoint;
    
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] private float _attackCooldown = 2f;
    private Coroutine _attackRoutine;
    
    private GolemState _currentState;
    public GolemState CurrentState  => _currentState;
    
    private ProjectilePool<Projectile> _shotPool;
    [SerializeField] private ProjectileController _projectileController;

    private void Awake()
    {
        _currentState = GolemState.Idle;
    }

    private void Start()
    {
        InitializeProjectileGolemPool();
    }

    private void InitializeProjectileGolemPool()
    {
        _shotPool = _projectileController.GolemShotPool;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        Vector3 direction = _playerTransform.position - transform.position;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void GolemSetTarget(Transform target)
    {
        _playerTransform = target;
        
        if (_attackRoutine == null)
            _attackRoutine = StartCoroutine(AttackLoop());
    }
    
    public void GolemClearTarget(Transform target)
    {
        if (_playerTransform != target) return;

        _playerTransform = null;

        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        _currentState = GolemState.Idle;
        OnStateChange?.Invoke(_currentState);
    }


    private void UpdateAttack()
    {
        _currentState = GolemState.Attack;
        
        
        if (_playerTransform == null || _shotPool == null) return;
        
        Projectile shot = _shotPool.Get();
        
        if (shot == null) return;

        
        shot.SetOwner(gameObject);
        shot.IgnoreOwnerCollision(GetComponent<Collider>());
        shot.ReturnToPool += OnDespawn;
        shot.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        shot.OnSpawn(_playerTransform.transform.position);
        
    }
    
    private void OnDespawn(Projectile obj)
    {
        obj.OnDespawn();
        obj.ReturnToPool -= OnDespawn;
        _shotPool.Return(obj);
    }
    
    private IEnumerator AttackLoop()
    {
        _currentState = GolemState.Attack;
        OnStateChange?.Invoke(_currentState);

        while (_playerTransform != null)
        {
            yield return new WaitForSeconds(_attackDelay);
            
            if (_playerTransform == null) break;

            UpdateAttack();

            yield return new WaitForSeconds(_attackCooldown);
        }

        _currentState = GolemState.Idle;
        _attackRoutine = null;
        
        
    }
}

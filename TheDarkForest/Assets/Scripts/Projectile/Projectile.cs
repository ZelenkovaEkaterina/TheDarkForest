using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private float _speed = 50f;
        [SerializeField] private float _lifetime = 3f;

        private Rigidbody _rb;
        private float _spawnTime = 3f;
        private bool _returned;

        private Collider _collider;
        private Collider _ignoreCollider;

        private GameObject _owner;
        
        private int _damage = 10;
        

        public event Action<Projectile> ReturnToPool;

        public float Speed => _speed;
        public float Lifetime => _lifetime;
        public GameObject Owner => _owner;
        public int  Damage => _damage;

        private void Start()
        {
            
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (Time.time - _spawnTime > _lifetime)
            {
                ReturnSelf();
            }
                
        }

        public void Initialize(Action<Projectile> returnCallback)
        {
            ReturnToPool = returnCallback;
        }

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }

        public void ReturnSelf()
        {
            if(_returned) return;
            
            ReturnToPool?.Invoke(this);
            _returned = true;
        }

        public void OnSpawn(Vector3 target)
        {
            _returned = false;
            _spawnTime = Time.time;
            _rb.linearVelocity = (target - transform.position).normalized * _speed;
        }

        public void OnDespawn()
        {
            _rb.linearVelocity = Vector3.zero;
            if (_ignoreCollider)
            {
                Physics.IgnoreCollision(_ignoreCollider,  _collider, false);
                _ignoreCollider = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //ReturnSelf();
        }
        

        public void IgnoreOwnerCollision(Collider owner)
        {
            if (owner == null) return;
            
            _ignoreCollider = owner;
            Physics.IgnoreCollision(owner, _collider, true);
        }
}

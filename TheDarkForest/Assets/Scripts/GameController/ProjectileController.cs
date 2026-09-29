using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class ProjectileController : MonoBehaviour
    {
        //public static GameController Instance {get; private set;}

        [Header("Prefabs")]
        [SerializeField] private Projectile _playerShotPrefab;
        [SerializeField] private Projectile _enemyShotPrefab;
        [SerializeField] private Projectile _golemShotPrefab;

        [Header("References")]
        [SerializeField] internal PlayerMovementComponent _player;
        /*[SerializeField] private PlayerInputHandler _playerInput;*/

        [Header("Parents")]
        [SerializeField] private Transform _playerShotParent;
        [SerializeField] private Transform _enemyShotParent;
        [SerializeField] private Transform _golemShotParent;

        private ProjectilePool<Projectile> _playerShotPool;
        private ProjectilePool<Projectile> _enemyShotPool;
        private ProjectilePool<Projectile> _golemShotPool;
        
        public ProjectilePool<Projectile> EnemyShotPool => _enemyShotPool;
        public ProjectilePool<Projectile> GolemShotPool => _golemShotPool;

        private Coroutine _gameLoop;

        private void Awake()
        {
            InitializePools();
        }

        private void Start()
        {
            _player.GetComponent<PlayerCombat>().Init(_playerShotPool);
        }
        

        private void InitializePools()
        {
            _playerShotPool =
                new ProjectilePool<Projectile>(_playerShotPrefab, 20, _playerShotParent);

            _enemyShotPool = 
                new ProjectilePool<Projectile>(_enemyShotPrefab, 20, _enemyShotParent);
            
            _golemShotPool =
                new ProjectilePool<Projectile>(_golemShotPrefab, 20, _golemShotParent);
        }
    }



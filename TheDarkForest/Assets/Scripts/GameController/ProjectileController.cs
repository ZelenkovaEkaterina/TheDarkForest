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

        [Header("References")]
        [SerializeField] internal PlayerMovementComponent _player;
        /*[SerializeField] private PlayerInputHandler _playerInput;*/

        [Header("Parents")]
        [SerializeField] private Transform _playerShotParent;
        [SerializeField] private Transform _enemyShotParent;

        private ProjectilePool<Projectile> _playerShotPool;
        private ProjectilePool<Projectile> _enemyShotPool;

        private Coroutine _gameLoop;

        private void Awake()
        {
            InitializePools();
        }

        private void Start()
        {
            _player.Init(_playerShotPool);
        }
        

        private void InitializePools()
        {
            _playerShotPool =
                new ProjectilePool<Projectile>(_playerShotPrefab, 20, _playerShotParent);

            _enemyShotPool = 
                new ProjectilePool<Projectile>(_enemyShotPrefab, 20, _enemyShotParent);
        }
    }



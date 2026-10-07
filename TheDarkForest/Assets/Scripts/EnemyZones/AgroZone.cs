using System;
using Enemy;
using UnityEngine;

public class AgroZone : MonoBehaviour
{
    private EnemiesSpawn _spawn;
    [SerializeField] private GameController _gameController;

    private void Awake()
    {
        _spawn = GetComponent<EnemiesSpawn>();
    }

    private void OnEnable()
    {
        _gameController.OnPlayerDead += HandleDeadTarget;
    }

    private void OnDisable()
    {
        _gameController.OnPlayerDead -= HandleDeadTarget;
    }

    private void HandleDeadTarget()
    {
        foreach (var mob in _spawn._activeEnemies)
        {
            mob.GetComponent<EnemyController>().SetDeadTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var mob in _spawn._activeEnemies)
            {
                mob.GetComponent<EnemyController>().SetTarget(other.transform);
            }
        }
    }
}

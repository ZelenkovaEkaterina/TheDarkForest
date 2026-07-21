using System;
using Enemy;
using UnityEngine;

public class AgroZone : MonoBehaviour
{
    private EnemiesSpawn _spawn;

    private void Awake()
    {
        _spawn = GetComponent<EnemiesSpawn>();
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

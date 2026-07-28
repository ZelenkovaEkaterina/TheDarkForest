using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawn : MonoBehaviour
{
   public int Id;
   public List<GameObject> _activeEnemies = new List<GameObject>();
   
   [SerializeField] private SettingsAreaZone settingsAreaZone;
   [SerializeField] private PoolManager _poolManager;
   private GameObjectPool _pool;
   private RandomPointInZone _waypoints;

   private float _radius;
   private Vector3 _center;

   private void Start()
   {
      _pool = _poolManager.EnemyPool;
      _radius = GetComponent<SphereCollider>().radius;
      _center = GetComponent<SphereCollider>().center;
      
      _waypoints = GetComponent<RandomPointInZone>();
        
      SpawnEnemies();
   }

   private void SpawnEnemies()
   {
      ZoneSettings foundZone = GetZoneById(Id);
        
      if (foundZone == null)
      {
         Debug.LogWarning($"Зона с Id {Id} не найдена!");
         return;
      }

      int near = foundZone.NearEnemiesCount;
      int range = foundZone.RangeEnemiesCount;
      int totalEnemies = near + range;

      // Берем объекты из пула
      List<GameObject> enemies = _pool.GetEnemy(totalEnemies);
      _activeEnemies.AddRange(enemies);
      
      // Получаем точки патрулирования
      List<Vector3> patrolPoints = _waypoints.GetPatrolPoints();
        
      ConfigureEnemies(enemies, near, patrolPoints);
      
      //Debug.Log($"Зона {Id}: Спавнено {totalEnemies} врагов (Ближних: {near}, Дальних: {range})");
   }

   private ZoneSettings GetZoneById(int id)
   {
      foreach (var zone in settingsAreaZone.Settings.Items)
      {
         if (zone != null && zone.Id == id)
            return zone;
      }
      return null;
   }

   private void ConfigureEnemies(List<GameObject> enemies, int nearCount, List<Vector3> patrolPoints)
   {
      for (int i = 0; i < enemies.Count; i++)
      {
         GameObject enemy = enemies[i];
         enemy.transform.position = transform.position + GetRandomPointInSphere(_center, _radius);
      }

      int miliCounter = 0;

      for (int i = 0; i < enemies.Count; i++)
      {
         if (miliCounter <= nearCount-1 && nearCount != 0)
         {
            GameObject enemy = enemies[i];
            MiliEnemyAI miliEnemyScript = enemy.AddComponent<MiliEnemyAI>();
            EnemyAI enemyAI = enemy.AddComponent<EnemyAI>();
            if (miliEnemyScript != null && patrolPoints.Count > 0)
            {
               enemyAI.SetPatrolPoints(patrolPoints);
            }

            miliCounter++;
         }
         else
         {
            GameObject enemy = enemies[i];
            RangeEnemyAI rangeEnemyScript = enemy.AddComponent<RangeEnemyAI>();
            EnemyAI enemyAI = enemy.AddComponent<EnemyAI>();
            if (rangeEnemyScript != null && patrolPoints.Count > 0)
            {
               enemyAI.SetPatrolPoints(patrolPoints);
            }
         }

         
      }

      /*for (int i = 0; i < nearCount; i++) //добавляем скрипт ближних врагов
      {
         GameObject enemy = enemies[i];
         MiliEnemyAI miliEnemyScript = enemy.AddComponent<MiliEnemyAI>();
         EnemyAI enemyAI = enemy.AddComponent<EnemyAI>();
         if (miliEnemyScript != null && patrolPoints.Count > 0)
         {
            enemyAI.SetPatrolPoints(patrolPoints);
         }
      }

      for (int i = nearCount; i < enemies.Count; i++) //скрипт дальних
      {
         GameObject enemy = enemies[i];
         RangeEnemyAI rangeEnemyScript = enemy.AddComponent<RangeEnemyAI>();
         EnemyAI enemyAI = enemy.AddComponent<EnemyAI>();
         if (rangeEnemyScript != null && patrolPoints.Count > 0)
         {
            enemyAI.SetPatrolPoints(patrolPoints);
         }
      }*/

   }
   
   private Vector3 GetRandomPointInSphere(Vector3 center, float radius)
   {
      Vector3 randomDirection = Random.insideUnitSphere;
      float randomDistance = Random.Range(0f, radius);
      return center + randomDirection * randomDistance;
   }

   public void ReturnAllEnemies()
   {
      if (_pool == null) return;
        
      foreach (var enemy in _activeEnemies)
      {
         if (enemy != null)
            _pool.Return(enemy);
      }
      _activeEnemies.Clear();
   }

   private void OnDestroy()
   {
      ReturnAllEnemies();
   }
}

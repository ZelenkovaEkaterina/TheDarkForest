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
   private GameObjectPool _pool;
   
   private float _radius;
   private Vector3 _center;

   private void Start()
   {
      _pool = PoolManager.Instance.EnemyPool;
      _radius = GetComponent<SphereCollider>().radius;
      _center = GetComponent<SphereCollider>().center;
        
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
      
      
        
      ConfigureEnemies(enemies, near);
      AddScripts(enemies, near, range);
        
      Debug.Log($"Зона {Id}: Спавнено {totalEnemies} врагов (Ближних: {near}, Дальних: {range})");
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

   private void ConfigureEnemies(List<GameObject> enemies, int nearCount)
   {
      for (int i = 0; i < enemies.Count; i++)
      {
         GameObject enemy = enemies[i];
            
         /*float angle = (i / (float)enemies.Count) * 360f;
         enemy.transform.position = transform.position + 
                                    new Vector3(
                                       Mathf.Sin(angle * Mathf.Deg2Rad) * _radius, 
                                       transform.position.y, 
                                       Mathf.Cos(angle * Mathf.Deg2Rad) * _radius
                                    );*/
         enemy.transform.position = transform.position + GetRandomPointInSphere(_center, _radius);
      }
      
   }
   
   private Vector3 GetRandomPointInSphere(Vector3 center, float radius)
   {
      Vector3 randomDirection = Random.insideUnitSphere;
      float randomDistance = Random.Range(0f, radius);
      return center + randomDirection * randomDistance;
   }

   private void AddScripts(List<GameObject> enemies, int nearCount, int rangeCount)
   {
      for (int i = 0; i < nearCount; i++) //добавляем скрипт ближних врагов
      {
        GameObject enemy = enemies[i];
        //EnemyEventsHandler enemyEventsHandler = enemy.GetComponent<EnemyEventsHandler>();
        NearEnemyAI nearEnemyScript = enemy.GetComponent<NearEnemyAI>();

        if (nearEnemyScript == null)
         {
            //enemyEventsHandler = enemy.AddComponent<EnemyEventsHandler>();
            nearEnemyScript = enemy.AddComponent<NearEnemyAI>();
            Debug.Log($"Добавлен скрипт EnemyAI на {enemy.name}");
         }
      }

      for (int i = nearCount; i < rangeCount+nearCount; i++) //скрипт дальних
      {
         GameObject enemy = enemies[i];
         RangeEnemyAI enemyScript = enemy.GetComponent<RangeEnemyAI>();
         
         if (enemyScript == null)
         {
            enemyScript = enemy.AddComponent<RangeEnemyAI>();
            Debug.Log($"Добавлен скрипт MagicEnemyAI на {enemy.name}");
         }
      }
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

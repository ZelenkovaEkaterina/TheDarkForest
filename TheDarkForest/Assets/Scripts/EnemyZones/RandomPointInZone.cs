using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomPointInZone : MonoBehaviour
{
    private int _numberOfPoints = 5;
    private float minDistanceBetweenPoints = 1.5f;
    private float _radius;
    private Vector3 _center;
    
    [SerializeField]private int _countPoint = 5;
    public List<Vector3> _coordinates =  new List<Vector3>();
    
    private void Awake()
    {
        _radius = GetComponent<SphereCollider>().radius;
        _center = GetComponent<Transform>().position;
    }

    private void Start()
    {
        GenerateRandomPointInSphere();
    }

    private void GenerateRandomPointInSphere()
    {
        _coordinates.Clear();

        int spawnedCount = 0;
        int maxAttempts = 5000; // Увеличиваем лимит, т.к. поиск стал сложнее

        for (int i = 0; i < _numberOfPoints; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _radius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _radius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) > _radius)
                    continue;
                
                bool tooClose = false;
                foreach (Vector3 existingPoint in _coordinates)
                {
                    if (Vector3.Distance(hit.position, existingPoint) < minDistanceBetweenPoints)
                    {
                        tooClose = true;
                        break; // выходим из цикла, точка нам не подходит
                    }
                }

                // если слишком близко пропускаем точку
                if (tooClose)
                    continue;
                
                _coordinates.Add(hit.position);
                spawnedCount++;
            }
        }

        Debug.Log($"Спавнено {spawnedCount} точек с минимальной дистанцией {minDistanceBetweenPoints}.");
    }
    
    public List<Vector3> GetPatrolPoints()
    {
        return new List<Vector3>(_coordinates);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.blue;
        foreach (Vector3 pos in _coordinates)
        {
            Gizmos.DrawSphere(pos, 1f);
        }
    }
}

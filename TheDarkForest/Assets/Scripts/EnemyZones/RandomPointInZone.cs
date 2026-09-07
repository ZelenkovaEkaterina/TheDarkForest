using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomPointInZone : MonoBehaviour
{
    private int _countOfPoints = 10;
    private float minDistanceBetweenPoints = 3f;
    private float _radius;
    private Vector3 _center;
    
    //public List<Vector3> _coordinates =  new List<Vector3>();
    public Queue<Vector3> _coordinates =  new Queue<Vector3>();
    
    private void Awake()
    {
        _radius = GetComponent<SphereCollider>().radius;
        _center = GetComponent<SphereCollider>().center;
        
        GenerateRandomPointInSphere();
    }

    private void Start()
    {
        //GenerateRandomPointInSphere();
    }

    private void GenerateRandomPointInSphere()
    {
        _coordinates.Clear();

        int spawnedCount = 0;

        for (int i = 0; i < _countOfPoints; i++)
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
                
                _coordinates.Enqueue(hit.position);
                spawnedCount++;
            }
        }
        
        //Debug.Log($"Спавнено {spawnedCount} точек с минимальной дистанцией {minDistanceBetweenPoints}.");
    }
    
    public Queue<Vector3> GetPatrolPoints()
    {
        return _coordinates;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(_center, _radius);
        //Debug.Log(_center);
        //Gizmos.color = Color.blue;
        foreach (Vector3 pos in _coordinates)
        {
            Gizmos.DrawSphere(pos, 1f);
        }
    }
}

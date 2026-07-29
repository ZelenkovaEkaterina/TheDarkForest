using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 2f;
    public float waitTime = 7f;
    public float arrivalDistance = 0.5f;
    
    private List<Vector3> _points = new List<Vector3>();
    private int _currentIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;
    private NavMeshAgent _agent;
    
    public bool IsActive { get; private set; }

    private void FixedUpdate()
    {
        if (!IsActive || _points.Count == 0) return;
        
        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0)
            {
                _isWaiting = false;
                _agent.isStopped = false;
                GoToNextPoint();
            }
            return;
        }
        
        if (_agent.remainingDistance <= arrivalDistance && !_agent.pathPending)
        {
            // Если точка занята другим врагом, выбираем другую
            if (IsPointOccupied(_points[_currentIndex]))
            {
                Debug.Log("занято");
                _points.RemoveAt(_currentIndex);
                _currentIndex = 0;
                if (_points.Count == 0)
                {
                    StopPatrol();
                    return;
                }
                GoToNextPoint();
            }
            else
            {
                _isWaiting = true;
                _waitTimer = waitTime;
                _agent.isStopped = true;

                // Переход к следующей точке
                _currentIndex = (_currentIndex + 1) % _points.Count;
                //_currentIndex = Random.Range(0, _points.Count);
            }
        }
    }

    private void GoToNextPoint()
    {
        
        if (_points.Count == 0) return;
    
        // Получаем все свободные точки
        List<Vector3> freePoints = new List<Vector3>();
        foreach (var point in _points)
        {
            if (IsPointOccupied(point) == false) 
            {
                freePoints.Add(point);
            }
        }
        
    
        // Если есть свободные точки - выбираем случайную
        if (freePoints.Count > 0)
        {
            int randomIndex = Random.Range(0, freePoints.Count);
            Vector3 targetPoint = freePoints[randomIndex];
        
            _agent.speed = speed;
            _agent.SetDestination(targetPoint);
            _agent.isStopped = false;
        }
        else
        {
            // Все точки заняты - ждем
            _isWaiting = true;
            _waitTimer = waitTime;
            _agent.isStopped = true;
        }
    }

    public void SetPoints(List<Vector3> points)
    {
        _points = new List<Vector3>(points);
        _currentIndex = 0;
        _isWaiting = false;
        _waitTimer = 0;
        
        if (_points.Count > 0)
        {
            StartPatrol();
        }
    }

    public void StartPatrol()
    {
        if (_points.Count == 0) return;
        
        IsActive = true;
        _agent.isStopped = false;
        GoToNextPoint();
    }

    public void StopPatrol()
    {
        IsActive = false;
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public void SetAgent(NavMeshAgent agent)
    {
        _agent = agent;
    }
    
    private bool IsPointOccupied(Vector3 point)
    {
        // Проверяем, есть ли другие враги на этой точке
        Collider[] colliders = Physics.OverlapSphere(point, 1.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Enemy"))
                return true;
        }
        return false;
    }
}

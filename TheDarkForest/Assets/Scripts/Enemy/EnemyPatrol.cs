using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 2f;
    public float waitTime = 2f;
    public float arrivalDistance = 0.5f;
    
    private List<Vector3> _points = new List<Vector3>();
    private int _currentIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;
    private NavMeshAgent _agent;
    
    public bool IsActive { get; private set; }
    
    private void Update()
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
            _isWaiting = true;
            _waitTimer = waitTime;
            _agent.isStopped = true;
            
            // Переход к следующей точке
            _currentIndex = (_currentIndex + 1) % _points.Count;
        }
    }

    private void GoToNextPoint()
    {
        
        if (_points.Count == 0) return;
        
        _agent.speed = speed;
        _agent.SetDestination(_points[_currentIndex]);
        _agent.isStopped = false;
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
        Debug.Log("Starting patrol");
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

    public bool HasPoints()
    {
        return _points.Count > 0;
    }

    public void SetAgent(NavMeshAgent agent)
    {
        _agent = agent;
    }
}

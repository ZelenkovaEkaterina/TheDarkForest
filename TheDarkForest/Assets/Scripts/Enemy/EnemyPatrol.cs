using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 2f;
    public float waitTime = 7f;
    public float arrivalDistance = 0.5f;
    
    private Queue<Vector3> _sharedWaypoints;
    // Оригинальный список точек (для восстановления, если очередь вдруг опустеет)
    private List<Vector3> _originalPoints = new List<Vector3>();
    
    private Vector3 _currentPoint;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;
    private NavMeshAgent _agent;

    public bool IsActive { get; private set; }

    private void FixedUpdate()
    {
        if (!IsActive) return;
        
        if (_sharedWaypoints == null || _sharedWaypoints.Count == 0)
        {
            if (_originalPoints.Count > 0)
            {
                RestoreWaypoints();
            }
            else
            {
                StopPatrol();
                return;
            }
        }

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0)
            {
                _isWaiting = false;
                _agent.isStopped = false;
                
                if (_currentPoint != Vector3.zero)
                {

                        _sharedWaypoints.Enqueue(_currentPoint);
                        _currentPoint = Vector3.zero;
                }
                
                GoToNextPoint();
            }
            return;
        }
        
        if (_agent.remainingDistance <= arrivalDistance && !_agent.pathPending)
        {
            _isWaiting = true;
            _waitTimer = waitTime;
            _agent.isStopped = true;
        }
    }

    private void GoToNextPoint()
    {
        Vector3 nextPoint;

        if (_sharedWaypoints == null || _sharedWaypoints.Count == 0)
        {
            if (_originalPoints.Count > 0)
                RestoreWaypoints();
            else
            {
                StopPatrol();
                return;
            }
        }
            
        nextPoint = _sharedWaypoints.Dequeue();
        _currentPoint = nextPoint;
        
            
        _agent.speed = speed;
        _agent.SetDestination(nextPoint);
        _agent.isStopped = false; 
    }
    
    public void SetPoints(Queue<Vector3> sharedQueue)
    {
        _sharedWaypoints = sharedQueue;
        _originalPoints = new List<Vector3>(sharedQueue);
        
        _currentPoint = Vector3.zero;
        _isWaiting = false;
        _waitTimer = 0;

        if (_sharedWaypoints != null && _sharedWaypoints.Count > 0)
        {
            StartPatrol();
        }
    }

    public void StartPatrol()
    {
        if (_sharedWaypoints == null || _sharedWaypoints.Count == 0)
        {
            if (_originalPoints.Count > 0)
                RestoreWaypoints();
            else
                return;
        }

        IsActive = true;
        _agent.isStopped = false;
        GoToNextPoint();
    }

    public void StopPatrol()
    {
        IsActive = false;
        _agent.isStopped = true;
        _agent.ResetPath();

        if (_currentPoint != Vector3.zero)
        {
                _sharedWaypoints.Enqueue(_currentPoint);
                _currentPoint = Vector3.zero;
        }
    }

    public void SetAgent(NavMeshAgent agent)
    {
        _agent = agent;
    }
    
    private void RestoreWaypoints()
    {
        if (_originalPoints.Count == 0) return;

        List<Vector3> shuffled = new List<Vector3>(_originalPoints);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
            /*Vector3 temp = shuffled[i];
            shuffled[i] = shuffled[rand];
            shuffled[rand] = temp;*/
        }

        _sharedWaypoints.Clear();
        foreach (var p in shuffled) 
            _sharedWaypoints.Enqueue(p);
    }

}

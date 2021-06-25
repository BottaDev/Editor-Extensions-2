using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public float Speed = 5f;

    private int _currentPoint = 0;
    private AIPath _path;
    private NavMeshAgent _agent;
    
    private void Awake()
    {
        _path = GetComponent<AIPath>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _agent.speed = Speed;

        if (Vector3.Distance(_path.pathPositions[_currentPoint].position, transform.position) < _agent.stoppingDistance)
        {
            _currentPoint++;
            if (_currentPoint > _path.pathPositions.Count - 1)
                _currentPoint = 0;
        }
        
        _agent.destination = _path.pathPositions[_currentPoint].position;
    }
}

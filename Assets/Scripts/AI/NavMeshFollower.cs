using System;
using UniRx;
using UniRx.Triggers;
using UnitScripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI
{
    public sealed class NavMeshFollower : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _rotationThreshold = 3f;
        [SerializeField] private float _movementThreshold = 0.3f;

        [SerializeField, Range(5, 20)] private float _positionUpdateFrequency = 10;
        [SerializeField, Range(0, 50)] private float _radiusAroundPlayer = 20;
        
        private TankMover _tankMover;
        private Transform _playerTransform;

        private Vector3 TargetVector => new Vector3(_navMeshAgent.transform.position.x, 0, _navMeshAgent.transform.position.z);
        private float GetRandomAxes() => Random.Range(-_radiusAroundPlayer, _radiusAroundPlayer);

        private void Start()
        {
        }

        private void Update()
        {
            Rotate();
            Move();
        }

        private void Rotate()
        {
        }

        private void Move()
        {
        }

        private Vector3 GetRandomPosition()
        {
            throw new NotImplementedException();
        }
    }
}
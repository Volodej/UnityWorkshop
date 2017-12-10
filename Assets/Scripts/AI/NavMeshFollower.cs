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
            _tankMover = GetComponent<TankMover>();
            _playerTransform = FindObjectOfType<PlayerInput>().transform;
            var updatePositionSubscription = AIHelper.GetTimerStreamWithRandomStart(_positionUpdateFrequency)
                .Subscribe(_ => _navMeshAgent.destination = GetRandomPosition());
            GetComponent<UnitHealth>().HealthPercentageStream.Subscribe(_ => { }, () =>
            {
                Destroy(_navMeshAgent.gameObject);
                enabled = false;
                updatePositionSubscription.Dispose();
            });
            _navMeshAgent.speed = _tankMover.MovementSpeed;
            
            gameObject.OnDestroyAsObservable().Subscribe(_ => updatePositionSubscription.Dispose());
        }

        private void Update()
        {
            Rotate();
            Move();
        }

        private void Rotate()
        {
            var forwardVector = transform.rotation * Vector3.forward;
            var directionVector = (TargetVector - transform.position).normalized;

            var angle = AIHelper.GetRotationAngle(forwardVector, directionVector);
            
            var turnFactor = Mathf.Abs(angle) > _rotationThreshold ? Mathf.Sign(angle) : 0;
            _tankMover.Turn(turnFactor);
        }

        private void Move()
        {
            var moveVector = TargetVector - transform.position;
            var localMoveVector = Quaternion.Inverse(transform.rotation) * moveVector;
            var forwardMovement = localMoveVector.z;

            var speedFactor = Mathf.Clamp(forwardMovement / _tankMover.MovementSpeed, -1, 1);
            var resultSpeedFactor = Mathf.Abs(forwardMovement) > _movementThreshold ? speedFactor : 0;

            _tankMover.Move(resultSpeedFactor);
        }

        private Vector3 GetRandomPosition()
        {
            var randomPoint = new Vector3(GetRandomAxes(), 0, GetRandomAxes());
            var randomPosition = _playerTransform.position + randomPoint;
            
            NavMeshHit navMeshHit;
            var found = NavMesh.SamplePosition(randomPosition, out navMeshHit, _radiusAroundPlayer, ~0);
            
            return !found ? GetRandomPosition() : navMeshHit.position;
        }
    }
}
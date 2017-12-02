using Interfaces;
using UnityEngine;

namespace UnitScripts
{
    public sealed class TankMover : MonoBehaviour, IMovable
    {
        #region Prepared

        [SerializeField] private AudioSource _movementAudioSource;
        [SerializeField] private AudioClip _engineIdling;
        [SerializeField] private AudioClip _engineDriving;
        [SerializeField] private float _pitchRange = 0.2f;

        [SerializeField] private float _turnSpeed = 60;
        [SerializeField] private float _maxMovementSpeed = 10;
        [SerializeField] private float _speedFactorAcceleration = 2;
        [SerializeField] private float _speedFactorBrake = 5;

        private float _originalPitch;
        private Rigidbody _rigidbody;
        private float _currentTurnFactor;
        private float _currentSpeedFactor;
        private float _targetSpeedFactor;

        public float TurnSpeed => _turnSpeed;
        public float MovementSpeed => _maxMovementSpeed;

        private void Awake()
        {
            _originalPitch = _movementAudioSource.pitch;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateSpeedFactor();
            UpdateEngineAudio();
        }

        private void UpdateSpeedFactor()
        {
            // Update speed to target value
            var factorAcceleration = _speedFactorAcceleration * Time.deltaTime;
            var factorBrake = _speedFactorBrake * Time.deltaTime;
            var factorSignIsCorrect = Mathf.Abs(Mathf.Sign(_currentSpeedFactor) - Mathf.Sign(_targetSpeedFactor)) < 0.001f && _targetSpeedFactor != 0;
            var speedFactorBrakeDelta = factorSignIsCorrect
                ? 0
                : -Mathf.Clamp(_currentSpeedFactor, -factorBrake, factorBrake);
            var speedFactorAccelerationDelta = Mathf.Clamp(_targetSpeedFactor - _currentSpeedFactor, -factorAcceleration, factorAcceleration);
            var maxDelta = Mathf.Max(Mathf.Abs(speedFactorBrakeDelta), Mathf.Abs(speedFactorAccelerationDelta));
            var totalDelta = Mathf.Clamp(speedFactorBrakeDelta + speedFactorAccelerationDelta, -maxDelta, maxDelta);
            _currentSpeedFactor = _currentSpeedFactor + totalDelta;
        }

        #endregion
        
        public void Move(float targetFactor)
        {
            // Set target value for tank movement factor
            _targetSpeedFactor = targetFactor;
        }

        public void Turn(float targetFactor)
        {
            // Set value for tank turn factor
            _currentTurnFactor = targetFactor;
        }

        public void Disable()
        {
            // Disable physics after unit destruction
            _rigidbody.isKinematic = true;
            enabled = false;
        }

        private void UpdateEngineAudio()
        {
            // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
            var correctAudio = (Mathf.Abs(_currentTurnFactor) < 0.1f && Mathf.Abs(_currentSpeedFactor) < 0.1f)
                ? _engineIdling
                : _engineDriving;

            if (_movementAudioSource.clip != correctAudio)
            {
                _movementAudioSource.clip = correctAudio;
                _movementAudioSource.pitch = Random.Range(_originalPitch - _pitchRange, _originalPitch + _pitchRange);
                _movementAudioSource.Play();
            }
        }

        private void FixedUpdate()
        {
            // Move and turn the tank.
            Move();
            Turn();
        }

        private void Move()
        {
            // Adjust the position of the tank based on the player's input.
            var movement = transform.forward * _currentSpeedFactor * _maxMovementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        private void Turn()
        {
            // Adjust the rotation of the tank based on the player's input.
            var turn = _currentTurnFactor * _turnSpeed * Time.fixedDeltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }
    }
}
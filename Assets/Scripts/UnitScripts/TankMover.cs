using UniRx;
using UnityEngine;

namespace UnitScripts
{
    public sealed class TankMover : MonoBehaviour
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

        private void Start()
        {
            GetComponent<UnitHealth>().HealthPercentageStream.Subscribe(_ => { }, () => enabled = false);
        }
        
        public void Move(float targetFactor)
        {
            // Set target value for tank movement factor
        }

        public void Turn(float targetFactor)
        {
            // Set value for tank turn factor
        }

        public void Disable()
        {
            // Disable physics after unit destruction
        }

        private void UpdateEngineAudio()
        {
            // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
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
        }

        private void Turn()
        {
            // Adjust the rotation of the tank based on the player's input.
        }
    }
}
using UniRx;
using UniRx.Triggers;
using UnitScripts;
using UnityEngine;

namespace AI
{
    public class AIShooter : MonoBehaviour
    {
        [SerializeField] private float _shootingFrequencity = 5f;
        [SerializeField] private float _distanceToPlayerToFire = 20f;
        [SerializeField] private float _rotationThreshold = 1f;
        [SerializeField] private Transform _fireTransform;
        
        [SerializeField] private float _minimalForce = 10f;
        [SerializeField] private float _maximalForce = 20;
        
        
        private Aimer _aimer;
        private Shooter _shooter;
        private Transform _playerTransform;

        private float DistanceToPlayer => (transform.position - _playerTransform.position).magnitude;
        private bool PlayerIsNear => DistanceToPlayer <= _distanceToPlayerToFire;

        private void Start()
        {
            _aimer = GetComponent<Aimer>();
            _shooter = GetComponent<Shooter>();
            _playerTransform = FindObjectOfType<PlayerInput>().transform;
                
            var shootingSubscription = AIHelper.GetTimerStreamWithRandomStart(_shootingFrequencity)
                .Where(_ => PlayerIsNear)
                .Subscribe(_ => Fire());

            GetComponent<UnitHealth>().HealthPercentageStream.Subscribe(_ => { }, () =>
            {
                shootingSubscription.Dispose();
                enabled = false;
            });

            gameObject.OnDestroyAsObservable().Subscribe(_ => shootingSubscription.Dispose());
        }

        private void Update()
        {
            Aim();
        }

        private void Fire()
        {
            var distanceToPlayer = DistanceToPlayer;
            var fireForce = (distanceToPlayer / _distanceToPlayerToFire) * (_maximalForce - _minimalForce) + _minimalForce;
            _shooter.Fire(fireForce);
        }

        private void Aim()
        {
            var aimVector = Vector3.ProjectOnPlane(_fireTransform.forward, Vector3.up);
            var directionVector = (_playerTransform.position - transform.position).normalized;
            var angle = AIHelper.GetRotationAngle(aimVector, directionVector);
            
            var aimFactor = Mathf.Abs(angle) > _rotationThreshold ? Mathf.Sign(angle) : 0;
            _aimer.Aim(aimFactor);
        }
    }
}
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
        }

        private void Update()
        {
            Aim();
        }

        private void Fire()
        {
        }

        private void Aim()
        {
        }
    }
}
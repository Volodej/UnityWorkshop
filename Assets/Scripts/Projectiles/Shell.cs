using System.Linq;
using UnitScripts;
using UnityEngine;

namespace Projectiles
{
    public class Shell : MonoBehaviour // TODO: split to shell and Explosion
    {
        [SerializeField] private LayerMask _unitsMask;                  // Used to filter what the explosion affects, this should be set to "Players".
        [SerializeField] private ParticleSystem _explosionParticles;    // Reference to the particles that will play on explosion.
        [SerializeField] private AudioSource _explosionAudio;           // Reference to the audio that will play on explosion.
        [SerializeField] private float _maxSplashDamage = 60f;          // The amount of damage done if the explosion is centred on a unit.
        [SerializeField] private float _hitDamage = 50f;                // The amount of damage done if shell hits a unit.
        [SerializeField] private float _explosionForce = 1000f;         // The amount of force added to a tank at the centre of the explosion.
        [SerializeField] private float _maxLifeTime = 2f;               // The time in seconds before the shell is removed.
        [SerializeField] private float _explosionRadius = 5f;           // The maximum distance away from the explosion tanks can be and are still affected.

        private void Start()
        {
            Destroy(gameObject, _maxLifeTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var hitUnit = other.GetComponent<UnitHealth>();
            if (hitUnit != null)
                ApplyHitToUnit(hitUnit);

            Physics.OverlapSphere(transform.position, _explosionRadius, _unitsMask)
                .Select(c => c.GetComponent<UnitHealth>())
                .ToList()
                .ForEach(ApplySplashToUnit);

            ShowHitEffects();
        }

        private void ShowHitEffects()
        {
            _explosionParticles.transform.parent = null;
            _explosionParticles.Play();
            _explosionAudio.Play();
            Destroy(_explosionParticles.gameObject, _explosionParticles.main.duration);
            Destroy(gameObject);
        }

        private void ApplyHitToUnit(UnitHealth targetHealth)
        {
            targetHealth.TakeDamage(_hitDamage);
        }

        private void ApplySplashToUnit(UnitHealth targetHealth)
        {
            var targetRigidbody = targetHealth.GetComponent<Rigidbody>();
            if (targetRigidbody)
                targetRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

            var damage = CalculateDamage(targetHealth.transform.position);
            targetHealth.TakeDamage(damage);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            var explosionToTarget = targetPosition - transform.position;
            var explosionDistance = explosionToTarget.magnitude;
            var relativeDistance = (_explosionRadius - explosionDistance) / _explosionRadius;
            var damage = Mathf.Max(0f, relativeDistance * _maxSplashDamage);
            return damage;
        }
    }
}
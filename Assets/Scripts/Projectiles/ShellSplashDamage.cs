using Smooth.Algebraics;
using Smooth.Slinq;
using UnitScripts;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Shell))]
    public class ShellSplashDamage : MonoBehaviour
    {
        #region Prepared

        [SerializeField] private LayerMask _explosionMask;              // Used to filter what the explosion affects.
        [SerializeField] private float _maxSplashDamage = 60f;          // The amount of damage done if the explosion is centered on a unit.
        [SerializeField] private float _explosionForce = 1000f;         // The amount of force added to a tank at the center of the explosion.
        [SerializeField] private float _explosionRadius = 5f;           // The maximum distance away from the explosion tanks can be and are still affected.

        #endregion

        private void Start()
        {
            GetComponent<Shell>().ShellHit += CreateSplash;
        }
        
        private void CreateSplash()
        {
            var explosionObjects = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionMask);
            explosionObjects.Slinq()
                .Select(c => c.GetComponent<Rigidbody>())
                .Where(r => r != null)
                .ForEach(ApplyForce);

            explosionObjects.Slinq()
                .SelectMany(c => c.GetComponent<UnitHealth>().ToOption())
                .ForEach(ApplySplashToUnit);
        }

        private void ApplyForce(Rigidbody targetRigidbody)
        {
            targetRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }

        private void ApplySplashToUnit(UnitHealth targetHealth)
        {
            var damage = CalculateDamage(targetHealth.transform.position);
            targetHealth.ApplyDamage(damage);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            // Calculate the distance from the shell to the target.
            var explosionToTarget = targetPosition - transform.position;
            var explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away (not less than 0).
            var relativeDistance = (_explosionRadius - explosionDistance) / _explosionRadius;
            var damage = Mathf.Max(0f, relativeDistance * _maxSplashDamage);
            return damage;
        }
    }
}
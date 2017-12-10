using System;
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
        }
        
        private void CreateSplash()
        {
        }

        private void ApplyForce(Rigidbody targetRigidbody)
        {
        }

        private void ApplySplashToUnit(UnitHealth targetHealth)
        {
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            throw new NotImplementedException();
        }
    }
}
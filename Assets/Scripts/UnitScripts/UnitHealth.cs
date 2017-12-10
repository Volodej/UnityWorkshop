using Projectiles;
using Smooth.Slinq;
using UniRx;
using UnityEngine;

namespace UnitScripts
{
    public class UnitHealth : MonoBehaviour
    {
        #region Prepared

        public IObservable<float> HealthPercentageStream { get; private set; }
        public bool IsAlive => _health.Value > 0;

        private BehaviorSubject<float> _health;

        [SerializeField] private float _startingHealth = 100f;              // The amount of health each tank starts with.
        [SerializeField] private float _deathColorFogging = 0.4f;           // Unit will be darkening for this value after death.
        [SerializeField] private Explosion _deathExplosion;                 // Explosion that will be created after unit death.

        #endregion

        private void Awake()
        {
        }

        public void ApplyDamage(float amount)
        {
            // Calculate new health (not less than 0)
        }

        private void OnDeath()
        {
        }
    }
}
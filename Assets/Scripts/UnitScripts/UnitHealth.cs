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

        private ReactiveProperty<float> _health;

        [SerializeField] private float _startingHealth = 100f;              // The amount of health each tank starts with.
        [SerializeField] private float _deathColorFogging = 0.7f;           // Unit will be darkening for this value after death.
        [SerializeField] private Explosion _deathExplosion;                 // Explosion that will be created after unit death.

        #endregion

        private void Awake()
        {
            _health = new ReactiveProperty<float>(_startingHealth);
            _health.DoOnCompleted(OnDeath);
            HealthPercentageStream = _health.Select(currentHealth => currentHealth / _startingHealth);
        }

        public void ApplyDamage(float amount)
        {
            // Calculate new health (not less than 0)
            var newHealth = _health.Value - amount;
            _health.Value = Mathf.Max(0, newHealth);

            if (newHealth <= 0)
                _health.Dispose();
        }

        private void OnDeath()
        {
            _deathExplosion.Activate();
            GetComponentsInChildren<MeshRenderer>().Slinq()
                .Select(meshRenderer => meshRenderer.material)
                .ForEach(material => material.color = material.color * _deathColorFogging);
        }
    }
}
using Smooth.Delegates;
using UnitScripts;
using UnityEngine;

namespace Projectiles
{
    public class Shell : MonoBehaviour
    {
        #region Prepared

        [SerializeField] private Explosion _explosion;                  // Reference to the component with explosion particles and sound
        [SerializeField] private float _hitDamage = 50f;                // The amount of damage done if shell hits a unit.
        [SerializeField] private float _maxLifeTime = 2f;               // The time in seconds before the shell is removed.

        #endregion

        public event DelegateAction ShellHit = () => { };

        private void Start()
        {
            Destroy(gameObject, _maxLifeTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var hitUnit = other.GetComponent<UnitHealth>();
            if (hitUnit != null)
                ApplyHitToUnit(hitUnit);

            _explosion.Activate();
            ShellHit();

            Destroy(gameObject);
        }

        private void ApplyHitToUnit(UnitHealth targetHealth)
        {
            targetHealth.ApplyDamage(_hitDamage);
        }
    }
}
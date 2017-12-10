using UniRx;
using UnityEngine;

namespace UnitScripts
{
    public sealed class Aimer : MonoBehaviour
    {
        #region Prepared
        
        public IObservable<float> AimRotation => _currentRotation;

        [SerializeField] private float _aimSpeed = 60;
        [SerializeField] private Transform _aimModule;

        private float _currentAimFactor;
        private ReactiveProperty<float> _currentRotation;
        
        #endregion

        public void Aim(float targetFactor)
        {
        }

        private void Update()
        {
        }

        private void Awake()
        {
        }

        private void Start()
        {
        }
    }
}
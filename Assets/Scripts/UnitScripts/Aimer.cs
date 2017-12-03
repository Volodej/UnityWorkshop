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
            _currentAimFactor = targetFactor;
        }

        private void Update()
        {
            var turn = _currentAimFactor * _aimSpeed * Time.deltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            _aimModule.localRotation = _aimModule.localRotation * turnRotation;
            _currentRotation.Value = _aimModule.localRotation.eulerAngles.y;
        }

        private void Awake()
        {
            _currentRotation = new ReactiveProperty<float>(_aimModule.localRotation.eulerAngles.y);
        }

        private void Start()
        {
            GetComponent<UnitHealth>().HealthPercentageStream.Subscribe(_ => { }, () => enabled = false);
        }
    }
}
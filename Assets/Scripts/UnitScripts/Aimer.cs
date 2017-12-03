using UniRx;
using UnityEngine;

namespace UnitScripts
{
    public sealed class Aimer : MonoBehaviour
    {
        public IObservable<Quaternion> AimRotation => _currentRotation;

        [SerializeField] private float _aimSpeed = 60;
        [SerializeField] private Transform _aimModule;

        private float _currentAimFactor;
        private ReactiveProperty<Quaternion> _currentRotation;

        public void Aim(float targetFactor)
        {
            _currentAimFactor = targetFactor;
        }

        private void Update()
        {
            var turn = _currentAimFactor * _aimSpeed * Time.deltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            _aimModule.localRotation = _aimModule.localRotation * turnRotation;
        }

        private void Awake()
        {
            _currentRotation = new ReactiveProperty<Quaternion>(_aimModule.localRotation);
        }

        private void Start()
        {
            GetComponent<UnitHealth>().HealthPercentageStream.DoOnCompleted(() => enabled = false);
        }
    }
}
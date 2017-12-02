using Interfaces;
using UnityEngine;

namespace UnitScripts
{
    public sealed class Aimer : MonoBehaviour, IAimable
    {
        [SerializeField] private float _aimSpeed = 60;
        [SerializeField] private Transform _aimModule;

        private float _currentAimFactor;
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
    }
}
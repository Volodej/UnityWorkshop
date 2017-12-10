using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Tuple = Smooth.Algebraics.Tuple;

namespace UnitScripts
{
    public class ShootingCharger : MonoBehaviour
    {

        #region Prepared

        private enum ChargingState
        {
            StartCharging,
            ContinueCharging,
            StopCharging,
            NotCharging
        }

        private struct ChargingFrameData
        {
            public readonly ChargingState ChargingState;
            public readonly float ChargingTime;

            public static ChargingFrameData StartCharging(float frameTime) => new ChargingFrameData(ChargingState.StartCharging, frameTime);
            public static ChargingFrameData StopCharging(float resultTime) => new ChargingFrameData(ChargingState.StopCharging, resultTime);
            public static ChargingFrameData ContinueCharging(float chargingTime) => new ChargingFrameData(ChargingState.ContinueCharging, chargingTime);
            public static ChargingFrameData NotCharging() => new ChargingFrameData(ChargingState.NotCharging, 0f);


            private ChargingFrameData(ChargingState chargingState, float chargingTime)
            {
                ChargingState = chargingState;
                ChargingTime = chargingTime;
            }
        }

        public UniRx.IObservable<float> ChargingForceStream => _chargingForce;

        private readonly Subject<bool> _chargingStream = new Subject<bool>();
        private readonly Subject<float> _chargingForce = new Subject<float>();

        /// <summary>
        /// How fast the launch force increases, based on the max charge time.
        /// </summary>
        private float ChargingSpeed => (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;

        /// <summary>
        /// Get launch force value, based on charging time.
        /// </summary>
        private float CalculateChargingForce(float chargingTime) => chargingTime * ChargingSpeed + _minLaunchForce; 

        [SerializeField] private AudioSource _chargingAudio;    // Reference to the audio source used to play the shooting audio.
        [SerializeField] private float _minLaunchForce = 15f;   // The force given to the shell if the fire button is not held.
        [SerializeField] private float _maxLaunchForce = 30f;   // The force given to the shell if the fire button is held for the max charge time.
        [SerializeField] private float _maxChargeTime = 0.75f;  // How long the shell can charge for before it is fired at max force.

        #endregion

        public void StartShotCharging()
        {
        }

        public void EndShotCharging()
        {
        }

        private void Start()
        {
            // State stream
            
            // Start playing charging audio on start of charging.

            // And stop playing when charging is finished.

            // Calculate shooting force and fire when charging is finished.

            // Stop charging when charge at maximum value.

            // Create stream with current charge force and subscribe _chargingForce stream for this events.

            // Subscribe for tank death - disable charging and set component off
        }

        private ChargingFrameData GetCurrentChargingState(ChargingFrameData lastData, float frameTime, bool currentlyCharging)
        {
            throw new NotImplementedException();
        }
        
        private void PlayChargingAudio()
        {
        }

        private void StopChargingAudio()
        {
        }
    }
}
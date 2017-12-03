using System;
using UniRx;
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
            _chargingStream.OnNext(true);
        }

        public void EndShotCharging()
        {
            _chargingStream.OnNext(false);
        }

        private void Start()
        {
            // State stream
            var chargingStateStream = Observable.EveryUpdate()
                .Select(_ => Time.deltaTime)
                .CombineLatest(_chargingStream.StartWith(false), Tuple.Create)
                .Scan(ChargingFrameData.NotCharging(),
                    (lastData, timeAndIsCharging) =>
                        GetCurrentChargingState(lastData, timeAndIsCharging.Item1, timeAndIsCharging.Item2));
            
            // Start playing charging audio on start of charging.
            chargingStateStream
                .Where(data => data.ChargingState == ChargingState.StartCharging)
                .Subscribe(_ => PlayChargingAudio());

            // And stop playing when charging is finished.
            chargingStateStream
                .Where(data => data.ChargingState == ChargingState.StopCharging)
                .Subscribe(_ => StopChargingAudio());

            // Calculate shooting force and fire when charging is finished.
            var shooter = GetComponent<Shooter>();
            chargingStateStream
                .Where(data => data.ChargingState == ChargingState.StopCharging)
                .Select(data => CalculateChargingForce(data.ChargingTime))
                .Select(chargeForce => Mathf.Min(chargeForce, _maxLaunchForce))
                .Subscribe(chargeForce => shooter.Fire(chargeForce));

            // Stop charging when charge at maximum value.
            chargingStateStream
                .Where(data => data.ChargingState == ChargingState.ContinueCharging && data.ChargingTime > _maxChargeTime)
                .Subscribe(_ => _chargingStream.OnNext(false));

            // Create stream with current charge force and subscribe _chargingForce stream for this events.
            chargingStateStream
                .Select(data => data.ChargingState == ChargingState.ContinueCharging ? data.ChargingTime : 0f)
                .Select(CalculateChargingForce)
                .StartWith(0)
                .Subscribe(_chargingForce);

        }

        private ChargingFrameData GetCurrentChargingState(ChargingFrameData lastData, float frameTime, bool currentlyCharging)
        {
            switch (lastData.ChargingState)
            {
                case ChargingState.StartCharging:
                case ChargingState.ContinueCharging:
                    return currentlyCharging
                        ? ChargingFrameData.ContinueCharging(frameTime + lastData.ChargingTime)
                        : ChargingFrameData.StopCharging(lastData.ChargingTime);

                case ChargingState.NotCharging:
                case ChargingState.StopCharging:
                    return currentlyCharging
                        ? ChargingFrameData.StartCharging(frameTime)
                        : ChargingFrameData.NotCharging();

                default:
                    throw new ArgumentOutOfRangeException(nameof(lastData.ChargingState), lastData.ChargingState, null);
            }
        }
        
        private void PlayChargingAudio()
        {
            _chargingAudio.Play();
        }

        private void StopChargingAudio()
        {
            _chargingAudio.Stop();
        }
    }
}
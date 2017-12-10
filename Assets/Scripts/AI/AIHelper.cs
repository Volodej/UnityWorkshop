using System;
using Smooth.Algebraics;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public static class AIHelper
    {
        public static float GetRotationAngle(Vector3 lookVector, Vector3 targetVector)
        {
            var angleCos = Vector3.Dot(targetVector, lookVector) / (targetVector.magnitude * lookVector.magnitude);
            var angle = Mathf.Acos(angleCos);

            var angleSign = Mathf.Sign(targetVector.x * lookVector.z - targetVector.z * lookVector.x);
            var resultAngle = Mathf.Rad2Deg * angle * angleSign;

            return resultAngle;
        }

        public static UniRx.IObservable<Unit> GetTimerStreamWithRandomStart(float timeInterval) =>
            Observable.Timer(TimeSpan.FromSeconds(Random.Range(0, timeInterval)), TimeSpan.FromSeconds(timeInterval)).AsUnitObservable();
    }
}
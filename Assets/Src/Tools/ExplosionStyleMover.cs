﻿#region imports

using System;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

namespace GlobalPlay.Tools
{
    public class ExplosionStyleMover : MonoBehaviour
    {
        private float actualAngle;
        private float actualRadius;
        public float Angle = 60f;
        public float AngleUniformOffset = 10f;
        private Vector3 centerPoint;
        private Vector3 centerToStartHandle, centerToEndHandle;
        private Vector3 endHandle;
        public Vector3 EndPoint;
        public AnimationCurve ExplosionCurve;
        public AnimationCurve ImplosionCurve;
        private Vector3 prevPos;
        public float Radius = 1f;
        public float RadiusUniformOffset = 0.1f;
        private Vector3 startHandle;
        public Vector3 StartPoint;
        public float TimeStretch = 0.1f;
        public float TotalTime = 2f;

        public void Start()
        {
            actualRadius = Radius + Random.Range(-RadiusUniformOffset, RadiusUniformOffset);
            actualAngle = Angle + Random.Range(-AngleUniformOffset, AngleUniformOffset);
            var defaultZ = StartPoint.z;
            centerPoint = new Vector3(StartPoint.x + actualRadius*Mathf.Cos(actualAngle*Mathf.Deg2Rad),
                StartPoint.y + actualRadius*Mathf.Sin(actualAngle*Mathf.Deg2Rad), defaultZ);

            var centerToEnd = (centerPoint - EndPoint)/5;
            var rightCenterToEnd = new Vector3(centerToEnd.y, -centerToEnd.x);
            var leftCenterToEnd = -rightCenterToEnd;

            endHandle = EndPoint - ((EndPoint - StartPoint)/5);
            centerToEndHandle = centerPoint + rightCenterToEnd;
            centerToStartHandle = centerPoint + leftCenterToEnd;
            startHandle = StartPoint - (StartPoint - centerToStartHandle)/5;

            transform.position = StartPoint;

            LeanTween.move(
                gameObject,
                new[] {StartPoint, centerToStartHandle, startHandle, centerPoint},
                TotalTime/2).setEase(ExplosionCurve);
            LeanTween.move(gameObject, new[] {centerPoint, endHandle, centerToEndHandle, EndPoint}, TotalTime/2)
                .setOnComplete(OnMovingComplete)
                .setDelay(TotalTime/2)
                .setEase(ImplosionCurve);
        }

        private void Update()
        {
            Debug.DrawLine(prevPos, transform.position, Color.magenta, 2);
            prevPos = transform.position;
        }

        private void OnMovingComplete()
        {
            //Let the explosion play a little
            LeanTween.delayedCall(0.2f, FinishedMoving);

            //For debugging
//            LeanTween.delayedCall(2, this.Start);
        }

        public void OnDrawGizmos()
        {
            //first line
            Gizmos.color = new Color(1, 1, 1);
            Gizmos.DrawLine(StartPoint, centerPoint);
            //first line handles
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(StartPoint, startHandle);
            Gizmos.DrawLine(centerPoint, centerToStartHandle);

            //secondline
            Gizmos.color = new Color(1, 1, 1);
            Gizmos.DrawLine(centerPoint, EndPoint);

            //handles
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawLine(centerPoint, centerToEndHandle);
            Gizmos.DrawLine(EndPoint, endHandle);
        }

        public event Action FinishedMoving = delegate { };
    }
}
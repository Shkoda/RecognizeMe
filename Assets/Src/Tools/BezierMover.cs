#region imports

using System;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace GlobalPlay.Tools
{
    public class BezierMover : MonoBehaviour
    {
        private float actualAngle;
        private float actualRadius;
        private Vector3 centerToStartHandle, centerToEndHandle;
        private Vector3 endHandle;
        public Vector3 EndPoint;
        private Vector3 prevPos;
        private Vector3 startHandle;
        public Vector3 StartPoint;
        public float TotalTime = 3f;

        public void Start()
        {
            startHandle = StartPoint + Vector3.up*2;
            endHandle = EndPoint + Vector3.up*2;

            transform.position = StartPoint;

            LeanTween.move(
                gameObject,
                new[] {StartPoint, endHandle, startHandle, EndPoint},
                TotalTime).setEase(LeanTweenType.linear).setOnComplete(OnMovingComplete);
        }

        [UsedImplicitly]
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

        [UsedImplicitly]
        public void OnDrawGizmos()
        {
            //first line
            //first line handles
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(StartPoint, startHandle);

            //handles
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawLine(EndPoint, endHandle);
        }

        public event Action FinishedMoving = delegate { };
    }
}
namespace GlobalPlay.Tools
{
    using System;

    using JetBrains.Annotations;

    using UnityEngine;
    
    public class BezierMover : MonoBehaviour
    {
        private float actualRadius;

        private float actualAngle;

        public Vector3 StartPoint;
        public Vector3 EndPoint;

        public float TotalTime = 3f;
        
        private Vector3 startHandle;
        private Vector3 centerToStartHandle, centerToEndHandle;
        private Vector3 endHandle;
        private Vector3 prevPos;
        
        public void Start()
        {
            this.startHandle = StartPoint + Vector3.up*2;
            this.endHandle = EndPoint + Vector3.up*2;

            this.transform.position = this.StartPoint;

            LeanTween.move(
                this.gameObject,
                new[] { this.StartPoint, this.endHandle, this.startHandle, this.EndPoint },
                this.TotalTime).setEase(LeanTweenType.linear).setOnComplete(this.OnMovingComplete);
        }

        [UsedImplicitly]
        private void Update()
        {
            Debug.DrawLine(this.prevPos, this.transform.position, Color.magenta, 2);
            this.prevPos = this.transform.position;
        }

        private void OnMovingComplete()
        {
            //Let the explosion play a little
            LeanTween.delayedCall(0.2f, this.FinishedMoving);

            //For debugging
            //            LeanTween.delayedCall(2, this.Start);
        }

        [UsedImplicitly]
        public void OnDrawGizmos()
        {
            //first line
            //first line handles
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(this.StartPoint, this.startHandle);

            //handles
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawLine(this.EndPoint, this.endHandle);
        }

        public event Action FinishedMoving = delegate { };
    }
}
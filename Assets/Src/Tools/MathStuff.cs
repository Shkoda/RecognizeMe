namespace GlobalPlay.Tools
{
    using UnityEngine;

    static internal class MathStuff
    {
        public static Vector3 RayPlaneIntersection(Vector3 planePosition, Ray ray, Vector3 planeNormal)
        {
            var distOnLine = Vector3.Dot(planePosition - ray.origin, planeNormal)
                             / Vector3.Dot(ray.direction, planeNormal);
            var pointOnGround = ray.origin + ray.direction * distOnLine;
            return pointOnGround;
        }
    }
}
using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class PlaneIntersection
    {
        public static bool Intersect(Ray ray, PotatoPlane plane, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        {
            double denom = Vector3.Dot(ray.Direction, plane.Normal);
            if (Math.Abs(denom) > Constants.EPSILON)
            {
                double t = Vector3.Dot(plane.Position - ray.Origin, plane.Normal) / denom;
                if (t > 0.0)
                {
                    distance = t;
                    hitPosition = ray.Origin + ray.Direction * distance;
                    hitNormal = plane.Normal;
                    return true;
                }
            }
            return false;
        }
    }
}

using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class SphereIntersection
    {
        public static bool Intersect(Ray ray, PotatoSphere sphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        {
            Vector3 l = sphere.Position - ray.Origin;
            double tca = Vector3.Dot(l, ray.Direction);
            if (tca < 0) return false;
            double d2 = Vector3.Dot(l, l) - tca * tca;
            if (d2 > (sphere.Radius * sphere.Radius)) return false;
            double thc = Math.Sqrt(sphere.Radius * sphere.Radius - d2);
            double t0 = tca - thc;
            double t1 = tca + thc;

            if (t0 > t1)
            {
                double temp = t0;
                t0 = t1;
                t1 = temp;
            }

            if (t0 < 0)
            {
                t0 = t1;
                if (t0 < 0) return false;
            }

            distance = t0;
            hitPosition = ray.Cast(ray.Origin, distance);
            hitNormal = Vector3.Normalize(Vector3.Subtract(hitPosition, sphere.Position));

            return true;
        }
    }
}

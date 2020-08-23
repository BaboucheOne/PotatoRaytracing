using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class SphereIntersection
    {
        //TODO: Integrer le point de sortie.
        //TODO: It's working but let's test wich is better !!!
        public static bool Intersect(Ray ray, PotatoSphere sphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        {
            Vector3 l = sphere.Position - ray.Origin;
            double tca = Vector3.Dot(l, ray.Direction);
            if (tca < 0) return false;
            double d2 = Vector3.Dot(l, l) - tca * tca;
            if (d2 > (sphere.Radius * sphere.Radius)) return false;
            double thc = Math.Sqrt((sphere.Radius * sphere.Radius) - d2);
            double t0 = tca - thc;
            double t1 = tca + thc;

            if (t0 > t1) Extensions.Swap(ref t0, ref t1);

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

        //public static bool Intersect(Ray ray, PotatoSphere sphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        //{
        //    double t0 = 0;
        //    double t1 = 0;

        //    Vector3 L = ray.Origin - sphere.Position;
        //    double a = 1;
        //    double b = 2.0 * Vector3.Dot(ray.Direction, L);
        //    double c = Vector3.Dot(L, L) - sphere.Radius*sphere.Radius;
        //    if (!SolveQuadratic(a, b, c, ref t0, ref t1)) return false;

        //    if (t0 > t1) Extensions.Swap(ref t0, ref t1);

        //    if (t0 < 0)
        //    {
        //        t0 = t1;
        //        if (t0 < 0) return false;
        //    }

        //    distance = t0;
        //    hitPosition = ray.Cast(ray.Origin, distance);
        //    hitNormal = Vector3.Normalize(Vector3.Subtract(hitPosition, sphere.Position));

        //    return true;
        //}

        public static bool SolveQuadratic(double a, double b, double c, ref double x0, ref double x1) 
        { 
            double discr = b * b - 4 * a * c;

            if (discr < 0.0) return false;

            if (discr == 0)
            { 
                x0 = x1 = - 0.5 * b / a; 
            } 
            else
            { 
                double q = (b > 0) ? -0.5 * (b + Math.Sqrt(discr)) : -0.5 * (b - Math.Sqrt(discr));
                x0 = q / a; 
                x1 = c / q; 
            } 
 
            return true; 
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public static class SphereIntersection
    {
        public static bool Intersect(ref Ray ray, PotatoSphere sphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref float distance)
        {
            bool hit = false;
            float a, b, delta = 0;

            CalculatePolynomial(sphere, ray.Origin, ray.Direction, out a, out b, out delta);
            CalculateIntersection(ref hit, distance, a, b, delta);

            hitPosition = ray.Cast(ray.Origin, distance);
            hitNormal = Vector3.Normalize(Vector3.Subtract(hitPosition, sphere.Position));

            return hit;
        }

        private static void CalculateIntersection(ref bool hit, float discriminent, float a, float b, float delta)
        {
            if (delta < 0) return;

            float polynomialResult1, polynomialResult2;
            PolynomialResult(a, b, delta, out polynomialResult1, out polynomialResult2);

            discriminent = IntersectionDiscriminent(polynomialResult1, polynomialResult2);

            hit = true;
        }

        private static void CalculatePolynomial(PotatoSphere sphere, Vector3 origin, Vector3 direction, out float a, out float b, out float delta)
        {
            Vector3 co = Vector3.Subtract(origin, sphere.Position);
            a = 1;
            b = 2 * Vector3.Dot(direction, co);
            float c = co.Length() * co.Length() - sphere.Radius * sphere.Radius;
            delta = b * b - 4 * (a * c);
        }

        private static void PolynomialResult(float a, float b, float delta, out float polynomialResult1, out float polynomialResult2)
        {
            polynomialResult1 = (-b - (float)Math.Sqrt(delta)) / (2 * a);
            polynomialResult2 = (-b + (float)Math.Sqrt(delta)) / (2 * a);
        }

        private static float IntersectionDiscriminent(float polynomialResult1, float polynomialResult2)
        {
            if (Math.Min(polynomialResult1, polynomialResult2) > 0) return Math.Min(polynomialResult1, polynomialResult2);

            return Math.Max(polynomialResult1, polynomialResult2);
        }
    }
}

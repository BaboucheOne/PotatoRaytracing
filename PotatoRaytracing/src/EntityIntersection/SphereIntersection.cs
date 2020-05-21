using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class SphereIntersection
    {
        public static bool Intersect(Ray ray, PotatoSphere sphere)
        {
            bool hit = false;
            double d = 0;

            Vector3 co = Vector3.Subtract(ray.Origin, sphere.Position);
            double a = 1;
            double b = 2 * Vector3.Dot(ray.Direction, co);
            double c = co.Length() * co.Length() - sphere.Radius * sphere.Radius;
            double delta = b * b - 4 * (a * c);

            if (delta >= 0)
            {
                double d1 = (-b - Math.Sqrt(delta)) / (2 * a);
                double d2 = (-b + Math.Sqrt(delta)) / (2 * a);

                if (Math.Min(d1, d2) > 0)
                {
                    d = Math.Min(d1, d2);
                }
                else
                {
                    d = Math.Max(d1, d2);
                }

                hit = true;
            }

            return hit;
        }

        public static bool Intersect(Ray ray, PotatoSphere sphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        {
            //bool hit = false;
            //double a, b, delta = 0;

            //CalculatePolynomial(sphere, ray.Origin, ray.Direction, out a, out b, out delta);
            //CalculateIntersection(ref hit, distance, a, b, delta);

            //hitPosition = ray.Cast(ray.Origin, distance);
            //hitNormal = Vector3.Normalize(Vector3.Subtract(hitPosition, sphere.Position));

            ////if (hit) Console.WriteLine("{0} {1}", hit, distance);

            //return hit;

            bool hit = false;
            double d = 0;

            Vector3 co = Vector3.Subtract(ray.Origin, sphere.Position);
            double a = 1;
            double b = 2 * Vector3.Dot(ray.Direction, co);
            double c = co.Length() * co.Length() - sphere.Radius * sphere.Radius;
            double delta = b * b - 4 * (a * c);

            if (delta >= 0)
            {
                double d1 = (-b - Math.Sqrt(delta)) / (2 * a);
                double d2 = (-b + Math.Sqrt(delta)) / (2 * a);

                if (Math.Min(d1, d2) > 0)
                {
                    d = Math.Min(d1, d2);
                }
                else
                {
                    d = Math.Max(d1, d2);
                }

                hit = true;
            }

            distance = d;
            hitPosition = ray.Cast(ray.Origin, d);
            hitNormal = Vector3.Normalize(Vector3.Subtract(hitPosition, sphere.Position));

            return hit;
        }

        private static void CalculateIntersection(ref bool hit, double discriminent, double a, double b, double delta)
        {
            if (delta < 0) return;

            double polynomialResult1, polynomialResult2 = 0.0;
            PolynomialResult(a, b, delta, out polynomialResult1, out polynomialResult2);

            discriminent = IntersectionDiscriminent(polynomialResult1, polynomialResult2);

            hit = true;
        }

        private static void CalculatePolynomial(PotatoSphere sphere, Vector3 origin, Vector3 direction, out double a, out double b, out double delta)
        {
            Vector3 co = Vector3.Subtract(origin, sphere.Position);
            a = 1;
            b = 2 * Vector3.Dot(direction, co);
            double c = co.Length() * co.Length() - sphere.Radius * sphere.Radius;
            delta = b * b - 4 * (a * c);
        }

        private static void PolynomialResult(double a, double b, double delta, out double polynomialResult1, out double polynomialResult2)
        {
            polynomialResult1 = (-b - Math.Sqrt(delta)) / (2 * a);
            polynomialResult2 = (-b + Math.Sqrt(delta)) / (2 * a);
        }

        private static double IntersectionDiscriminent(double polynomialResult1, double polynomialResult2)
        {
            if (Math.Min(polynomialResult1, polynomialResult2) > 0) return Math.Min(polynomialResult1, polynomialResult2);

            return Math.Max(polynomialResult1, polynomialResult2);
        }
    }
}

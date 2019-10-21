using System;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoSphere : PotatoObject
    {
        public readonly float Radius;

        public PotatoSphere(Vector3 pos, float radius) : base(pos)
        {
            Radius = radius;
        }

        public override Vector3 GetNormal(params object[] arguments)
        {
            return Vector3.Normalize(Vector3.Subtract((Vector3)arguments[0], Position));
        }

        public override IntersectResult Intersect(Vector3 origin, Vector3 direction)
        {
            bool hit = false;
            float a, b, delta, distance = 0;

            CalculatePolynomial(origin, direction, out a, out b, out delta);
            CalculateIntersection(ref hit, ref distance, a, b, delta);

            return new IntersectResult(hit, distance);
        }

        private static void CalculateIntersection(ref bool hit, ref float distance, float a, float b, float delta)
        {
            if (delta < 0) return;

            float polynomialResult1, polynomialResult2;
            PolynomialResult(a, b, delta, out polynomialResult1, out polynomialResult2);

            distance = IntersectionDistance(polynomialResult1, polynomialResult2);

            hit = true;
        }

        private void CalculatePolynomial(Vector3 origin, Vector3 direction, out float a, out float b, out float delta)
        {
            Vector3 co = Vector3.Subtract(origin, Position);
            a = 1;
            b = 2 * Vector3.Dot(direction, co);
            float c = co.Length() * co.Length() - Radius * Radius;
            delta = b * b - 4 * (a * c);
        }

        private static void PolynomialResult(float a, float b, float delta, out float polynomialResult1, out float polynomialResult2)
        {
            polynomialResult1 = (-b - (float)Math.Sqrt(delta)) / (2 * a);
            polynomialResult2 = (-b + (float)Math.Sqrt(delta)) / (2 * a);
        }

        private static float IntersectionDistance(float polynomialResult1, float polynomialResult2)
        {
            if (Math.Min(polynomialResult1, polynomialResult2) > 0) return Math.Min(polynomialResult1, polynomialResult2);

            return Math.Max(polynomialResult1, polynomialResult2);
        }
    }
}

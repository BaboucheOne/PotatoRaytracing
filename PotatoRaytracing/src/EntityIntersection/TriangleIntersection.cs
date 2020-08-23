using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class TriangleIntersection
    {
        public static bool Intersect(Ray ray, Triangle inTriangle,
                                     ref Vector3 outIntersectionPoint, ref Vector3 outNormal, ref double distance)
        {
            Vector3 edge1 = Vector3.Subtract(inTriangle.V1, inTriangle.V0);
            Vector3 edge2 = Vector3.Subtract(inTriangle.V2, inTriangle.V0);
            Vector3 h = Vector3.Cross(ray.Direction, edge2);
            Vector3 s = new Vector3();
            Vector3 q = new Vector3();
            double a = Vector3.Dot(edge1, h);
            if (a > -Constants.EPSILON && a < Constants.EPSILON) return false;

            double f = 1.0 / a;
            s = Vector3.Subtract(ray.Origin, inTriangle.V0);
            double u = f * Vector3.Dot(s, h);
            if (u < 0.0 || u > 1.0) return false;

            q = Vector3.Cross(s, edge1);
            double v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0.0 || ((u + v) > 1.0)) return false;

            double t = f * Vector3.Dot(edge2, q);
            if (t < Constants.EPSILON) return false;

            outNormal = inTriangle.Normal;
            outIntersectionPoint = Vector3.Add(ray.Origin, ray.Direction) * t;
            distance = Vector3.Distance(ray.Origin, outIntersectionPoint);

            return true;
        }
    }
}

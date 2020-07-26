using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class TriangleIntersection
    {
        public static bool Intersect(Vector3 rayOrigin,
                                     Vector3 rayDirection,
                                     Triangle inTriangle,
                                     ref Vector3 outIntersectionPoint,
                                     ref Vector3 outNormal,
                                     ref double distance)
        {
            Vector3 edge1 = Vector3.Subtract(inTriangle.V1, inTriangle.V0);
            Vector3 edge2 = Vector3.Subtract(inTriangle.V2, inTriangle.V0);
            Vector3 h = Vector3.Cross(rayDirection, edge2);
            Vector3 s = new Vector3();
            Vector3 q = new Vector3();
            double a = Vector3.Dot(edge1, h);
            if (a > -Constants.EPSILON && a < Constants.EPSILON) return false;

            double f = 1.0 / a;
            s = Vector3.Subtract(rayOrigin, inTriangle.V0);
            double u = f * Vector3.Dot(s, h);
            if (u < 0.0 || u > 1.0) return false;

            q = Vector3.Cross(s, edge1);
            double v = f * Vector3.Dot(rayDirection, q);
            if (v < 0.0 || ((u + v) > 1.0)) return false;

            // On calcule t pour savoir ou le point d'intersection se situe sur la ligne.
            double t = f * Vector3.Dot(edge2, q);
            if (t < Constants.EPSILON) return false;

            outNormal = inTriangle.Normal;
            outIntersectionPoint = Vector3.Add(rayOrigin, rayDirection) * t;
            distance = Vector3.Distance(rayOrigin, outIntersectionPoint);
            return true;
        }

        public static bool Intersect2(Vector3 origin, Vector3 direction, Triangle triangle, ref Vector3 outHitPosition, ref Vector3 outHitNormal, ref double distance)
        {
            Vector3 normal = triangle.Normal;
            double area = normal.Length();

            double normalRayDirection = Vector3.Dot(normal, direction);
            if (Math.Abs(normalRayDirection) < Constants.EPSILON) return false;

            double d = Vector3.Dot(normal, triangle.V0);
            distance = (Vector3.Dot(normal, origin) + d) / normalRayDirection;

            if (distance < 0) return false;

            Vector3 p = origin + distance * direction;

            Vector3 edge0 = triangle.V1 - triangle.V0;
            Vector3 edge1 = triangle.V2 - triangle.V1;
            Vector3 edge2 = triangle.V0 - triangle.V2;

            Vector3 vp0 = p - triangle.V0;
            Vector3 vp1 = p - triangle.V1;
            Vector3 vp2 = p - triangle.V2;

            Vector3 c = new Vector3();
            c = Vector3.Cross(edge0, vp0);
            if (Vector3.Dot(normal, c) < 0) return false;

            c = Vector3.Cross(edge1, vp1);
            if (Vector3.Dot(normal, c) < 0) return false;

            c = Vector3.Cross(edge2, vp2);
            if (Vector3.Dot(normal, c) < 0) return false;

            return false;
        }
    }
}

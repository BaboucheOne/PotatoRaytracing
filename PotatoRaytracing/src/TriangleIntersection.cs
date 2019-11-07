using System.Numerics;

namespace PotatoRaytracing
{
    public class TriangleIntersection
    {
        public static bool rayIntersectsTriangle(Vector3 rayOrigin,
                                                 Vector3 rayVector,
                                                 Triangle inTriangle,
                                                 ref Vector3 outIntersectionPoint,
                                                 ref Vector3 outNormal,
                                                 ref float distance)
        {
            Vector3 vertex0 = inTriangle.GetVertex0();
            Vector3 vertex1 = inTriangle.GetVertex1();
            Vector3 vertex2 = inTriangle.GetVertex2();
            Vector3 edge1 = Vector3.Subtract(vertex1, vertex0);
            Vector3 edge2 = Vector3.Subtract(vertex2, vertex0);
            outNormal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
            Vector3 h = Vector3.Cross(rayVector, edge2);
            Vector3 s = new Vector3();
            Vector3 q = new Vector3();
            double a = 0;
            double f = 0;
            double u = 0;
            double v = 0; ;
            a = Vector3.Dot(edge1, h);
            if (a > -Constants.EPSILON && a < Constants.EPSILON)
            {
                return false;    // Le rayon est parallèle au triangle.
            }
            f = 1.0 / a;
            s = Vector3.Subtract(rayOrigin, vertex0);
            u = f * Vector3.Dot(s, h);
            if (u < 0.0 || u > 1.0)
            {
                return false;
            }

            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(rayVector, q);
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }
            // On calcule t pour savoir ou le point d'intersection se situe sur la ligne.
            double t = f * Vector3.Dot(edge2, q);
            if (t > Constants.EPSILON) // // Intersection avec le rayon
            {
                distance = (float)t;
                outIntersectionPoint = Vector3.Add(rayOrigin, Vector3.Multiply(rayVector, (float)t));
                return true;
            }

            return false;
        }
    }
}

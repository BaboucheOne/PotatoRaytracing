using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class KDIntersection
    {
        public static bool Intersect(Ray ray, KDNode node, ref Vector3 hitPosition, ref Vector3 hitNormal, ref double distance)
        {
            double boxDst = 0.0;
            bool boxHit = BoxIntersection.Intersect(ray, node.Bbox, ref hitPosition, ref boxDst);

            if (!boxHit && !node.Bbox.ContainsPoint(hitPosition)) return false;

            if(node.Left != null || node.Right != null)
            {
                bool hitLeft = false;
                bool hitRight = false;

                if(node.Left != null) hitLeft = Intersect(ray, node.Left, ref hitPosition, ref hitNormal, ref distance);
                if(node.Right != null) hitRight = Intersect(ray, node.Right, ref hitPosition, ref hitNormal, ref distance);

                return hitLeft || hitRight;
            }

            bool hit = false;
            double minDst = double.MaxValue;

            Vector3 hitPos = new Vector3();
            Vector3 hitNor = new Vector3();
            double dst = 0.0;
            for (int i = 0; i < node.Triangles.Count; i++)
            {
                if(TriangleIntersection.Intersect(ray, node.Triangles[i], ref hitPos, ref hitNor, ref dst))
                {
                    if(dst < minDst)
                    {
                        minDst = dst;
                        hit = true;

                        distance = dst;
                        hitPosition = hitPos;
                        hitNormal = hitNor;
                    }
                }
            }

            return hit;
        }
    }
}

using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class KDIntersectionResult
    {
        public bool Hit;
        public Vector3 HitPosition;
        public Vector3 HitNormal;
        public Color Color = Color.Yellow;

        public KDIntersectionResult(bool hit, Vector3 hitPosition, Vector3 hitNormal, Color color)
        {
            Hit = hit;
            HitPosition = hitPosition;
            HitNormal = hitNormal;
            Color = color;
        }
    }

    public static class KDIntersection
    {
        public static KDIntersectionResult Intersect(Ray ray, KDNode node)
        {
            double boxDst = 0.0;
            bool boxHit = BoxIntersection.Intersect(ray, node.Bbox, ref boxDst);

            if (!boxHit) return null;

            if(node.Left != null || node.Right != null)
            {
                if(node.Left != null)
                {
                    return Intersect(ray, node.Left);
                }

                if(node.Right != null)
                {
                    return Intersect(ray, node.Right);
                }
            }

            bool hit = false;
            double minDst = double.MaxValue;
            Vector3 hitP = new Vector3();
            Vector3 hitN = new Vector3();
            for (int i = 0; i < node.Triangles.Count; i++)
            {
                Vector3 hitPos = new Vector3();
                Vector3 hitNor = new Vector3();
                double dst = 0.0;
                if(TriangleIntersection.Intersect(ray.Origin, ray.Direction, node.Triangles[i], ref hitPos, ref hitNor, ref dst))
                {
                    if(dst < minDst)
                    {
                        minDst = dst;
                        hit = true;
                        hitP = hitPos;
                        hitN = hitNor;
                    }
                }
            }

            return new KDIntersectionResult(hit, hitP, hitN, Color.White);
        }
    }
}

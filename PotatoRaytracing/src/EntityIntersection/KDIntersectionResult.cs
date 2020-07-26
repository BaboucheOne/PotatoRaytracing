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
}

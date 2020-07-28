using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public struct HitInfo
    {
        public readonly bool Hit;
        public readonly Vector3 HitPosition;
        public readonly Vector3 HitNormal;
        public readonly double Distance;
        public readonly Color TextureColor;
        public readonly Color Color;

        public HitInfo(bool hit, Vector3 hitPosition, Vector3 hitNormal, double distance, Color textureColor, Color color)
        {
            Hit = hit;
            HitPosition = hitPosition;
            HitNormal = hitNormal;
            Distance = distance;
            TextureColor = textureColor;
            Color = color;
        }
    }
}

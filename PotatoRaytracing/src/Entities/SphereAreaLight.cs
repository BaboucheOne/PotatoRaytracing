using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public interface AreaLight
    {
    }

    public class SphereAreaLight : PotatoLight, AreaLight
    {
        public readonly double Radius = 1.0;

        public SphereAreaLight(Vector3 position, float radius, float intensity, Color color, LightType type = LightType.Point) : base(position, intensity, color, type)
        {
            Radius = radius;
        }

        public override Vector3 DirectionToLight(Vector3 position)
        {
            return Vector3.Normalize(Position - position);
        }

        public override float IntensityOverDistance(Vector3 position)
        {
            double dst = Vector3.Distance(position, Position);
            return (float)(Intensity / (4 * System.Math.PI * (dst * dst)));
        }

        public override bool IsInRange(Vector3 position)
        {
            return Vector3.Distance(position, Position) <= Radius;
        }
    }
}

using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoPointLight : PotatoLight
    {
        public readonly float Radius;

        public PotatoPointLight(Vector3 position, float radius, float intensity, Color color) : base(position, intensity, color, LightType.Point)
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

using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class PotatoDirectionalLight : PotatoLight
    {
        public readonly Vector3 Direction;

        public PotatoDirectionalLight(Vector3 direction, float intensity, Color color) : base(Vector3.Zero, intensity, color, LightType.Directional)
        {
            Direction = Vector3.Normalize(direction);
        }

        public override Vector3 DirectionToLight(Vector3 position)
        {
            return -Direction;
        }

        public override float IntensityOverDistance(Vector3 position)
        {
            return Intensity;
        }

        public override bool IsInRange(Vector3 position)
        {
            return true;
        }
    }
}

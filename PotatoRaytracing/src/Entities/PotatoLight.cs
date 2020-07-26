using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public abstract class PotatoLight : PotatoEntity
    {
        public enum LightType { Point, Directional }
        public readonly float Intensity;
        public readonly Color Color;

        public readonly LightType Type;

        public PotatoLight(Vector3 position, float intensity, Color color, LightType type) : base(position)
        {
            Intensity = intensity;
            Color = color;
            Type = type;
        }

        public abstract Vector3 DirectionToLight(Vector3 position);
        public abstract float IntensityOverDistance(Vector3 position);
        public abstract bool IsInRange(Vector3 position);
    }
}

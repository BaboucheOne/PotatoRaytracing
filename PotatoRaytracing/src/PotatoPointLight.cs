using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoPointLight : PotatoEntity
    {
        public Color Color;
        public float Radius;
        public float Intensity;

        public PotatoPointLight() : base(Vector3.Zero)
        {
            Color = Color.White;
            Radius = 10f;
            Intensity = 1;
            Position = Vector3.Zero;
        }

        public PotatoPointLight(Vector3 position, float radius, float intensity, Color color) : base(position)
        {
            Color = color;
            Radius = radius;
            Intensity = intensity;
        }

        public bool InRange(Vector3 hitPosition)
        {
            return Vector3.Distance(hitPosition, Position) <= Radius;
        }

        public Vector3 GetDirection(Vector3 position)
        {
            return Vector3.Normalize(Position - position);
        }
    }
}

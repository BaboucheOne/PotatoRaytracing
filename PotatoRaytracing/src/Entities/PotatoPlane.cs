using System.DoubleNumerics;
using System.Drawing;
using PotatoRaytracing.Materials;

namespace PotatoRaytracing
{
    public class PotatoPlane : PotatoObject
    {
        public Vector3 Normal = WorldCoordinate.PotatoCoordinate.VECTOR_UP;

        public PotatoPlane() : base(Vector3.Zero, new DefaultMaterial())
        {
        }

        public PotatoPlane(Vector3 position, Material material) : base(position, material)
        {
        }

        public override Vector2 GetUV(double x, double y, double z, Bitmap texture)
        {
            return Vector2.Zero;
        }

        public override Vector2 GetUV(Vector3 position, Bitmap texture)
        {
            return Vector2.Zero;
        }
    }
}

using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        public Material Material;

        public PotatoObject(Vector3 pos, Material material) : base(pos)
        {
            Material = material;
        }

        public abstract Vector2 GetUV(double x, double y, double z, Bitmap texture);
        public abstract Vector2 GetUV(Vector3 position, Bitmap texture);
    }
}
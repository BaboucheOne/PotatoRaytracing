using System;
using System.Drawing;
using System.DoubleNumerics;
using PotatoRaytracing.Materials;

namespace PotatoRaytracing
{
    public class PotatoSphere : PotatoObject
    {
        public readonly double Radius = 1.0;

        public PotatoSphere() : this(new Vector3(), 1.0, new DefaultMaterial())
        {
        }

        public PotatoSphere(Vector3 pos, double radius, Material material) : base(pos, material)
        {
            Radius = radius;
        }

        public PotatoSphere(Vector3 pos, double radius, Material material, string texturePath = @"Resources\\Textures\\default.bmp") : base(pos, material, texturePath)
        {
            Radius = radius;
        }

        public override Vector2 GetUV(Vector3 position, Bitmap texture)
        {
            return GetUV(position.X, position.Y, position.Z, texture);
        }

        public override Vector2 GetUV(double x, double y, double z, Bitmap texture)
        {
            double u = 0.5 - (Math.Atan2(z, x) * Constants.INV_DOUBLE_PI);
            double v = 0.5 - (Math.Asin(y) * Constants.INV_PI);

            return new Vector2(u * texture.Width, v * texture.Height);
        }
    }
}

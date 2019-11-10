using System;
using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoSphere : PotatoObject
    {
        public readonly float Radius;

        public PotatoSphere(Vector3 pos, float radius) : base(pos)
        {
            Radius = radius;
        }

        public PotatoSphere(Vector3 pos, float radius, string texturePath = "Textures\\uvTexture.bmp") : base(pos, texturePath)
        {
            Radius = radius;
        }

        public override Vector2 GetUV(Vector3 position, Bitmap texture)
        {
            return GetUV(position.X, position.Y, position.Z, texture);
        }

        public override Vector2 GetUV(float x, float y, float z, Bitmap texture)
        {
            double u = 0.5 - (Math.Atan2(z, x) * Constants.INV_DOUBLE_PI);
            double v = 0.5 - (Math.Asin(y) * Constants.INV_PI);
            return new Vector2((float)u * texture.Width, (float)v * texture.Height);
        }
    }
}

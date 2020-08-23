using System.Drawing;
using System.IO;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        public Color Color = new Color();
        protected Bitmap texture = null;
        public string TexturePath = string.Empty;

        public PotatoObject(Vector3 pos, string texturePath = @"Textures\uvTexture.bmp") : base(pos)
        {
            Color = Color.Red;

            TexturePath = texturePath;
        }

        public string GetTexturePath() => TexturePath;

        public abstract Vector2 GetUV(double x, double y, double z, Bitmap texture);
        public abstract Vector2 GetUV(Vector3 position, Bitmap texture);

        private void LoadTexture(string texturePath)
        {
            if (File.Exists(texturePath))
            {
                texture = new Bitmap(texturePath);
            }
        }
    }
}
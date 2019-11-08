using System.Drawing;
using System.IO;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        private Color Color = new Color();
        protected Bitmap texture;
        protected string texturePath;

        public PotatoObject(Vector3 pos, string texturePath = @"Textures\uvTexture.bmp") : base(pos)
        {
            Color = Color.Red;

            this.texturePath = texturePath;
        }

        public string GetTexturePath() => texturePath;

        public abstract Vector2 GetUV(float x, float y, float z, Bitmap texture);
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
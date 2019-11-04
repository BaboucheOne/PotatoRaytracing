using System.Drawing;
using System.IO;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        private Color color = new Color();
        protected Bitmap texture;
        protected string texturePath;

        public PotatoObject(Vector3 pos, string texturePath = @"Textures\uvTexture.bmp") : base(pos)
        {
            color = Color.Red;

            this.texturePath = texturePath;
        }

        public Color GetColor() => color;
        public string GetTexturePath() => texturePath;

        public abstract Vector2 GetUV(float x, float y, float z, Bitmap texture);
        public abstract Vector2 GetUV(Vector3 position, Bitmap texture);
        public abstract IntersectResult Intersect(Vector3 origin, Vector3 direction);
        public abstract IntersectResult Intersect(Ray ray);
        public abstract Vector3 GetNormal(params object[] arguments);

        private void LoadTexture(string texturePath)
        {
            if (File.Exists(texturePath))
            {
                texture = new Bitmap(texturePath);
            }
        }
    }
}
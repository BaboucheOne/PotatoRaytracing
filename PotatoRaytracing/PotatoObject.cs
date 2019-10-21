using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        public Color Color = new Color();
        public string texturePath = null;
        public string textureNormalPath = null;

        public PotatoObject(Vector3 pos, string texturepath = null) : base(pos)
        {
            if(!string.IsNullOrEmpty(texturepath))
            {
                texturePath = texturepath;
                SetTexture(texturePath);
            }
        }

        public abstract IntersectResult Intersect(Vector3 origin, Vector3 direction);
        public abstract Vector3 GetNormal(params object[] arguments);
        public Color GetColorFromTexture(Vector3 point) { return new Color(); }

        public bool HasTexture()
        {
            return !string.IsNullOrEmpty(texturePath);
        }
        public bool HasNormalTexture()
        {
            return !string.IsNullOrEmpty(textureNormalPath);
        }

        public void SetTexture(string path)
        {
            if(PotatoRender.TextureManager.AddTexture(path))
            {
                texturePath = path;
            }
        }
        public void SetMapTexture(string path)
        {
            if(PotatoRender.TextureManager.AddNormapMap(path))
            {
                textureNormalPath = path;
            }
        }
    }
}
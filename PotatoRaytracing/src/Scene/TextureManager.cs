using System.Collections.Generic;
using System.DoubleNumerics;
using System.Drawing;
using System.IO;

namespace PotatoRaytracing
{
    public class TextureManager
    {
        private Dictionary<string, Bitmap> textures = new Dictionary<string, Bitmap>();

        public void Clear()
        {
            DisposeTextures();
            textures.Clear();
        }

        private void DisposeTextures()
        {
            foreach (Bitmap img in textures.Values)
            {
                img.Dispose();
            }
        }

        public void AddTextures(string[] texturePaths)
        {
            for(int i = 0; i < texturePaths.Length; i++)
            {
                AddTexture(texturePaths[i]);
            }
        }

        public void AddTexture(string texturePath)
        {
            if(!textures.ContainsKey(texturePath))
            {
                if(File.Exists(texturePath))
                {
                    textures[texturePath] = new Bitmap(texturePath);
                }
            }
        }

        public Bitmap GetTexture(string path)
        {
            return textures[path];
        }

        public Color GetTextureColor(Vector2 uv, string texturePath)
        {
            return GetTextureColor((int)uv.X, (int)uv.Y, texturePath);
        }

        public Color GetTextureColor(int x, int y, string texturePath)
        {
            return textures[texturePath].GetPixel(x, y);
        }
    }
}

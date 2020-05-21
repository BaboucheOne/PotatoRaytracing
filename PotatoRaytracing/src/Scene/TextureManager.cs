using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.DoubleNumerics;
using System.Drawing;
using System.IO;

namespace PotatoRaytracing
{
    public class TextureManager
    {
        public Dictionary<string, Bitmap> textures = new Dictionary<string, Bitmap>(); 
        private readonly object _lock = new object();

        public TextureManager()
        {
        }

        public Dictionary<string, Bitmap> DeepCloneTextures()
        {
            Dictionary<string, Bitmap> texturesClone = new Dictionary<string, Bitmap>();

            foreach (KeyValuePair<string, Bitmap> entry in textures)
            {
                Rectangle cloneRect = new Rectangle(0, 0, entry.Value.Width, entry.Value.Height);
                System.Drawing.Imaging.PixelFormat format = entry.Value.PixelFormat;
                Bitmap m = entry.Value.Clone(cloneRect, format);
                texturesClone[entry.Key] = m;
            }

            return texturesClone;
        }

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
            for (int i = 0; i < texturePaths.Length; i++)
            {
                AddTexture(texturePaths[i]);
            }
        }

        public void AddTexture(string texturePath)
        {
            if (!textures.ContainsKey(texturePath))
            {
                if(File.Exists(texturePath))
                {
                    textures[texturePath] = new Bitmap(texturePath);
                }
            }
        }

        public Bitmap GetTexture(string path)
        {
            lock (_lock)
            {
                return textures[path];
            }
        }

        public Color GetTextureColor(Vector2 uv, string texturePath)
        {
            return GetTextureColor((int)uv.X, (int)uv.Y, texturePath);
        }

        public Color GetTextureColor(int x, int y, string texturePath)
        {
            lock (_lock)
            {
                return textures[texturePath].GetPixel(x, y);
            }
        }
    }
}

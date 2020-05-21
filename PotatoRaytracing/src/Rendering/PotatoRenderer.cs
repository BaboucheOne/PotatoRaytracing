using System.Drawing;
using System.DoubleNumerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System;

namespace PotatoRaytracing
{
    public class PotatoRenderer
    {
        private readonly PotatoSceneData sceneData;
        private readonly TextureManager textureManager;

        public PotatoRenderer(PotatoSceneData sceneData)
        {
            this.sceneData = sceneData;

            textureManager = new TextureManager();
            textureManager.AddTextures(sceneData.TexturePath);
        }

        public Bitmap ParallelWork(Tile[] tiles, int lightIndex)
        {
            CreateImageBuffer(out Bitmap bmp, out BitmapData data, out int bytesPerPixel, out byte[] buffer);
            GenerateTiles(tiles, lightIndex, bmp, data, bytesPerPixel, buffer);
            return bmp;
        }

        private void GenerateTiles(Tile[] tiles, int lightIndex, Bitmap bmp, BitmapData data, int bytesPerPixel, byte[] buffer)
        {
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            Action[] actions = CreateTileAction(tiles, lightIndex, data, bytesPerPixel, buffer);
            Parallel.Invoke(actions);
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            bmp.UnlockBits(data);
        }

        private void CreateImageBuffer(out Bitmap bmp, out BitmapData data, out int bytesPerPixel, out byte[] buffer)
        {
            bmp = new Bitmap(sceneData.Option.Width, sceneData.Option.Height);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            data = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            bytesPerPixel = Image.GetPixelFormatSize(data.PixelFormat) / 8;
            buffer = new byte[data.Width * data.Height * bytesPerPixel];
        }

        private Action[] CreateTileAction(Tile[] tiles, int lightIndex, BitmapData data, int bytesPerPixel, byte[] buffer)
        {
            Action[] actions = new Action[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                int x = tiles[i].X;
                int y = tiles[i].Y;
                int size = tiles[i].Size;

                PotatoSceneData sd = sceneData.DeepCopy();
                TextureManager tex = new TextureManager
                {
                    textures = textureManager.DeepCloneTextures()
                };
                PotatoTracer t = new PotatoTracer(sd, tex);

                actions[i] = () => Process(buffer, x, y, x + size, y + size, data.Width, bytesPerPixel, t, lightIndex);
            }

            return actions;
        }

        private void Process(byte[] buffer, int x, int y, int endx, int endy, int width, int bytesPerPixel, PotatoTracer t, int lightIndex)
        {
            Color col;
            Ray ray = new Ray();
            SuperSampling superSampling = null;
            bool superSamplingEnable = false;

            if (t.sceneData.Option.SuperSampling)
            {
                superSampling = new SuperSampling(t.sceneData.Option.Height, t.sceneData.Option.SuperSamplingDivision, t.sceneData, t);
                superSamplingEnable = true;
            }

            for (int i = x; i < endx; i++)
            {
                for (int j = y; j < endy; j++)
                {
                    int offset = ((j * width) + i) * bytesPerPixel;

                    if (superSamplingEnable)
                    {
                        col = superSampling.GetSampleColor(ray, lightIndex, i, j);
                    }
                    else
                    {
                        SetRayDirectionByPixelPosition(ref ray, t.sceneData, i, j);
                        col = t.Trace(ray, lightIndex);
                    }

                    buffer[offset] = col.B;
                    buffer[offset + 1] = col.G;
                    buffer[offset + 2] = col.R;
                }
            }
        }

        public static void SetRayDirectionByPixelPosition(ref Ray ray, PotatoSceneData sceneData, double pixelPositionX, double pixelPositionY)
        {
            ray.Set(sceneData.Camera.Position, GetDirectionFromPixel(sceneData, pixelPositionX, pixelPositionY));
        }

        public static Vector3 GetDirectionFromPixel(PotatoSceneData scene, double pixelPositionX, double pixelPositionY)
        {
            Vector3 V1 = Vector3.Multiply(scene.Camera.Right(), pixelPositionX);
            Vector3 V2 = Vector3.Multiply(scene.Camera.Up(), pixelPositionY);
            Vector3 pixelPos = Vector3.Add(Vector3.Add(scene.Option.ScreenLeft, V1), V2);

            return Vector3.Normalize(Vector3.Add(scene.Camera.Forward(), pixelPos));
        }
    }
}

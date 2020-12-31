using System; 
using System.Drawing;
using System.DoubleNumerics;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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

        public unsafe bool ParallelWork(ref Stack<Tile> stack, Bitmap bitmap)
        {
            PotatoSceneData sd = sceneData.DeepCopy();
            TextureManager tex = new TextureManager
            {
                textures = textureManager.DeepCloneTextures()
            };
            PotatoTracer t = new PotatoTracer(sd, tex);

            SuperSampling superSampling = null;
            bool superSamplingEnable = t.sceneData.Option.SuperSampling;
            if (superSamplingEnable) superSampling = new SuperSampling(t.sceneData.Option.Height, t.sceneData.Option.SuperSamplingDivision, t.sceneData, t);
            Tile tileToProcess;
            Color col;

            while (stack.Count > 0)
            {
                lock (stack)
                {
                    tileToProcess = stack.Pop();
                }

                if (superSamplingEnable)
                {
                    col = superSampling.GetSampleColor(tileToProcess.X, tileToProcess.Y);
                }
                else
                {
                    Vector2 screenCoord = new Vector2(2.0 * tileToProcess.X / sceneData.Option.Width - 1.0, (-2.0 * tileToProcess.Y) / sceneData.Option.Height + 1.0);
                    col = t.Trace(t.sceneData.Camera.CreateRay(screenCoord.X, screenCoord.Y));
                }

                lock (bitmap)
                {
                    BitmapData bData = bitmap.LockBits(new Rectangle(tileToProcess.X, tileToProcess.Y, 1, 1), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    byte bitsPerPixel = (byte)Image.GetPixelFormatSize(bitmap.PixelFormat);

                    byte* data = (byte*)bData.Scan0.ToPointer();

                    data[0] = col.B;
                    data[1] = col.G;
                    data[2] = col.R;

                    bitmap.UnlockBits(bData);
                }
            }

            return true;
        }
    }
}

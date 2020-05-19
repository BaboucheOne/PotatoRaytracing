using System;
using System.Collections.Generic;
using System.DoubleNumerics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class TestMultiTileRendering
    {
        private readonly Option option;
        private readonly PotatoScene scene;
        private readonly PotatoTracer tracer;
        private readonly TextureManager textureManager;
        private readonly SuperSampling superSampling;

        private int lightIndex = 0;

        public TestMultiTileRendering(PotatoScene scene, int lightIndex)
        {
            this.scene = scene;
            option = scene.GetOptions();

            textureManager = new TextureManager();
            textureManager.AddTextures(scene.GetTexturesPath());
            tracer = new PotatoTracer(scene, textureManager);

            this.lightIndex = lightIndex;

            if (option.SuperSampling) superSampling = new SuperSampling(option.Height, option.SuperSamplingDivision, scene, tracer);
        }

        public Color[] CreateRenderImageWorker(int beginX, int beginY, int lightIndex)
        {
            Ray ray = new Ray();

            Color[] data = new Color[256*256];

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (option.SuperSampling)
                    {
                        data[y * 256 + x] = superSampling.GetSampleColor(ray, lightIndex, beginX + x, beginY + y);
                    }
                    else
                    {
                        SetRayDirectionByPixelPosition(ref ray, scene, beginX + x, beginY + y);
                        int index = y * 256 + x;
                        data[index] = tracer.Trace(ray, lightIndex);
                    }
                }
            }

            return data;
        }

        public static void SetRayDirectionByPixelPosition(ref Ray ray, PotatoScene scene, double pixelPositionX, double pixelPositionY)
        {
            ray.Set(scene.GetCamera().Position, GetDirectionFromPixel(scene, pixelPositionX, pixelPositionY));
        }

        public static Vector3 GetDirectionFromPixel(PotatoScene scene, double pixelPositionX, double pixelPositionY)
        {
            Vector3 V1 = Vector3.Multiply(scene.GetCamera().Right(), pixelPositionX);
            Vector3 V2 = Vector3.Multiply(scene.GetCamera().Up(), pixelPositionY);
            Vector3 pixelPos = Vector3.Add(Vector3.Add(scene.GetOptions().ScreenLeft, V1), V2);

            return Vector3.Normalize(Vector3.Add(scene.GetCamera().Forward(), pixelPos));
        }
    }
}

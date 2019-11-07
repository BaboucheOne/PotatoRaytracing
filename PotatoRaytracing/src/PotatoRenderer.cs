using System;
using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoRenderer
    {
        private Option option;
        private PotatoScene scene;
        private PotatoTracer tracer;
        private TextureManager textureManager;
        private SuperSampling superSampling;
        private int lightIndex;

        public PotatoRenderer(PotatoScene scene, int lightIndex)
        {
            this.scene = scene;
            option = scene.GetOptions();

            textureManager = new TextureManager();
            textureManager.AddTextures(scene.GetTexturesPath());
            tracer = new PotatoTracer(scene, textureManager);

            this.lightIndex = lightIndex;

            if (option.SuperSampling) superSampling = new SuperSampling(option.Height, option.SuperSamplingDivision, scene, tracer);
        }

        public Bitmap RenderImage()
        {
            Console.WriteLine("light index to render {0}", lightIndex);

            Bitmap image = new Bitmap(scene.GetOptions().Width, scene.GetOptions().Height);
            Ray ray = new Ray();
            Color pixelColor = Color.Black;

            CreateRenderedImage(lightIndex, image, ref ray, ref pixelColor);

            textureManager.Clear();

            return image;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private void CreateRenderedImage(int lightIndex, Bitmap image, ref Ray ray, ref Color pixelColor)
        {
            float scale = (float)Math.Tan(DegreeToRadian(option.Fov * 0.5));
            float imageAspectRatio = option.Width / (float)option.Height;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (option.SuperSampling)
                    {
                        pixelColor = superSampling.GetSampleColor(ray, lightIndex, x, y);
                    }
                    else
                    {
                        //float ray_x = (2 * (x + 0.5f) / (float)option.Width - 1) * imageAspectRatio * scale;
                        //float ray_y = (1 - 2 * (y + 0.5f) / (float)option.Height) * scale;

                        SetRayDirectionByPixelPosition(ref ray, scene, x, y);
                        //pixelColor = tracer.Trace(ray, lightIndex, 2);
                        //ray.Direction = new Vector3(x, y, -1);
                        //ray.Direction = Vector3.Normalize(ray.Direction);
                        pixelColor = tracer.Trace(ray, lightIndex);
                    }

                    image.SetPixel(x, y, pixelColor);
                }
            }
        }

        public static void SetRayDirectionByPixelPosition(ref Ray ray, PotatoScene scene, float pixelPositionX, float pixelPositionY)
        {
            ray.Set(scene.GetCamera().Position, GetDirectionFromPixel(scene, pixelPositionX, pixelPositionY));
        }

        public static Vector3 GetDirectionFromPixel(PotatoScene scene, float pixelPositionX, float pixelPositionY)
        {
            Vector3 V1 = Vector3.Multiply(scene.GetCamera().Right(), pixelPositionX);
            Vector3 V2 = Vector3.Multiply(scene.GetCamera().Up(), pixelPositionY);
            Vector3 pixelPos = Vector3.Add(Vector3.Add(scene.GetOptions().ScreenLeft, V1), V2);

            return Vector3.Normalize(Vector3.Add(scene.GetCamera().Forward(), pixelPos));
        }
    }
}

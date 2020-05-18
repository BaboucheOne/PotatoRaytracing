using System;
using System.Drawing;
using System.DoubleNumerics;
using System.Drawing.Imaging;

namespace PotatoRaytracing
{
    public class PotatoRenderer
    {
        private readonly Option option;
        private readonly PotatoScene scene;
        private readonly PotatoTracer tracer;
        private readonly TextureManager textureManager;
        private readonly SuperSampling superSampling;
        private readonly int lightIndex;

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

            Bitmap img = CreateRenderedImage(lightIndex);

            textureManager.Clear();

            return img;
        }

        private unsafe Bitmap CreateRenderedImage(int lightIndex)
        {
            Ray ray = new Ray();
            Color pixelColor = Color.Black;

            Bitmap image = new Bitmap(scene.GetOptions().Width, scene.GetOptions().Height);
            BitmapData bData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            byte bitsPerPixel = (byte)Image.GetPixelFormatSize(image.PixelFormat);
            byte* scan0 = (byte*)bData.Scan0.ToPointer();

            for (int x = 0; x < bData.Width; x++)
            {
                for (int y = 0; y < bData.Height; y++)
                {
                    byte* data = scan0 + x * bData.Stride + y * bitsPerPixel / 8;

                    if (option.SuperSampling)
                    {
                        pixelColor = superSampling.GetSampleColor(ray, lightIndex, x, y);
                    }
                    else
                    {
                        SetRayDirectionByPixelPosition(ref ray, scene, x, y);
                        pixelColor = tracer.Trace(ray, lightIndex);
                    }

                    data[0] = pixelColor.B;
                    data[1] = pixelColor.G;
                    data[2] = pixelColor.R;
                }
            }

            image.UnlockBits(bData);

            return image;
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

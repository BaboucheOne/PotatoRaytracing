using System;
using System.Drawing;
using System.DoubleNumerics;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        private Task<Color[]>[] tasks;

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

        public unsafe Bitmap RenderImage()
        {
            Console.WriteLine("light index to render {0}", lightIndex);

            int tasksCount = 2;
            int tileSize = 256;//scene.GetOptions().Width / tasksCount;
            tasks = new Task<Color[]>[4];
            int taskIndex = 0;
            List<Vector2> screenIndex = new List<Vector2>();
            for (int x = 0; x < tasksCount; x++)
            {
                for (int y = 0; y < tasksCount; y++)
                {
                    screenIndex.Add(new Vector2(x * tileSize, y * tileSize));
                }
            }

            for (int i = 0; i < screenIndex.Count; i++)
            {
                int x = (int)screenIndex[i].X;
                int y = (int)screenIndex[i].Y;
                TestMultiTileRendering tmtr = new TestMultiTileRendering(scene, lightIndex);
                tasks[i] = Task.Run(() => tmtr.CreateRenderImageWorker(x, y, lightIndex));
                taskIndex++;
            }

            Task.WaitAll(tasks);
            textureManager.Clear();

            Bitmap image = new Bitmap(scene.GetOptions().Width, scene.GetOptions().Height);
            BitmapData bData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            byte bitsPerPixel = (byte)Image.GetPixelFormatSize(image.PixelFormat);
            byte* scan0 = (byte*)bData.Scan0.ToPointer();
            Console.WriteLine(bData.Width);
            Console.WriteLine(bData.Height);

            for (int i = 0; i < screenIndex.Count; i++)
            {
                int arrWidth = 256;
                for (int x = 0; x < arrWidth; x++)
                {
                    for (int y = 0; y < arrWidth; y++)
                    {
                        byte* data = scan0 + (x + (int)screenIndex[i].X) * bData.Stride + (y + (int)screenIndex[i].Y) * bitsPerPixel / 8;
                        int colorIndex = y * arrWidth + x;
                        data[0] = tasks[i].Result[colorIndex].B;
                        data[1] = tasks[i].Result[colorIndex].G;
                        data[2] = tasks[i].Result[colorIndex].R;
                    }
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

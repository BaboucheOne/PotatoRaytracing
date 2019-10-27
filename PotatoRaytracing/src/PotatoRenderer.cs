using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoRenderer
    {
        private int totalTaskTodo = 0;
        private PotatoScene scene;
        private PotatoTracer tracer;

        Task<Bitmap>[] renderTasks;

        public PotatoRenderer(PotatoScene scene)
        {
            this.scene = scene;
            tracer = new PotatoTracer(scene);

            totalTaskTodo = scene.GetLightCount();
            renderTasks = new Task<Bitmap>[totalTaskTodo];
        }

        public Task<Bitmap>[] GetRenderTasks() => renderTasks;

        public void RenderScene()
        {
            RunRenderTasks(renderTasks);
            Task.WaitAll(renderTasks);
        }

        private void RunRenderTasks(Task<Bitmap>[] tasks)
        {
            for (int i = 0; i < totalTaskTodo; i++)
            {
                tasks[i] = RunRenderTask(i);
            }
        }

        private Task<Bitmap> RunRenderTask(int lightIndex)
        {
            return Task.Run(() =>
            {
                return RenderImage(lightIndex);
            });
        }

        private Bitmap RenderImage(int lightIndex)
        {
            Console.WriteLine("light index to render {0}", lightIndex);

            Bitmap image = new Bitmap(scene.GetOptions().Width, scene.GetOptions().Height);
            Ray ray = new Ray();
            Color pixelColor = Color.Black;

            CreateRenderedImage(lightIndex, image, ref ray, ref pixelColor);

            return image;
        }

        private void CreateRenderedImage(int lightIndex, Bitmap image, ref Ray ray, ref Color pixelColor)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    SetRayDirectionByPixelPosition(ref ray, x, y);
                    pixelColor = tracer.Trace(ray, lightIndex, 1);
                    image.SetPixel(x, y, pixelColor);
                }
            }
        }

        private void SetRayDirectionByPixelPosition(ref Ray ray, int pixelPositionX, int pixelPositionY)
        {
            ray.Set(scene.GetCamera().Position, GetDirectionFromPixel(pixelPositionX, pixelPositionY));
        }

        private Vector3 GetDirectionFromPixel(int pixelPositionX, int pixelPositionY)
        {
            Vector3 V1 = Vector3.Multiply(scene.GetCamera().Right(), pixelPositionX);
            Vector3 V2 = Vector3.Multiply(scene.GetCamera().Up(), pixelPositionY);
            Vector3 pixelPos = Vector3.Add(Vector3.Add(scene.GetOptions().ScreenLeft, V1), V2);

            return Vector3.Normalize(Vector3.Add(scene.GetCamera().Forward(), pixelPos));
        }
    }
}

using System.Diagnostics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class PotatoRenderContext
    {
        public Option Option { get; private set; }
        public PotatoScene Scene { get; private set; }
        private ImageBlender imageBlender;
        private PotatoTasksSceneRenderer tasksSceneRenderer;

        private Stopwatch watch = new Stopwatch();

        public long GetRenderTime => watch.ElapsedMilliseconds;

        public PotatoRenderContext(Option option)
        {
            Option = option;

            Scene = new PotatoScene(option);
            tasksSceneRenderer = new PotatoTasksSceneRenderer(Scene);
            imageBlender = new ImageBlender();
        }

        public void Start(string imageName)
        {
            watch.Start();

            Bitmap[] imgs = tasksSceneRenderer.Run();

            BlendAllRenderedImageContainInTasksResult(imgs);

            SaveAndOpenImage(imageName);

            ClearRenderContext();

            watch.Stop();
        }

        private void ClearRenderContext()
        {
            tasksSceneRenderer.Clear();
            System.GC.Collect();
        }

        private void SaveAndOpenImage(string imageName)
        {
            SaveImage(imageName);
            OpenImage(imageName);
        }

        private void BlendAllRenderedImageContainInTasksResult(Bitmap[] images)
        {
            for (int i = 0; i < images.Length; i++)
            {
                imageBlender.AddImage(images[i]);
            }
        }

        private void SaveImage(string imgageName)
        {
            Bitmap finalImage = imageBlender.GetFinalImageRender();
            finalImage.Save(imgageName);
        }

        private static void OpenImage(string imageName)
        {
            Process photoViewer = new Process();
            photoViewer.StartInfo.FileName = imageName;
            photoViewer.Start();
        }
    }
}

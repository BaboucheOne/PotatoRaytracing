using System.Diagnostics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class PotatoRenderContext
    {
        private Option option;
        private PotatoScene scene;
        private ImageBlender imageBlender;
        private PotatoTasksSceneRenderer tasksSceneRenderer;

        public PotatoRenderContext(Option option)
        {
            this.option = option;

            scene = new PotatoScene(option);
            tasksSceneRenderer = new PotatoTasksSceneRenderer(scene);
            imageBlender = new ImageBlender();
        }

        public PotatoScene GetScene() => scene;

        public void Start(string imageName)
        {
            Bitmap[] imgs = tasksSceneRenderer.Run();

            BlendAllRenderedImageContainInTasksResult(imgs);

            SaveAndOpenImage(imageName);

            ClearRenderContext();
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

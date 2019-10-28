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


        public void Start()
        {
            Bitmap[] imgs = tasksSceneRenderer.Run();
            BlendAllRenderedImageContainInTasksResult(imgs);

            SaveResultImage("resultTasks");

            OpenResultPicture();
        }

        private void BlendAllRenderedImageContainInTasksResult(Bitmap[] images)
        {
            for (int i = 0; i < images.Length; i++)
            {
                imageBlender.AddImage(images[i]);
            }
        }

        private void SaveResultImage(string imgName)
        {
            Bitmap finalImage = imageBlender.GetFinalImageRender();
            finalImage.Save(imgName + ".bmp");
        }

        private static void OpenResultPicture()
        {
            Process photoViewer = new Process();
            photoViewer.StartInfo.FileName = "resultTasks.bmp";
            photoViewer.Start();
        }
    }
}

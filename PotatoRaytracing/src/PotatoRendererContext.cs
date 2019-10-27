using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoRenderContext
    {
        private Option option;
        private PotatoScene scene;
        private ImageBlender imageBlender;
        private PotatoRenderer renderer;

        public PotatoRenderContext(Option option)
        {
            this.option = option;

            scene = new PotatoScene(option);
            renderer = new PotatoRenderer(scene);
            imageBlender = new ImageBlender();
        }

        public PotatoScene GetScene() => scene;


        public void Start()
        {
            renderer.RenderScene();

            BlendAllRenderedImageContainInTasksResult(renderer.GetRenderTasks());

            SaveResultImage("resultTasks");

            OpenResultPicture();
        }

        private void BlendAllRenderedImageContainInTasksResult(Task<Bitmap>[] tasks)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Result.Save(string.Format("image render {0}.bmp", i));
                imageBlender.AddImage(tasks[i].Result);
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

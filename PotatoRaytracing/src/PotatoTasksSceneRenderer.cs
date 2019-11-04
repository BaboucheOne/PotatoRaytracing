using System.Drawing;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoTasksSceneRenderer
    {
        private PotatoScene scene;
        private Task<Bitmap>[] tasks;
        private Bitmap[] imagesRendered;
        private int tasksCountToDo;

        public PotatoTasksSceneRenderer(PotatoScene scene)
        {
            this.scene = scene;

            tasksCountToDo = scene.GetLightCount();
            imagesRendered = new Bitmap[tasksCountToDo];
            tasks = new Task<Bitmap>[tasksCountToDo];
        }

        public Bitmap[] Run()
        {
            ProcessRendererTasks();

            return imagesRendered;
        }

        private void ProcessRendererTasks()
        {
            RunRendererTasks();

            Task.WaitAll(tasks);

            TransfertTasksResultToImageRendererd();
        }

        private void TransfertTasksResultToImageRendererd()
        {
            for (int i = 0; i < tasksCountToDo; i++)
            {
                imagesRendered[i] = tasks[i].Result;
            }
        }

        private void RunRendererTasks()
        {
            for (int i = 0; i < tasksCountToDo; i++)
            {
                PotatoRenderer r = new PotatoRenderer(scene, i);
                tasks[i] = Task.Run(() => r.RenderImage());
            }
        }
    }
}

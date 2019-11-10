using System.Drawing;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoTasksSceneRenderer
    {
        private PotatoScene scene;
        private Task<Bitmap>[] tasks;
        private Bitmap[] imagesRendered;
        private int tasksToDo;

        public PotatoTasksSceneRenderer(PotatoScene scene)
        {
            this.scene = scene;

            InitTasks();
        }

        private void InitTasks()
        {
            tasksToDo = scene.LightCount;
            imagesRendered = new Bitmap[tasksToDo];
            tasks = new Task<Bitmap>[tasksToDo];
        }

        public void Clear()
        {
            for(int i = 0; i < imagesRendered.Length; i++)
            {
                imagesRendered[i].Dispose();
            }
        }

        public Bitmap[] Run()
        {
            InitTasks();
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
            for (int i = 0; i < tasksToDo; i++)
            {
                imagesRendered[i] = tasks[i].Result;
            }
        }

        private void RunRendererTasks()
        {
            for (int i = 0; i < tasksToDo; i++)
            {
                PotatoRenderer r = new PotatoRenderer(scene, i);
                tasks[i] = Task.Run(() => r.RenderImage());
            }
        }
    }
}

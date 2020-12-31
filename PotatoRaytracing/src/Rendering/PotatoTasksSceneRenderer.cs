using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoTasksSceneRenderer
    {
        private readonly PotatoSceneData sceneData;
        private Task<bool>[] tasks;
        private Bitmap imageToRender;
        private Stack<Tile> tiles = new Stack<Tile>();
        private int tasksToDo => sceneData.Option.ScreenTiles;

        public PotatoTasksSceneRenderer(PotatoSceneData sceneData)
        {
            this.sceneData = sceneData;
        }

        private void Init()
        {
            InitTasks();
            InitTiles();
        }

        private void InitTasks()
        {
            tasks = new Task<bool>[tasksToDo];
            imageToRender = new Bitmap(sceneData.Option.Width, sceneData.Option.Height);
        }

        private void InitTiles()
        {
            tiles.Clear();
            for (int x = 0; x < imageToRender.Width; x++)
            {
                for (int y = 0; y < imageToRender.Height; y++)
                {
                    tiles.Push(new Tile(x, y));
                }
            }
        }

        public void Clear()
        {
            imageToRender.Dispose();
        }

        public Bitmap Run()
        {
            Init();
            RunRendererTasks();
            return imageToRender;
        }

        private void RunRendererTasks()
        {
            for (int i = 0; i < tasksToDo; i++)
            {
                PotatoSceneData sd = sceneData.DeepCopy();
                PotatoRenderer pr = new PotatoRenderer(sd);
                tasks[i] = Task.Run(() => pr.ParallelWork(ref tiles, imageToRender));
            }

            Task.WaitAll(tasks);
        }
    }
}

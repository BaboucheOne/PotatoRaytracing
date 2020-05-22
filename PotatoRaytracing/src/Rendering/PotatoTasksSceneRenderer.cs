using System.Drawing;
using System.Threading.Tasks;

namespace PotatoRaytracing
{
    public class PotatoTasksSceneRenderer
    {
        private readonly PotatoSceneData sceneData;
        private Task<Bitmap>[] tasks;
        private Bitmap[] imagesRendered;
        private Tile[] tiles;
        private int tileSize;
        private int tasksToDo;

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
            tasksToDo = sceneData.Lights.Count;
            tasks = new Task<Bitmap>[tasksToDo];
            imagesRendered = new Bitmap[tasksToDo];
        }

        private void InitTiles()
        {
            int tileArrayLen = (int)System.Math.Sqrt(sceneData.Option.ScreenTiles);

            tiles = new Tile[sceneData.Option.ScreenTiles];
            tileSize = sceneData.Option.Width / tileArrayLen;

            int tileIndex = 0;
            for (int x = 0; x < tileArrayLen; x++)
            {
                for (int y = 0; y < tileArrayLen; y++)
                {
                    tiles[tileIndex] = new Tile(x * tileSize, y * tileSize, tileSize);
                    tileIndex++;
                }
            }
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
            Init();

            RunRendererTasks();
            TransferTasksResultToImagesRendered();

            return imagesRendered;
        }

        private void RunRendererTasks()
        {
            for (int i = 0; i < tasksToDo; i++)
            {
                PotatoRenderer pr = new PotatoRenderer(sceneData);
                int light = i;
                tasks[i] = Task.Run(() => pr.ParallelWork(tiles, light));
            }

            Task.WaitAll(tasks);
        }

        private void TransferTasksResultToImagesRendered()
        {
            for (int i = 0; i < tasksToDo; i++)
            {
                imagesRendered[i] = tasks[i].Result;
            }
        }
    }
}

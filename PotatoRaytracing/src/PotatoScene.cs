using PotatoRaytracing.WorldCoordinate;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoScene
    {
        private Option option;
        private SceneObjectsParser sceneObjectsParser = new SceneObjectsParser();

        private Camera camera = new Camera(new Vector3(0, 0, 0), new Quaternion());

        private List<PotatoObject> potatoObjects = new List<PotatoObject>();
        private List<PotatoMesh> meshs = new List<PotatoMesh>();
        private List<PotatoPointLight> lights = new List<PotatoPointLight>();
        private HashSet<string> textures = new HashSet<string>();

        public PotatoScene()
        {
            Init();
        }

        public PotatoScene(Option option)
        {
            this.option = option;

            Init();
        }

        private void Init()
        {
            camera.SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);

            //SceneLoaderAndSaver.SaveScene("scene.xml", meshs.ToArray(), lights.ToArray());

            //SceneFile sceneFile = SceneLoaderAndSaver.LoadScene("scene.xml");
            //meshs = sceneFile.Meshes.ToList();
            //lights = sceneFile.PointLights.ToList();
            //sceneObjectsParser.Parse(meshs);

            InitOption();
            CreateRandomScene();

        }

        private void InitOption()
        {
            if (option == null)
            {
                option = new Option(256, 256, 60, true, 4, camera);
            }
            else
            {
                if (option.camera == null)
                {
                    option.SetCamera(camera);
                }
            }
        }

        public Option GetOptions() => option;
        public Camera GetCamera() => camera;

        public int PotatoObjectsCount => potatoObjects.Count;
        public List<PotatoObject> GetPotatoObjects() => potatoObjects;
        public PotatoObject GetPotatoObject(int index) => potatoObjects[index];

        public int MeshCout => meshs.Count;
        public List<PotatoMesh> GetPotatoMeshes() => meshs;
        public PotatoMesh GetPotatoMesh(int index) => meshs[index];

        public int LightCount => lights.Count;
        public List<PotatoPointLight> GetPointLights() => lights;
        public PotatoPointLight GetPointLight(int index) => lights[index];

        public string[] GetTexturesPath() => textures.ToArray();

        private void CreateRandomScene()
        {
            List<Color> colors = new List<Color>()
            {
                Color.Red,
                Color.Purple,
                Color.Plum,
                Color.RosyBrown,
                Color.Green,
                Color.White,
                Color.Yellow,
                Color.BlueViolet,
                Color.CadetBlue,
                Color.OrangeRed,
                Color.Orange
            };

            //Random r = new Random();
            //for (int i = 0; i < 25; i++)
            //{
            //    Vector3 pos = new Vector3(r.Next(0, 300), r.Next(-100, 100), r.Next(-100, 100));
            //    float rad = (float)r.NextDouble() * 20;
            //    potatoObjects.Add(new PotatoSphere(pos, rad, "Textures\\uvTexture.bmp"));
            //    potatoObjects[i].Color = colors[(int)(r.NextDouble() * colors.Count)];
            //    textures.Add(potatoObjects[i].GetTexturePath());
            //}

            lights.Add(new PotatoPointLight(new Vector3(100, 0, 0), 250, 1, Color.White));

            PotatoMesh mesh = new PotatoMesh
            {
                Position = new Vector3(0.5f, 0, -0.25f),
                ObjectPath = @"Objects\\teapot.obj"
            };

            PotatoMesh mesh2 = new PotatoMesh
            {
                Position = new Vector3(0.5f, 0.25f, -0.25f),
                ObjectPath = @"Objects\\teapot.obj"
            };

            meshs.Add(mesh);
            meshs.Add(mesh2);
            sceneObjectsParser.Parse(ref meshs);
            meshs[0].SetPosition();
            meshs[1].SetPosition();
        }

        public override string ToString()
        {
            return string.Format("Scene parameters:\nObjects: {0} \nLights: {1} \nTexture loaded: {2} \nmeshes count: {3}", potatoObjects.Count, lights.Count, textures.Count, meshs.Count);
        }
    }
}

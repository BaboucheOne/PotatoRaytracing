using System;
using System.Linq;
using System.Drawing;
using System.DoubleNumerics;
using System.Collections.Generic;
using PotatoRaytracing.WorldCoordinate;

namespace PotatoRaytracing
{
    public class PotatoScene
    {
        private Option option;
        private MeshsBuilder meshsBuilder = new MeshsBuilder();

        private Camera camera = new Camera(new Vector3(), new Quaternion());

        private List<PotatoObject> potatoObjects = new List<PotatoObject>();
        private List<PotatoMesh> meshs = new List<PotatoMesh>();
        private List<PotatoPointLight> lights = new List<PotatoPointLight>();
        private HashSet<string> textures = new HashSet<string>();

        public string SceneName { get; private set; } = "Untiled.xml";

        public PotatoScene()
        {
            InitOption();
        }

        public PotatoScene(Option option)
        {
            this.option = option;
            InitOption();
        }

        public void LoadRandomScene()
        {
            CreateRandomScene();
        }

        public void LoadScene(string filename)
        {
            SceneFile sceneFile = SceneLoaderAndSaver.LoadScene(filename);
            lights = sceneFile.PointLights.ToList();
            meshsBuilder.Build(ref meshs);

            SceneName = filename;
        }

        private void InitOption()
        {
            camera.SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);

            if (option == null)
            {
                option = new Option(256, 256, 60, false, 4, camera);
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

            Random r = new Random();
            //for (int i = 0; i < 25; i++)
            //{
            //    Vector3 pos = new Vector3(r.Next(0, 300), r.Next(-100, 100), r.Next(-100, 100));
            //    float rad = (float)r.NextDouble() * 20;
            //    potatoObjects.Add(new PotatoSphere(pos, rad, "Textures\\uvTexture.bmp"));
            //    potatoObjects[i].Color = colors[(int)(r.NextDouble() * colors.Count)];
            //    textures.Add(potatoObjects[i].GetTexturePath());
            //}

            //lights.Add(new PotatoPointLight(new Vector3(0, 100, 0), 250, 1, Color.Green));
            //lights.Add(new PotatoPointLight(new Vector3(0, -100, 0), 250, 1, Color.Red));
            lights.Add(new PotatoPointLight(new Vector3(0, 0, 0), 250, 1, Color.Blue));
            lights.Add(new PotatoPointLight(new Vector3(0, 0, 100), 250, 1, Color.White));
            const int randomMeshCount = 100;

            for (int i = 0; i < randomMeshCount; i++)
            {
                PotatoMesh mesh = new PotatoMesh
                {
                    Position = new Vector3(r.Next(1, 3), r.Next(-3, 3), r.Next(-3, 3)),
                    ObjectPath = @"Objects\\tetrahedron.obj",
                    Color = colors[(int)(r.NextDouble() * colors.Count)]
            };

                meshs.Add(mesh);
            }
            meshsBuilder.Build(ref meshs);
            for (int i = 0; i < randomMeshCount; i++)
            {
                meshs[i].SetPosition();
            }
        }

        public override string ToString()
        {
            return string.Format("Objects: {0} \nLights: {1} \nTexture loaded: {2} \nmeshes count: {3}", potatoObjects.Count, lights.Count, textures.Count, meshs.Count);
        }
    }
}

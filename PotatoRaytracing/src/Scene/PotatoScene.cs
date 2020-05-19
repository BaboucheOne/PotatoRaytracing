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

        public Camera camera = new Camera(new Vector3(), new Quaternion());

        private List<PotatoSphere> spheres = new List<PotatoSphere>();
        private List<PotatoMesh> meshs = new List<PotatoMesh>();
        private List<PotatoPointLight> lights = new List<PotatoPointLight>();
        private HashSet<string> textures = new HashSet<string>();
        public BoundingBoxTree BoundingBoxTree;
        public List<BoundingBoxNode> aabbNodes = new List<BoundingBoxNode>();

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
            ClearScene();
            CreateRandomScene();
            //SceneLoaderAndSaver.SaveScene("SceneSphere.xml", spheres.ToArray(), meshs.ToArray(), lights.ToArray());
        }

        public void LoadScene(string filename)
        {
            ClearScene();

            SceneFile sceneFile = SceneLoaderAndSaver.LoadScene(filename);
            spheres = sceneFile.Spheres.ToList();
            lights = sceneFile.PointLights.ToList();
            meshsBuilder.Build(ref meshs);

            SetSpheresTexture();

            SceneName = filename;
        }

        private void ClearScene()
        {
            aabbNodes.Clear();
            textures.Clear();
            meshs.Clear();
            spheres.Clear();
            lights.Clear();
        }

        private void InitOption()
        {
            camera.SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);

            if (option == null)
            {
                option = new Option(256, 256, 60, false, 4, 4, 4, 5, camera);
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

        public int SphereCount => spheres.Count;
        public List<PotatoSphere> GetSpheres() => spheres;
        public PotatoSphere GetSphere(int index) => spheres[index];

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
            for (int i = 0; i < 100; i++)
            {
                Vector3 pos = new Vector3(r.Next(100, 300), r.Next(-100, 100), r.Next(-100, 100));
                float rad = (float)r.NextDouble() * 20;
                spheres.Add(new PotatoSphere(pos, rad, "Textures\\uvTexture.bmp"));
                spheres[i].Color = colors[(int)(r.NextDouble() * colors.Count)];
            }

            //lights.Add(new PotatoPointLight(new Vector3(0, 100, 0), 250, 1, Color.Green));
            //lights.Add(new PotatoPointLight(new Vector3(0, -100, 0), 250, 1, Color.Red));
            lights.Add(new PotatoPointLight(new Vector3(0, 0, 0), 2500, 1, Color.Red));
            lights.Add(new PotatoPointLight(new Vector3(0, 0, 100), 2500, 1, Color.White));
            const int randomMeshCount = 100;

            for (int i = 0; i < 0; i++)
            {
                PotatoMesh mesh = new PotatoMesh
                {
                    Position = new Vector3(r.Next(1, 20), r.Next(-20, 20), r.Next(-20, 20)),
                    //Position = new Vector3(5, 0, 0),
                    ObjectPath = @"Objects\\cube.obj",
                    Color = colors[(int)(r.NextDouble() * colors.Count)]

                };

                meshs.Add(mesh);
            }

            meshsBuilder.Build(ref meshs);

            /* Tree intersection
            List<PotatoBox> boxes = new List<PotatoBox>();
            for (int i = 0; i < randomMeshCount; i++)
            {
                Vector3 pos = new Vector3(5, i, i * 3 - 10);
                //Vector3 pos = new Vector3(r.Next(1, 100), r.Next(0, 20), r.Next(0, 20));
                float rad = 0.25f;
                PotatoSphere sphere = new PotatoSphere(pos, rad, "Textures\\uvTexture.bmp");
                spheres.Add(sphere);

                PotatoBox box = new PotatoBox(pos);
                BoundingBoxNode bbn = new BoundingBoxNode(box, spheres[i], false);
                aabbNodes.Add(bbn);
            }

            BoundingBoxTree = new BoundingBoxTree();
            BoundingBoxTree.Build(ref aabbNodes);
            */

            SetSpheresTexture();
        }

        private void SetSpheresTexture()
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                textures.Add(spheres[i].GetTexturePath());
            }
        }

        public override string ToString()
        {
            return string.Format("Spheres: {0} \nLights: {1} \nTexture loaded: {2} \nmeshes count: {3}", spheres.Count, lights.Count, textures.Count, meshs.Count);
        }
    }
}

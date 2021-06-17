using System;
using System.Linq;
using System.Drawing;
using System.DoubleNumerics;
using System.Collections.Generic;
using PotatoRaytracing.Materials;

namespace PotatoRaytracing
{
    public class PotatoScene
    {
        private readonly Option option;
        private readonly ObjBaker meskBaker = new ObjBaker();

        public Cubemap Cubemap = new Cubemap();
        public Camera Camera = new Camera();
        public PotatoSceneData PotatoSceneData;

        public string SceneName { get; private set; } = "Untiled.xml";

        private List<PotatoLight> lights = new List<PotatoLight>();
        private List<PotatoSphere> spheres = new List<PotatoSphere>();
        private List<PotatoPlane> planes = new List<PotatoPlane>();
        private List<PotatoMesh> meshes = new List<PotatoMesh>();

        public PotatoScene(Option option)
        {
            this.option = option;
        }

        public void Save(string scenePath)
        {
            SceneLoaderAndSaver.Save(scenePath, spheres.ToArray(), planes.ToArray(), meshes.ToArray(), lights);
        }

        public void Load(string scenePath)
        {
            ClearScene();

            SceneFile sceneFile = SceneLoaderAndSaver.Load(scenePath);

            spheres = sceneFile.Spheres.ToList();
            meshes = sceneFile.Meshes.ToList();
            lights = sceneFile.GetLigths().ToList();
            planes = new List<PotatoPlane>();

            PrepareScene();
        }

        private void ClearScene()
        {
            spheres.Clear();
            planes.Clear();
            meshes.Clear();
            lights.Clear();
            PotatoSceneData.Clear();
        }

        public Option GetOptions() => option;
        public Camera GetCamera() => PotatoSceneData.Camera;

        public void CreateRandomScene()
        {
            ClearScene();

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
            //for (int i = 0; i < 10; i++)
            //{
            //    //Vector3 pos = new Vector3(r.Next(-50, 50), r.Next(-50, 50), r.Next(10, 10));
            //    Vector3 pos = new Vector3(r.Next(-25, 25), r.Next(-25, 25), 25);
            //    float rad = 1f;//(float)r.NextDouble() * 5f;
            //    Material mat = new DefaultMaterial(1f, 0.6f, colors[(int)(r.NextDouble() * colors.Count)], 75, 1f);
            //    spheres.Add(new PotatoSphere(pos, rad, mat));
            //}

            //for (int x = 0; x < 50; x++)
            //{
            //    for (int y = 0; y < 25; y++)
            //    {
            //        Vector3 pos = new Vector3(x * 2 - 50 , y * 2 - 50, 25);
            //        float rad = 1f;
            //        Material mat = new DefaultMaterial(1f, 0.6f, colors[(int)(r.NextDouble() * colors.Count)], 75, 1f);
            //        spheres.Add(new PotatoSphere(pos, rad, mat));
            //    }
            //}

            //spheres.Add(new PotatoSphere(new Vector3(0, 0, 50), 15f, new Refraction(1.2f)));
            //spheres.Add(new PotatoSphere(new Vector3(40, 0, 75), 15f, new Reflection(1f, 0.5f, 120)));
            Material mat = new DefaultMaterial(1f, 0.6f, colors[(int)(r.NextDouble() * colors.Count)], 75, 1f);
            spheres.Add(new PotatoSphere(new Vector3(35, 15, 75), 15f, mat));

            //lights.Add(new PotatoPointLight(new Vector3(0, 10, 0), 50000, 10000000, Color.White));
            lights.Add(new PotatoDirectionalLight(new Vector3(0, -0.5, 0.5), 150f, Color.White));

            PotatoPlane potatoPlane = new PotatoPlane(Vector3.Zero)
            {
                Normal = WorldCoordinate.PotatoCoordinate.VECTOR_UP
            };
            planes.Add(potatoPlane);

            //const int randomMeshCount = 1;
            //for (int i = 0; i < randomMeshCount; i++)
            //{
            //    PotatoMesh mesh = new PotatoMesh
            //    {
            //        Position = new Vector3(r.Next(1, 20), r.Next(-20, 20), r.Next(-20, 20)),
            //        ObjectPath = @"Resources\\Objects\\teapot.obj", //TODO: Implement ressource path.
            //        Color = colors[(int)(r.NextDouble() * colors.Count)]
            //    };

            //    meshs.Add(mesh);
            //}

            PotatoMesh mesh = new PotatoMesh //TODO: Implement ressource path.
            {
                //Position = new Vector3(0, -5, 6),
                Position = new Vector3(0, 0, 1),
                //Position = new Vector3(3, 0, 0),
                //ObjectPath = @"Resources\\Objects\\kukuri.obj",
                //ObjectPath = @"Resources\\Objects\\Stock_Lr_22.obj",
                //ObjectPath = @"Resources\\Objects\\bunny.obj",
                ObjectPath = @"Resources\\Objects\\teapot.obj",
                //ObjectPath = @"Resources\\Objects\\red_dot.obj",
                //ObjectPath = @"Resources\\Objects\\ico.obj",
                Color = colors[(int)(r.NextDouble() * colors.Count)]
            };
            //meshes.Add(mesh);

            PrepareScene();
        }

        private void PrepareScene()
        {
            meskBaker.Build(ref meshes);
            Cubemap.LoadCubemap(option.Cubemap);
            KDTree tree = new KDTree(GetAllTrianglesInScene(meshes));
            PotatoSceneData = new PotatoSceneData(spheres, planes, meshes, lights, RetreiveAllTextureInScene(spheres), tree, option, Cubemap, Camera);
        }

        private List<Triangle> GetAllTrianglesInScene(List<PotatoMesh> meshs)
        {
            List<Triangle> triangles = new List<Triangle>();
            foreach (PotatoMesh mesh in meshs)
            {
                triangles.AddRange(mesh.GetTriangles().ToList());
            }

            return triangles;
        }

        private HashSet<string> RetreiveAllTextureInScene(List<PotatoSphere> spheres)
        {
            HashSet<string> textures = new HashSet<string>();

            for (int i = 0; i < spheres.Count; i++)
            {
                textures.Add(spheres[i].Material.AlbedoTexturePath);
            }

            return textures;
        }

        public override string ToString()
        {
            return string.Format("Spheres: {0} \nLights: {1} \nTexture loaded: {2} \nmeshes: {3}",
                                PotatoSceneData.Spheres.Count, PotatoSceneData.Lights.Count, PotatoSceneData.Textures.Count, PotatoSceneData.Meshs.Count);
        }
    }
}

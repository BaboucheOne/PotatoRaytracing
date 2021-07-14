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
        private readonly MeshsBuilder meshsBuilder = new MeshsBuilder();

        public Cubemap Cubemap = new Cubemap();
        public Camera Camera = new Camera();
        public PotatoSceneData PotatoSceneData;

        public string SceneName { get; private set; } = "Untiled.xml";

        public PotatoScene()
        {
        }

        public PotatoScene(Option option)
        {
            this.option = option;
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
            List<PotatoSphere> spheres = sceneFile.Spheres.ToList();
            List<PotatoPlane> planes = new List<PotatoPlane>();
            List<PotatoMesh> meshs = new List<PotatoMesh>(); //TODO: Support mesh in scenes data file.
            meshsBuilder.Build(ref meshs);

            SceneName = filename;

            Cubemap.LoadCubemap(option.Cubemap);

            KDTree tree = new KDTree(GetAllTrianglesInScene(meshs));
            PotatoSceneData = new PotatoSceneData(spheres, planes, meshs, sceneFile.PointLights.ToList(), RetreiveAllTextureInScene(spheres), tree, option, Cubemap);
        }

        private void ClearScene()
        {
            //throw new NotImplementedException();
        }


        public Option GetOptions() => option;
        public Camera GetCamera() => PotatoSceneData.Camera;

        private void CreateRandomScene() //TODO: Refactor this (colors should go, separate into several methods)
        {
            List<PotatoSphere> spheres = new List<PotatoSphere>();
            List<PotatoPlane> planes = new List<PotatoPlane>();
            List<PotatoMesh> meshs = new List<PotatoMesh>();
            List<PotatoLight> lights = new List<PotatoLight>();

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
            for (int i = 0; i < 0; i++)
            {
                Vector3 pos = new Vector3(r.Next(-50, 50), r.Next(-50, 50), r.Next(75, 100));
                float rad = (float)r.NextDouble() * 5;
                Material mat = new DefaultMaterial(1f, 0.6f, colors[(int)(r.NextDouble() * colors.Count)], 75, 1f);
                spheres.Add(new PotatoSphere(pos, rad, mat));
            }

            //spheres.Add(new PotatoSphere(new Vector3(0, 0, 50), 15f, new Refraction(1.2f)));
            //spheres.Add(new PotatoSphere(new Vector3(40, 0, 75), 15f, new Reflection(1f, 0.5f, 120)));
            
            
            //spheres.Add(new PotatoSphere(new Vector3(0, 0, 60), 15f, new Lambertian(1f, Color.White)));
            //spheres.Add(new PotatoSphere(new Vector3(-20, 0, 55), 15f, new Lambertian(1f, Color.White)));
            //spheres.Add(new PotatoSphere(new Vector3(0, 0, 0), 250f, new Lambertian(1f, Color.White)));
            //spheres.Add(new PotatoSphere(new Vector3(-35, 0, 60), 15f, new Lambertian(1f, Color.White)));
            planes.Add(new PotatoPlane(new Vector3(0, -15, 0), new Lambertian(1f, Color.White)));


            //lights.Add(new PotatoPointLight(new Vector3(0, 16, 20), 5000, 10000000, Color.White));
            lights.Add(new PotatoDirectionalLight(new Vector3(0, -0.3, 0.5), 350f, Color.White));
            //lights.Add(new SphereAreaLight(new Vector3(0, 30, 55), 20f, 10f, Color.White));

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
                Position = new Vector3(0, -5, 6),
                //Position = new Vector3(5, 0, 0),
                //Position = new Vector3(3, 0, 0),
                //ObjectPath = @"Resources\\Objects\\kukuri.obj",
                ObjectPath = @"Resources\\Objects\\teapot.obj",
                //ObjectPath = @"Resources\\Objects\\bunny.obj",
                //ObjectPath = @"Resources\\Objects\\teapot.obj",
                //ObjectPath = @"Resources\\Objects\\red_dot.obj",
                //ObjectPath = @"Resources\\Objects\\ico.obj",
                Color = colors[(int)(r.NextDouble() * colors.Count)]
            };

            meshs.Add(mesh);
            meshsBuilder.Build(ref meshs);

            Cubemap.LoadCubemap(option.Cubemap);

            KDTree tree = new KDTree(GetAllTrianglesInScene(meshs));
            PotatoSceneData = new PotatoSceneData(spheres, planes, meshs, lights, RetreiveAllTextureInScene(spheres), tree, option, Cubemap, Camera);
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

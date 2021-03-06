﻿using System;
using System.Linq;
using System.Drawing;
using System.DoubleNumerics;
using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class PotatoScene
    {
        private readonly Option option;
        private readonly MeshsBuilder meshsBuilder = new MeshsBuilder();

        public Cubemap Cubemap = new Cubemap();
        public Camera Camera = new Camera();
        public PotatoSceneData PotatoSceneData;
        public BoundingBoxTree BoundingBoxTree;
        public List<BoundingBoxNode> aabbNodes = new List<BoundingBoxNode>();

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
            List<PotatoMesh> meshs = new List<PotatoMesh>(); //TODO: Support mesh in scenes data file.
            meshsBuilder.Build(ref meshs);

            SceneName = filename;

            Cubemap.LoadCubemap(option.Cubemap);
            PotatoSceneData = new PotatoSceneData(spheres, meshs, sceneFile.PointLights.ToList(), RetreiveAllTextureInScene(spheres), option, Cubemap);
        }

        private void ClearScene()
        {
            aabbNodes.Clear();
        }


        public Option GetOptions() => option;
        public Camera GetCamera() => PotatoSceneData.Camera;

        private void CreateRandomScene() //TODO: Refactor this (colors should go, separate into several methods)
        {
            List<PotatoSphere> spheres = new List<PotatoSphere>();
            List<PotatoMesh> meshs = new List<PotatoMesh>();
            List<PotatoPointLight> lights = new List<PotatoPointLight>();

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
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = new Vector3(r.Next(-250, 250), r.Next(-100, 100), r.Next(-100, 100));
                float rad = (float)r.NextDouble() * 20;
                spheres.Add(new PotatoSphere(pos, rad, @"Resources\\Textures\\default.bmp"));
                spheres[i].Color = colors[(int)(r.NextDouble() * colors.Count)];
            }

            //Vector3 pos = new Vector3(50, 0, 0);
            //float rad = 20f;//(float)r.NextDouble() * 20;
            //spheres.Add(new PotatoSphere(pos, rad, @"Resources\\Textures\\default.bmp"));
            //spheres[0].Color = colors[(int)(r.NextDouble() * colors.Count)];

            //lights.Add(new PotatoPointLight(new Vector3(0, -100, 0), 500, 1, Color.Green));
            //lights.Add(new PotatoPointLight(new Vector3(0, 0, 0), 500, 1, Color.Blue));
            //lights.Add(new PotatoPointLight(new Vector3(0, 100, 0), 500, 1, Color.Red));
            lights.Add(new PotatoPointLight(new Vector3(0, 0, 100000), 500, 1, Color.White));

            const int randomMeshCount = 0;
            for (int i = 0; i < randomMeshCount; i++)
            {
                PotatoMesh mesh = new PotatoMesh
                {
                    Position = new Vector3(r.Next(1, 20), r.Next(-20, 20), r.Next(-20, 20)),
                    ObjectPath = @"Resources\\Objects\\cube.obj", //TODO: Implement ressource path.
                    Color = colors[(int)(r.NextDouble() * colors.Count)]

                };

                meshs.Add(mesh);
            }

            meshsBuilder.Build(ref meshs);

            Cubemap.LoadCubemap(option.Cubemap);
            PotatoSceneData = new PotatoSceneData(spheres, meshs, lights, RetreiveAllTextureInScene(spheres), option, Cubemap, Camera);
        }

        private HashSet<string> RetreiveAllTextureInScene(List<PotatoSphere> spheres)
        {
            HashSet<string> textures = new HashSet<string>();

            for (int i = 0; i < spheres.Count; i++)
            {
                textures.Add(spheres[i].GetTexturePath());
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

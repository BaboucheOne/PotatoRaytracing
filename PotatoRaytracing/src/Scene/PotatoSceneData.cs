﻿using PotatoRaytracing.WorldCoordinate;
using System.Collections.Generic;
using System.DoubleNumerics;
using System.Linq;

namespace PotatoRaytracing
{
    public struct PotatoSceneData
    {
        public Option Option;
        public Camera Camera;
        public Cubemap Cubemap;
        public readonly List<PotatoSphere> Spheres;
        public readonly List<PotatoMesh> Meshs;
        public readonly List<PotatoPointLight> Lights;
        public readonly HashSet<string> Textures;

        public string[] TexturePath => Textures.ToArray();

        public PotatoSceneData(List<PotatoSphere> spheres, List<PotatoMesh> meshs, List<PotatoPointLight> lights, HashSet<string> textures, Option option, Cubemap cubemap, Camera cam = null) : this()
        {
            Spheres = spheres;
            Meshs = meshs;
            Lights = lights;
            Textures = textures;
            Option = option;
            Cubemap = cubemap;

            if (cam == null)
            {
                Camera = new Camera(new Vector3(), new Quaternion());
            }
            else
            {
                Camera = cam;
            }

            InitOption();
        }

        private void InitOption()
        {
            Camera.SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);

            if (Option == null)
            {
                Option = new Option(512, 512, 60, false, 4, 4, 1, 4, 5, @"Resources\\Textures\cubemap5.bmp", Camera);
            }
            else
            {
                if (Option.Camera == null)
                {
                    Option.SetCamera(Camera);
                }
            }
        }

        public PotatoSceneData DeepCopy()
        {
            List<PotatoSphere> spheres = new List<PotatoSphere>(Spheres);
            List<PotatoMesh> meshs = new List<PotatoMesh>(Meshs);
            List<PotatoPointLight> lights = new List<PotatoPointLight>(Lights);
            HashSet<string> textures = new HashSet<string>(Textures);
            Cubemap cubemap = Cubemap.DeepCopy();

            return new PotatoSceneData(spheres, meshs, lights, textures, Option, cubemap, Camera);
        }
    }
}

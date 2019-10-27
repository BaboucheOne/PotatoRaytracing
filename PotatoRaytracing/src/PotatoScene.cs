using PotatoRaytracing.WorldCoordinate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoScene
    {
        private Option option;

        private Camera camera = new Camera();
        private List<PotatoObject> potatoObjects = new List<PotatoObject>();
        private List<PotatoPointLight> lights = new List<PotatoPointLight>();

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
            InitOption();

            CreateRandomScene();
        }

        private void InitOption()
        {
            if (option == null)
            {
                option = new Option(256, 256, 60, camera);
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

        public int GetPotatoObjectsCount() => potatoObjects.Count;
        public List<PotatoObject> GetPotatoObjects() => potatoObjects;
        public PotatoObject GetPotatoObject(int index) => potatoObjects[index];

        public int GetLightCount() => lights.Count;
        public List<PotatoPointLight> GetPointLights() => lights;
        public PotatoPointLight GetPointLight(int index) => lights[index];

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
            for (int i = 0; i < 50; i++)
            {
                Vector3 pos = new Vector3(r.Next(10, 300), r.Next(-100, 100), r.Next(-100, 100));
                float rad = (float)r.NextDouble() * 20;
                potatoObjects.Add(new PotatoSphere(pos, rad));
            }
            
            lights.Add(new PotatoPointLight(new Vector3(100, 0, 0), 250, 1, Color.Red));
            lights.Add(new PotatoPointLight(new Vector3(100, 100, 0), 250, 1, Color.Green));
            lights.Add(new PotatoPointLight(new Vector3(100, 0, 100), 250, 1, Color.Blue));
        }
    }
}

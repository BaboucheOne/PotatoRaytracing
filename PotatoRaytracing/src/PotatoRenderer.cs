using PotatoRaytracing.WorldCoordinate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoRender
    {
        public static Camera camera = new Camera(new Vector3(0, 0, 0));
        private Ray rendererRay = new Ray();
        private Option option;

        public List<PotatoObject> SceneObjectsToRender = new List<PotatoObject>();

        public PotatoRender()
        {
        }

        public void Init()
        {
            CreateRandomScene();

            camera.SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);

            option = new Option(1000, 1000, 60, camera);
        }

        private void CreateRandomScene()
        {
            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                Vector3 pos = new Vector3(r.Next(0, 300), r.Next(-00, 100), r.Next(-100, 100));
                float rad = (float)r.NextDouble() * 20;
                SceneObjectsToRender.Add(new PotatoSphere(pos, rad));
            }
        }

        public void RenderScene()
        {
            Bitmap image = new Bitmap(option.Width, option.Heigth);
            Color finalColor = new Color();

            double startRenderTime = Program.CurrentProcess.UserProcessorTime.TotalMilliseconds;

            for (int i = 0; i < option.Width; i++)
            {
                for (int j = 0; j < option.Heigth; j++)
                {
                    rendererRay.Direction = GetDirectionFromPixel(i, j);
                    
                    PotatoObject objectToRender = GetClosestObject();
                    if (objectToRender == null) continue;

                    IntersectObjectToRender(objectToRender);

                    image.SetPixel(i, j, finalColor);
                }
            }

            Console.WriteLine("Render tine: {0} s", (Program.CurrentProcess.UserProcessorTime.TotalMilliseconds - startRenderTime) % 60000 / 1000);
            image.Save("output.bmp");
        }

        private bool IsIntersect(PotatoObject objectToRender)
        {
            return objectToRender.Intersect(camera.Position, rendererRay.Direction).Intersect;
        }

        private float GetIntersectionDistance(PotatoObject objectToRender)
        {
            return objectToRender.Intersect(camera.Position, rendererRay.Direction).Discriminent;
        }

        private void IntersectObjectToRender(PotatoObject objectToRender)
        {
            if (IsIntersect(objectToRender))
            {
                float dist = GetIntersectionDistance(objectToRender);

                Vector3 hitPoint = rendererRay.Shoot(camera.Position, dist);
                Vector3 normal = objectToRender.GetNormal(hitPoint);
            }
        }

        public Vector3 GetDirectionFromPixel(int i, int j)
        {
            Vector3 V1 = Vector3.Multiply(camera.Right(), i);
            Vector3 V2 = Vector3.Multiply(camera.Up(), j);
            Vector3 pixelPos = Vector3.Add(Vector3.Add(option.ScreenLeft, V1), V2);
            return Vector3.Normalize(Vector3.Add(camera.Forward(), pixelPos));
        }

        private PotatoObject GetClosestObject()
        {
            IntersectResult r = new IntersectResult();
            PotatoObject obj = null;
            float minD = float.PositiveInfinity;
            for (int i = 0; i < SceneObjectsToRender.Count; i++)
            {
                r = SceneObjectsToRender[i].Intersect(rendererRay.Position, rendererRay.Direction);
                if (r.Intersect && r.Discriminent < minD)
                {
                    minD = r.Discriminent;
                    obj = SceneObjectsToRender[i];
                }
            }

            return obj;
        }
    }
}

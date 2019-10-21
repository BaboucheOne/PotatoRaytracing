using System;
using System.Numerics;

namespace PotatoRaytracing
{
    public class Option
    {
        public int Width;
        public int Heigth;
        public int HalfWidth;
        public int HalfHeight;
        public double Fov;
        public Vector3 ScreenLeft;
        public Vector3 ScreenCenter;

        public Camera camera;

        public Option(int width, int heigth, double fov, Camera cam)
        {
            InitializeParameters(width, heigth, fov, cam);

            SetScreenSettings();
        }

        private void InitializeParameters(int width, int heigth, double fov, Camera cam)
        {
            Width = width;
            Heigth = heigth;
            Fov = fov;
            camera = cam;
        }

        private void SetScreenSettings()
        {
            SetHalfResolution();
            SetScreenCenter();
            SetLeftScreenBorder();
        }

        private void SetLeftScreenBorder()
        {
            Vector3 rightScreen = Vector3.Subtract(ScreenCenter, camera.Right()) * HalfWidth;
            Vector3 upScreen = camera.Up() * HalfHeight;

            ScreenLeft = Vector3.Subtract(rightScreen, upScreen);
        }

        private void SetScreenCenter()
        {
            ScreenCenter = Vector3.Subtract(camera.Position, camera.Forward());
        }

        private void SetHalfResolution()
        {
            HalfWidth = (int)Math.Round(Width * 0.5f);
            HalfHeight = (int)Math.Round(Heigth * 0.5f);
        }

        public override string ToString()
        {
            return string.Format("Width: {0} \n" +
                "Height: {1} \n" +
                "Fov: {2} \n" +
                "Camera Position: {3} \n" +
                "Camera Rotation: {4} \n" +
                "Screen Center {5} \n" +
                "Screen Left {6}", Width, Heigth, Fov, camera.Position, camera.Rotation, ScreenCenter, ScreenLeft);
        }
    }
}

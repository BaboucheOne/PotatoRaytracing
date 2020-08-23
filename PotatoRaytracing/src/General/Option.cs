using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class Option
    {
        public int Width;
        public int Height;
        public int HalfWidth;
        public int HalfHeight;
        public double Fov;
        public bool SuperSampling;
        public int SuperSamplingDivision;
        public int ScreenTiles;
        public int RecursionDepth;
        public int VideoDuration;
        public int VideoFPS;
        public string Cubemap;
        public Vector3 ScreenLeft;
        public Vector3 ScreenCenter;

        public Camera Camera;

        public Option(int width, int heigth, double fov, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, string cubemap)
        {
            InitializeParameters(width, heigth, fov, superSampling, superSamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, cubemap, null);
        }

        public Option(int width, int heigth, double fov, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, string cubemap, Camera cam)
        {
            InitializeParameters(width, heigth, fov, superSampling, superSamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, cubemap, cam);

            SetScreenSettings();
        }

        private void InitializeParameters(int width, int heigth, double fov, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, string cubemap, Camera cam)
        {
            Width = width;
            Height = heigth;
            Fov = fov;
            Camera = cam;
            SuperSampling = superSampling;
            SuperSamplingDivision = superSamplingDivision;
            ScreenTiles = screenTiles;
            RecursionDepth = recursionDepth;
            VideoDuration = videoDuration;
            VideoFPS = videoFPS;
            Cubemap = cubemap;
        }

        public void SetCamera(Camera camera)
        {
            Camera = camera;

            SetScreenSettings();
        }

        private void SetScreenSettings()
        {
            SetHalfResolution();
            SetScreenCenter();
            SetLeftScreenBorder();
        }

        private void SetLeftScreenBorder()
        {
            Vector3 rightScreen = Vector3.Subtract(ScreenCenter, Camera.Right) * HalfWidth;
            Vector3 upScreen = Camera.Up * HalfHeight;

            ScreenLeft = Vector3.Subtract(rightScreen, upScreen);
        }

        private void SetScreenCenter()
        {
            ScreenCenter = Vector3.Subtract(Camera.Position, Camera.Forward);
        }

        private void SetHalfResolution()
        {
            HalfWidth = (int)Math.Round(Width * 0.5f);
            HalfHeight = (int)Math.Round(Height * 0.5f);
        }

        public override string ToString()
        {
            return string.Format("Width: {0} \n" +
                "Height: {1} \n" +
                "Fov: {2} \n" +
                "Camera Position: {3} \n" +
                "Camera Rotation: {4} \n" +
                "Screen Center {5} \n" +
                "Screen Left {6} \n" +
                "Super sampling enable {7} \n" +
                "Super sampling division {8} \n" +
                "Screen tiles {9} \n" +
                "Recursion depth {10} \n" +
                "Video Duration {11} \n" +
                "Video FPS {12} \n" +
                "Cubemap {13}", Width, Height, Fov, Camera.Position, Camera.Rotation,
                                    ScreenCenter, ScreenLeft, SuperSampling, SuperSamplingDivision,
                                    ScreenTiles, RecursionDepth, VideoDuration, VideoFPS, Cubemap);
        }
    }
}

using System;
using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class Option
    {
        public int Width;
        public int Height;
        public int HalfWidth;
        public int HalfHeight;
        public float Fov;
        public double Bias;
        public bool SuperSampling;
        public int SuperSamplingDivision;
        public int ScreenTiles;
        public int RecursionDepth;
        public int VideoDuration;
        public int VideoFPS;
        public bool UseSolidColor;
        public string Cubemap;
        public Color SolidColor;
        public Vector3 ScreenLeft;
        public Vector3 ScreenCenter;

        public float AspectRatio => Width / (float)Height;

        public Camera Camera;

        public Option(int width, int heigth, float fov, double bias, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, bool useSolidColor, string cubemap, Color solidColor)
        {
            InitializeParameters(width, heigth, fov, bias, superSampling, superSamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, useSolidColor, cubemap, solidColor, null);
        }

        public Option(int width, int heigth, float fov, double bias, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, bool useSolidColor, string cubemap, Color solidColor, Camera cam)
        {
            InitializeParameters(width, heigth, fov, bias, superSampling, superSamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, useSolidColor, cubemap, solidColor, cam);

            SetScreenSettings();
        }

        private void InitializeParameters(int width, int heigth, float fov, double bias, bool superSampling, int superSamplingDivision, int screenTiles, int recursionDepth, int videoDuration, int videoFPS, bool useSolidColor, string cubemap, Color solidColor, Camera cam)
        {
            Width = width;
            Height = heigth;
            Fov = fov;
            Bias = bias;
            Camera = cam;
            SuperSampling = superSampling;
            SuperSamplingDivision = superSamplingDivision;
            ScreenTiles = screenTiles;
            RecursionDepth = recursionDepth;
            VideoDuration = videoDuration;
            VideoFPS = videoFPS;
            UseSolidColor = useSolidColor;
            Cubemap = cubemap;
            SolidColor = solidColor;
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
                "Bias: {3} \n" +
                "Camera Position: {4} \n" +
                "Camera Rotation: {5} \n" +
                "Screen Center {6} \n" +
                "Screen Left {7} \n" +
                "Super sampling enable {8} \n" +
                "Super sampling division {9} \n" +
                "Screen tiles {10} \n" +
                "Recursion depth {11} \n" +
                "Video Duration {12} \n" +
                "Video FPS {13} \n" +
                "Cubemap {14} \n" +
                "Solid color {15}", Width, Height, Fov, Bias, Camera.Position, Camera.Rotation,
                                    ScreenCenter, ScreenLeft, SuperSampling, SuperSamplingDivision,
                                    ScreenTiles, RecursionDepth, VideoDuration, VideoFPS, Cubemap, UseSolidColor);
        }
    }
}

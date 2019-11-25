﻿using System;
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
        public Vector3 ScreenLeft;
        public Vector3 ScreenCenter;

        public Camera camera;

        public Option(int width, int heigth, double fov, bool superSampling, int superSamplingDivision)
        {
            InitializeParameters(width, heigth, fov, superSampling, superSamplingDivision, null);
        }

        public Option(int width, int heigth, double fov, bool superSampling, int superSamplingDivision, Camera cam)
        {
            InitializeParameters(width, heigth, fov, superSampling, superSamplingDivision, cam);

            SetScreenSettings();
        }

        private void InitializeParameters(int width, int heigth, double fov, bool superSampling, int superSamplingDivision, Camera cam)
        {
            Width = width;
            Height = heigth;
            Fov = fov;
            camera = cam;
            SuperSampling = superSampling;
            SuperSamplingDivision = superSamplingDivision;
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;

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
                "Super sampling division {8}", Width, Height, Fov, camera.Position, camera.Rotation, ScreenCenter, ScreenLeft, SuperSampling, SuperSamplingDivision);
        }
    }
}
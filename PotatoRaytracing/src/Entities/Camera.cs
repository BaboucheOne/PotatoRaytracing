using System;
using System.DoubleNumerics;
using PotatoRaytracing.WorldCoordinate;

namespace PotatoRaytracing
{
    public class Camera : PotatoEntity
    {
        public Vector3 Up { get; private set; } = new Vector3();
        public Vector3 Forward { get; private set; } = new Vector3();
        public Vector3 Right { get; private set; } = new Vector3();
        public Vector3 PointOfInterest { get; private set; } = new Vector3();
        public double H = 0;
        public double W = 0;

        public Camera()
        {
            ComputeDirection(PotatoCoordinate.VECTOR_FORWARD, 40f, 1); //TODO: A fix
        }

        public Camera(Vector3 position, float fov, float aspectRatio) : base(position)
        {
            ComputeDirection(PotatoCoordinate.VECTOR_FORWARD, fov, aspectRatio);
        }

        public Camera(Vector3 position, Quaternion rotation, float fov, float aspectRatio) : base(position, rotation)
        {
            ComputeDirection(PotatoCoordinate.VECTOR_FORWARD, fov, aspectRatio);
        }


        private void ComputeDirection(Vector3 pointOfInterest, float fov, float aspectRation)
        {
            Forward = Vector3.Normalize(pointOfInterest - Position);
            Right = Vector3.Cross(Forward, PotatoCoordinate.VECTOR_UP);
            Up = Vector3.Cross(Right, Forward);
            H = Math.Tan(fov * Math.PI / 180.0);
            W = H * aspectRation;
        }

        public Ray CreateRay(double pixelX, double pixelY)
        {
            return new Ray(Position, Vector3.Normalize(Forward + (pixelX * W * Right) + (pixelY * H * Up)));
        }
    }
}

using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class BoundingBox
    {
        public Vector3 Position { get; private set; } = new Vector3();
        public Vector3 Size => size;
        public Vector3 Min {get; private set; }= new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
        public Vector3 Max {get; private set; }= new Vector3();

        private Vector3 size = new Vector3();

        public BoundingBox()
        {
        }

        public BoundingBox(Vector3 position, double width, double height, double depth)
        {
            Position = position;
            SetBounds(width, height, depth);
        }

        public void SetBounds(double width, double height, double depth)
        {
            Vector3 half = new Vector3(width * 0.5, height * 0.5, depth * 0.5);
            Min = Position - half;
            Max = Position + half;
            size.Set(width, height, depth);
        }

        public int GetLongestAxis()
        {
            double max = Math.Max(Size.X, Math.Max(Size.Y, Size.Z));

            if (Size.X == max) return 0;
            if (Size.Y == max) return 1;
            return 2;
        }

        public bool ContainsPoint(Vector3 point)
        {
            return (Min.X <= point.X && point.X <= Max.X) && (Min.Y <= point.Y && point.Y <= Max.Y) && (Min.Z <= point.Z && point.Z <= Max.Z);
        }
}
}

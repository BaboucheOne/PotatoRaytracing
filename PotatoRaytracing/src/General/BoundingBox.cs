using System;
using System.DoubleNumerics;
using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class BoundingBox
    {
        public Vector3 Min = new Vector3();
        public Vector3 Max = new Vector3();
        public Vector3 Center = new Vector3();
        public Vector3 Size = new Vector3();

        public BoundingBox()
        {
        }

        public BoundingBox(List<Triangle> triangles)
        {
            SetBounds(triangles);
        }

        public void SetBounds(List<Triangle> triangles)
        {
            Min.Set(0.0);
            Max.Set(0.0);

            foreach (Triangle t in triangles)
            {
                Min.X = Math.Min(Min.X, t.Min.X);
                Min.Y = Math.Min(Min.Y, t.Min.Y);
                Min.Z = Math.Min(Min.Z, t.Min.Z);

                Max.X = Math.Max(Max.X, t.Max.X);
                Max.Y = Math.Max(Max.Y, t.Max.Y);
                Max.Z = Math.Max(Max.Z, t.Max.Z);
            }
        }

        public void UpdateCentroid()
        {
            Center.X = (Min.X + Max.X) * 0.5;
            Center.Y = (Min.Y + Max.Y) * 0.5;
            Center.Z = (Min.Z + Max.Z) * 0.5;
            UpdateSize();
        }

        public void UpdateSize()
        {
            Size.X = Min.X < Max.X ? Max.X - Min.X : Min.X - Max.X;
            Size.Y = Min.Y < Max.Y ? Max.Y - Min.Y : Min.Y - Max.Y;
            Size.Z = Min.Z < Max.Z ? Max.Z - Min.Z : Min.Z - Max.Z;
        }

        public int GetLongestAxis()
        {
            double max = Math.Max(Size.X, Math.Max(Size.Y, Size.Z));

            if (Size.X == max) return 0;
            if (Size.Y == max) return 1;
            return 2;
        }
    }
}

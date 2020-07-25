using System.DoubleNumerics;

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
            SetBounds(0);
        }

        public BoundingBox(Triangle triangle)
        {
            SetBounds(triangle);
        }

        public void SetBounds(Triangle t)
        {
            Min.X = t.V0.X < t.V1.X ? (t.V0.X < t.V2.X ? t.V0.X : t.V2.X) : (t.V1.X < t.V2.X ? t.V1.X : t.V2.X);
            Min.Y = t.V0.Y < t.V1.Y ? (t.V0.Y < t.V2.Y ? t.V0.Y : t.V2.Y) : (t.V1.Y < t.V2.Y ? t.V1.Y : t.V2.Y);
            Min.Z = t.V0.Z < t.V1.Z ? (t.V0.Z < t.V2.Z ? t.V0.Z : t.V2.Z) : (t.V1.Z < t.V2.Z ? t.V1.Z : t.V2.Z);

            Max.X = t.V0.X > t.V1.X ? (t.V0.X > t.V2.X ? t.V0.X : t.V2.X) : (t.V1.X > t.V2.X ? t.V1.X : t.V2.X);
            Max.Y = t.V0.Y > t.V1.Y ? (t.V0.Y > t.V2.Y ? t.V0.Y : t.V2.Y) : (t.V1.Y > t.V2.Y ? t.V1.Y : t.V2.Y);
            Max.Z = t.V0.Z > t.V1.Z ? (t.V0.Z > t.V2.Z ? t.V0.Z : t.V2.Z) : (t.V1.Z > t.V2.Z ? t.V1.Z : t.V2.Z);

            UpdateCentroid();
            UpdateSize();
        }

        public void SetBounds(double val)
        {
            Min.Set(val);
            Max.Set(val);
            Center.Set(val);
            Size.Set(0.0);
        }

        public void SetBounds(double xMin, double yMin, double zMin, double xMax, double yMax, double zMax)
        {
            Min.Set(xMin, yMin, zMin);
            Max.Set(xMax, yMax, zMax);
            UpdateCentroid();
        }

        public void SetBounds(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
            UpdateCentroid();
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

        public int GetBiggerAxis()
        {
            double sizeX = Max.X - Min.X;
            double sizeY = Max.Y - Min.Y;
            double sizeZ = Max.Z - Min.Z;

            if (sizeX > sizeY && sizeX > sizeZ) return 0;
            if (sizeY > sizeZ && sizeY > sizeX) return 1;

            return 2;
        }
    }
}

using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class BoxIntersection
    {
        public static bool Intersect(Ray ray, PotatoBox box, ref double distance)
        {
            double tx1 = (box.Min.X - ray.Origin.X) * ray.InverseDirection.X;
            double tx2 = (box.Max.X - ray.Origin.X) * ray.InverseDirection.X;

            double tmin = Math.Min(tx1, tx2);
            double tmax = Math.Max(tx1, tx2);

            double ty1 = (box.Min.Y - ray.Origin.Y) * ray.InverseDirection.Y;
            double ty2 = (box.Max.Y - ray.Origin.Y) * ray.InverseDirection.Y;

            tmin = Math.Max(tmin, Math.Min(ty1, ty2));
            tmax = Math.Min(tmax, Math.Max(ty1, ty2));

            distance = tmin;

            return tmax >= tmin;
        }

        public static bool Stack(PotatoBox a, PotatoBox b)
        {
            return (a.Min.X <= b.Max.X && a.Max.X >= b.Min.X) &&
                   (a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y) &&
                   (a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z);
        }

        public static PotatoBox MergePotatoBox(PotatoBox a, PotatoBox b)
        {
            Vector3 pos = Vector3.Add(a.Position, b.Position) * 0.5;
            Vector3 max = Vector3.Max(a.Max, b.Max);
            Vector3 min = Vector3.Max(a.Min, b.Min);

            return new PotatoBox(pos, min, max);
        }
    }
}

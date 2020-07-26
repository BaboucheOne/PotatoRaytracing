using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class BoxIntersection
    {
        public static bool Intersect(Ray ray, BoundingBox box, ref double distance)
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
    }
}

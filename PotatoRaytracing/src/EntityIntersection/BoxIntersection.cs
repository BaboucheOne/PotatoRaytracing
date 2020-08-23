using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public static class BoxIntersection
    {
        public static bool Intersect(Ray ray, BoundingBox box)
        {
            double t1, t2, tnear = -1000.0f, tfar = 1000.0f, temp, tCube;
            Vector3 b1 = box.Min;
            Vector3 b2 = box.Max;
            bool hit = true;
            for (int i = 0; i < 3; i++)
            {
                if (ray.Direction.Get(i) == 0)
                {
                    if (ray.Origin.Get(i) < b1.Get(i) || ray.Origin.Get(i) > b2.Get(i))
                        hit = false;
                }
                else
                {
                    t1 = (b1.Get(i) - ray.Origin.Get(i)) / ray.Direction.Get(i);
                    t2 = (b2.Get(i) - ray.Origin.Get(i)) / ray.Direction.Get(i);
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > tnear)
                        tnear = t1;
                    if (t2 < tfar)
                        tfar = t2;
                    if (tnear > tfar)
                        hit = false;
                    if (tfar < 0)
                        hit = false;
                }
            }
            if (hit == false)
                tCube = -1;
            else
                tCube = tnear;

            return hit;
        }

        public static bool Intersect(Ray ray, BoundingBox box, ref Vector3 hitPosition, ref double distance)
        {
            double t1, t2, tnear = -1000.0f, tfar = 1000.0f, temp, tCube;
            Vector3 b1 = box.Min;
            Vector3 b2 = box.Max;
            bool hit = true;
            for (int i = 0; i < 3; i++)
            {
                if (ray.Direction.Get(i) == 0)
                {
                    if (ray.Origin.Get(i) < b1.Get(i) || ray.Origin.Get(i) > b2.Get(i))
                        hit = false;
                }
                else
                {
                    t1 = (b1.Get(i) - ray.Origin.Get(i)) / ray.Direction.Get(i);
                    t2 = (b2.Get(i) - ray.Origin.Get(i)) / ray.Direction.Get(i);
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > tnear)
                        tnear = t1;
                    if (t2 < tfar)
                        tfar = t2;
                    if (tnear > tfar)
                        hit = false;
                    if (tfar < 0)
                        hit = false;
                }
            }
            if (hit == false)
                tCube = -1;
            else
                tCube = tnear;

            distance = tCube;
            hitPosition = ray.Origin + ray.Direction * distance;

            return hit;
        }
    }
}

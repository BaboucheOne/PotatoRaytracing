using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        public Color Color = new Color();

        public PotatoObject(Vector3 pos) : base(pos)
        {
            Color = Color.Red;
        }

        public abstract IntersectResult Intersect(Vector3 origin, Vector3 direction);
        public abstract IntersectResult Intersect(Ray ray);
        public abstract Vector3 GetNormal(params object[] arguments);
    }
}
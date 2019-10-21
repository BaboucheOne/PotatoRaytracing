using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoObject : PotatoEntity
    {
        public Color Color = new Color();

        public PotatoObject(Vector3 pos) : base(pos)
        {
        }

        public abstract IntersectResult Intersect(Vector3 origin, Vector3 direction);
        public abstract Vector3 GetNormal(params object[] arguments);
    }
}
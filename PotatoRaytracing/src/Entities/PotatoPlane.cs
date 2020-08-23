using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoPlane : PotatoEntity
    {
        public Vector3 Normal = WorldCoordinate.PotatoCoordinate.VECTOR_UP;

        public PotatoPlane() : base(Vector3.Zero)
        {
        }

        public PotatoPlane(Vector3 position) : base(position)
        {
        }

        public PotatoPlane(Vector3 position, Quaternion rotation) : base(position, rotation)
        {
        }

        public PotatoPlane(Vector3 position, Quaternion rotation, Vector3 scale) : base(position, rotation, scale)
        {
        }
    }
}

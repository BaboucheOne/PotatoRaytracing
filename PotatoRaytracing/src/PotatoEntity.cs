using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoEntity
    {
        public Vector3 Position = new Vector3();
        public Quaternion Rotation = new Quaternion();

        public PotatoEntity(Vector3 position)
        {
            Position = position;
        }

        public PotatoEntity(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
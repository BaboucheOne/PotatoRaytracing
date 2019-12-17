using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public abstract class PotatoEntity
    {
        public Vector3 Position = new Vector3();
        public Quaternion Rotation = new Quaternion();
        public Vector3 Scale = new Vector3(1, 1, 1);

        public PotatoEntity()
        {
        }

        public PotatoEntity(Vector3 position)
        {
            Position = position;
        }

        public PotatoEntity(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public PotatoEntity(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
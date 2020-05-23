using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class Ray
    {
        public Vector3 Origin { get; private set; } = new Vector3();
        public Vector3 Direction { get; private set; } = new Vector3();
        public Vector3 InverseDirection { get; private set; } = new Vector3();

        public Ray()
        {
        }

        public Ray(Vector3 position)
        {
            Origin = position;
        }

        public Ray(Vector3 position, Vector3 direction)
        {
            Origin = position;
            Direction = direction;
            SetInvertDirection();
        }

        public Vector3 Cast(Vector3 pos, double time)
        {
            return Vector3.Add(pos, Vector3.Multiply(Direction, time));
        }

        public void Set(Vector3 position, Vector3 direction)
        {
            Origin = position;
            Direction = direction;
            SetInvertDirection();
        }

        public void SetDirection(Vector3 direction)
        {
            Direction = direction;
            SetInvertDirection();
        }

        private void SetInvertDirection()
        {
            Vector3 vector3 = new Vector3(1 / Direction.X, 1 / Direction.Y, 1 / Direction.Z);
            InverseDirection = vector3;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Origin, Direction);
        }
    }
}

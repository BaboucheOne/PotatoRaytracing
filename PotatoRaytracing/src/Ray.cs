using System.Numerics;

namespace PotatoRaytracing
{
    public class Ray
    {
        public Vector3 Origin = new Vector3();
        public Vector3 Direction = new Vector3();

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
        }
        
        public Vector3 Cast(Vector3 pos, float time)
        {
            return Vector3.Add(pos, Vector3.Multiply(Direction, time));
        }

        public void Set(Vector3 position, Vector3 direction)
        {
            Origin = position;
            Direction = direction;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Origin, Direction);
        }
    }
}

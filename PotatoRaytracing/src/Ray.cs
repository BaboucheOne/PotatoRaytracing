using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PotatoRaytracing
{
    public class Ray
    {
        public Vector3 Position = new Vector3();
        public Vector3 Direction = new Vector3();

        public Ray()
        {

        }

        public Ray(Vector3 position)
        {
            Position = position;
        }

        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
        
        public Vector3 Shoot(Vector3 pos, float time)
        {
            return Vector3.Add(pos, Vector3.Multiply(Direction, time));
        }
    }
}

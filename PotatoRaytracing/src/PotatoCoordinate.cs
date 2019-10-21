using System.Numerics;

namespace PotatoRaytracing.WorldCoordinate
{
    public static class PotatoCoordinate
    {
        public static Vector3 VECTOR_FORWARD = new Vector3(1, 0, 0);
        public static Vector3 VECTOR_BACKWARD = new Vector3(-1, 0, 0);
        public static Vector3 VECTOR_RIGHT = new Vector3(0, 0, 1);
        public static Vector3 VECTOR_LEFT = new Vector3(0, 0, -1);
        public static Vector3 VECTOR_UP = new Vector3(0, 1, 0);
        public static Vector3 VECTOR_DOWN = new Vector3(0, -1, 0);
        public static Vector3 VECTOR_ZERO = new Vector3(0, 0, 0);
    }
}

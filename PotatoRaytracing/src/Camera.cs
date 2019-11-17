using System.DoubleNumerics;
using PotatoRaytracing.WorldCoordinate;

namespace PotatoRaytracing
{
    public class Camera : PotatoEntity
    {
        private Vector3 Direction = new Vector3();
        private Vector3 PointOfInterest = new Vector3();

        public Camera(Vector3 position) : base(position)
        {
        }
        public Camera(Vector3 position, Quaternion rotation) : base(position, rotation)
        {
        }

        public Camera() : base()
        {
        }

        public void SetPointOfInterest(Vector3 pointOfInterest)
        {
            PointOfInterest = pointOfInterest;
            ComputeDirection();
        }

        private void ComputeDirection()
        {
            Direction = Vector3.Normalize(Vector3.Subtract(Position, PointOfInterest));
        }

        public Vector3 GetPointOfInterest()
        {
            return PointOfInterest;
        }

        public Vector3 Forward()
        {
            return Direction;
        }

        public Vector3 Right()
        {
            Vector3 cross = Vector3.Cross(PotatoCoordinate.VECTOR_UP, Forward());
            return Vector3.Normalize(cross);
        }

        public Vector3 Up()
        {
            return Vector3.Cross(Forward(), Right());
        }
    }
}

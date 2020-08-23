using System.DoubleNumerics;
using PotatoRaytracing.WorldCoordinate;

namespace PotatoRaytracing
{
    public class Camera : PotatoEntity
    {
        private Vector3 Direction = new Vector3();

        public Vector3 Up { get; private set; } = new Vector3();
        public Vector3 Forward { get; private set; } = new Vector3();
        public Vector3 Right { get; private set; } = new Vector3();
        public Vector3 PointOfInterest { get; private set; } = new Vector3();

        public Camera(Vector3 position) : base(position)
        {
            SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);
        }

        public Camera(Vector3 position, Quaternion rotation) : base(position, rotation)
        {
            SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);
        }

        public Camera() : base()
        {
            SetPointOfInterest(PotatoCoordinate.VECTOR_FORWARD);
        }

        public void SetPointOfInterest(Vector3 pointOfInterest)
        {
            PointOfInterest = pointOfInterest;
            ComputeDirection();
        }

        private void ComputeDirection()
        {
            Direction = Vector3.Normalize(Vector3.Subtract(Position, PointOfInterest));

            Forward = Direction;
            Right = Vector3.Cross(PotatoCoordinate.VECTOR_UP, Forward);
            Up = Vector3.Cross(Forward, Right);
        }
    }
}

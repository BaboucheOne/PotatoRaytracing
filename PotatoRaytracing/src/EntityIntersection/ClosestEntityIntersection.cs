using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class ClosestEntityIntersection
    {
        public readonly Vector3 HitPosition;
        public readonly Vector3 HitNormal;
        public readonly bool IsMesh;
        public readonly bool IsNull;
        public readonly double Distance;

        public ClosestEntityIntersection(Vector3 hitPosition, Vector3 hitNormal, double distance, bool isMesh, bool isNull = false)
        {
            HitPosition = hitPosition;
            HitNormal = hitNormal;
            Distance = distance;
            IsMesh = isMesh;
            IsNull = isNull;
        }

        public virtual bool IsIntersect => false;
    }

    public class ClosestTriangle : ClosestEntityIntersection
    {
        public readonly PotatoMesh Mesh;
        public readonly Triangle Triangle;

        public ClosestTriangle(PotatoMesh mesh, Triangle triangle, Vector3 hitPosition, Vector3 hitNormal, double distance) : base(hitPosition, hitNormal, distance, true)
        {
            Mesh = mesh;
            Triangle = triangle;
        }

        public override bool IsIntersect => Mesh != null;
    }

    public class ClosestSphere : ClosestEntityIntersection
    {
        public readonly PotatoSphere Sphere;

        public ClosestSphere(PotatoSphere sphere, Vector3 hitPosition, Vector3 hitNormal, double distance) : base(hitPosition, hitNormal, distance, false)
        {
            Sphere = sphere;
        }

        public override bool IsIntersect => Sphere != null;
    }
}

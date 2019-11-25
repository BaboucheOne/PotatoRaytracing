using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class IntersectionHandler
    {
        private PotatoScene scene;

        public IntersectionHandler(PotatoScene scene)
        {
            this.scene = scene;
        }

        public ClosestEntityIntersection GetClosestEntity(Ray ray)
        {
            ClosestTriangle triangleResult = GetClosestTriangleIntersection(ray);
            ClosestSphere sphereResult = GetClosestSphereIntersection(ray);

            if (!triangleResult.IsIntersect && sphereResult.IsIntersect)
            {
                return sphereResult;
            }
            else if (!sphereResult.IsIntersect && triangleResult.IsIntersect)
            {
                return triangleResult;
            }
            else if (triangleResult.IsIntersect && sphereResult.IsIntersect)
            {
                if (sphereResult.Distance > triangleResult.Distance)
                {
                    return triangleResult;
                }
                else
                {
                    return sphereResult;
                }
            }

            return new ClosestEntityIntersection(new Vector3(), new Vector3(), 0.0, false, true);
        }

        private ClosestTriangle GetClosestTriangleIntersection(Ray ray)
        {
            Triangle triangleInt = null;
            PotatoMesh meshInt = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();
            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0;

            for (int i = 0; i < scene.MeshCout; i++)
            {
                PotatoMesh mesh = scene.GetPotatoMesh(i);

                for (int j = 0; j < mesh.GetTrianglesCount; j++)
                {
                    if (TriangleIntersection.RayIntersectsTriangle(ray.Origin, ray.Direction, mesh.GetTriangle(j), ref localHitPosition, ref localHitNormal, ref t))
                    {
                        if (t < distance)
                        {
                            distance = t;
                            triangleInt = mesh.GetTriangle(j);
                            meshInt = mesh;

                            hitPosition = localHitPosition;
                            hitNormal = localHitNormal;
                        }
                    }
                }
            }

            return new ClosestTriangle(meshInt, triangleInt, hitPosition, hitNormal, distance);
        }

        private ClosestSphere GetClosestSphereIntersection(Ray ray)
        {
            PotatoSphere intersectedSphere = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();

            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0.0;

            for (int i = 0; i < scene.SphereCount; i++)
            {
                if (SphereIntersection.Intersect(ray, scene.GetSphere(i), ref localHitPosition, ref localHitNormal, ref t))
                {
                    if (t < distance)
                    {
                        distance = t;
                        intersectedSphere = scene.GetSphere(i);

                        hitPosition = localHitPosition;
                        hitNormal = localHitNormal;
                    }
                }
            }

            return new ClosestSphere(intersectedSphere, hitPosition, hitNormal, distance);
        }
    }
}

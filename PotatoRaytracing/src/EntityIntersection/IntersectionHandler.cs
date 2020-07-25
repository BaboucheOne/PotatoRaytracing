using System.Collections.Generic;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class IntersectionHandler
    {
        private PotatoSceneData sceneData;

        public IntersectionHandler(PotatoSceneData sceneData)
        {
            this.sceneData = sceneData;
        }

        public ClosestEntityIntersection GetClosestEntity(Ray ray)
        {
            /* Tree intersection
            HashSet<BoundingBoxNode> nodesToExplore = new HashSet<BoundingBoxNode>();
            double distance = 0.0;
            for(int i = 0; i < scene.BoundingBoxTree.Root.Childs.Count; i++)
            {
                if(BoxIntersection.Intersect(ray, scene.BoundingBoxTree.Root.Childs[i].Box, ref distance))
                {
                    System.Console.WriteLine("Root to explore");
                    nodesToExplore.Add(scene.BoundingBoxTree.Root.Childs[i]);
                }
            }

            while(nodesToExplore.Count > 1)
            {
                foreach (BoundingBoxNode node in nodesToExplore)
                {
                    for (int i = 0; i < node.Childs.Count; i++)
                    {
                        if (BoxIntersection.Intersect(ray, node.Childs[i].Box, ref distance))
                        {
                            nodesToExplore.Add(node.Childs[i]);
                        }
                    }

                    nodesToExplore.Remove(node);
                }
            }

            bool intersect = false;
            double distance = 0.0;
            double realDistance = double.PositiveInfinity;
            BoundingBoxNode aabbNode = null;
            List<BoundingBoxNode> closestIntersectedBox = new List<BoundingBoxNode>();
            for(int i = 0; i < scene.aabbNodes.Count; i++)
            {
                if(BoxIntersection.Intersect(ray, scene.aabbNodes[i].Box, ref distance))
                {
                    if (distance < realDistance)
                    {
                        intersect = true;
                        aabbNode = scene.aabbNodes[i];
                        realDistance = distance;

                        closestIntersectedBox.Add(scene.aabbNodes[i]);
                    }
                }
            }

            for (int i = closestIntersectedBox.Count - 1; i >= 0; i--)
            {
                if (closestIntersectedBox[i].IsMesh)
                {
                    ClosestTriangle st = GetClosestTriangleIntersectionInList(ray, new List<PotatoMesh>() { closestIntersectedBox[i].Entity as PotatoMesh });
                    if (st.IsIntersect) return st;
                }
                else
                {
                    ClosestSphere sr = GetClosestSphereIntersectionInList(ray, new List<PotatoSphere>() { closestIntersectedBox[i].Entity as PotatoSphere });
                    if (sr.IsIntersect) return sr;
                }
            }

            return new ClosestEntityIntersection(new Vector3(), new Vector3(), 0.0, false, true);
            */


            ClosestTriangle triangleResult = GetClosestTriangleIntersectionInScene(ray);
            ClosestSphere sphereResult = GetClosestSphereIntersectionInScene(ray);

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

        private ClosestTriangle GetClosestTriangleIntersectionInScene(Ray ray)
        {
            Triangle triangleInt = null;
            PotatoMesh meshInt = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();
            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0;

            LoopMeshListToGetTheClosets(ray,  sceneData.Meshs, ref triangleInt, ref meshInt, ref hitPosition, ref hitNormal, ref localHitPosition, ref localHitNormal, ref distance, ref t);

            return new ClosestTriangle(meshInt, triangleInt, hitPosition, hitNormal, distance);
        }

        private ClosestTriangle GetClosestTriangleIntersectionInList(Ray ray, List<PotatoMesh> meshs)
        {
            Triangle triangleInt = null;
            PotatoMesh meshInt = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();
            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0;

            LoopMeshListToGetTheClosets(ray, meshs, ref triangleInt, ref meshInt, ref hitPosition, ref hitNormal, ref localHitPosition, ref localHitNormal, ref distance, ref t);

            return new ClosestTriangle(meshInt, triangleInt, hitPosition, hitNormal, distance);
        }

        private void LoopMeshListToGetTheClosets(Ray ray, List<PotatoMesh> meshs, ref Triangle triangleInt, ref PotatoMesh meshInt, ref Vector3 hitPosition, ref Vector3 hitNormal, ref Vector3 localHitPosition, ref Vector3 localHitNormal, ref double distance, ref double t)
        {
            for (int i = 0; i < meshs.Count; i++)
            {
                PotatoMesh mesh = meshs[i];

                for (int j = 0; j < mesh.GetTrianglesCount; j++)
                {
                    if (TriangleIntersection.Intersect(ray.Origin, ray.Direction, mesh.GetTriangle(j), ref localHitPosition, ref localHitNormal, ref t))
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
        }

        private ClosestSphere GetClosestSphereIntersectionInScene(Ray ray)
        {
            PotatoSphere intersectedSphere = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();

            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0.0;

            LoopSphereListToGetTheClosets(ray, sceneData.Spheres, ref intersectedSphere, ref hitPosition, ref hitNormal, ref localHitPosition, ref localHitNormal, ref distance, ref t);

            return new ClosestSphere(intersectedSphere, hitPosition, hitNormal, distance);
        }

        private ClosestSphere GetClosestSphereIntersectionInList(Ray ray, List<PotatoSphere> spheres)
        {
            PotatoSphere intersectedSphere = null;
            Vector3 hitPosition = new Vector3();
            Vector3 hitNormal = new Vector3();

            Vector3 localHitPosition = new Vector3();
            Vector3 localHitNormal = new Vector3();
            double distance = double.PositiveInfinity;
            double t = 0.0;

            //LoopSphereListToGetTheClosets(ray, spheres, ref intersectedSphere, ref hitPosition, ref hitNormal, ref localHitPosition, ref localHitNormal, ref distance, ref t);

            for (int i = 0; i < spheres.Count; i++)
            {
                if (SphereIntersection.Intersect(ray, spheres[i], ref localHitPosition, ref localHitNormal, ref t))
                {
                    if (t < distance)
                    {
                        distance = t;
                        intersectedSphere = spheres[i];

                        hitPosition = localHitPosition;
                        hitNormal = localHitNormal;
                    }
                }
            }

            return new ClosestSphere(intersectedSphere, hitPosition, hitNormal, distance);
        }

        private void LoopSphereListToGetTheClosets(Ray ray, List<PotatoSphere> spheres, ref PotatoSphere intersectedSphere, ref Vector3 hitPosition, ref Vector3 hitNormal, ref Vector3 localHitPosition, ref Vector3 localHitNormal, ref double distance, ref double t)
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                if (SphereIntersection.Intersect(ray, spheres[i], ref localHitPosition, ref localHitNormal, ref t))
                {
                    if (t < distance)
                    {
                        distance = t;
                        intersectedSphere = spheres[i];

                        hitPosition = localHitPosition;
                        hitNormal = localHitNormal;
                    }
                }
            }
        }
    }
}

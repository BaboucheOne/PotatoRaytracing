using PotatoRaytracing.Materials;
using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class IntersectionHandler
    {
        private PotatoSceneData sceneData;
        private readonly TextureManager textureManager;

        public IntersectionHandler(PotatoSceneData sceneData, TextureManager textureManager)
        {
            this.sceneData = sceneData;
            this.textureManager = textureManager;
        }

        public HitInfo Intersect(Ray ray)
        {
            double dstTriangle = 0.0;
            Vector3 hitPosTriangle = new Vector3();
            Vector3 hitNormalTriangle = new Vector3();
            bool hitTriangle = KDIntersection.Intersect(ray, sceneData.Tree.Root, ref hitPosTriangle, ref hitNormalTriangle, ref dstTriangle);

            double dstSphere = double.MaxValue;
            bool hitSphere = false;
            PotatoSphere sphere = null;
            Vector3 hitPosSphere = new Vector3();
            Vector3 hitNormalSphere = new Vector3();
            for (int i = 0; i < sceneData.Spheres.Count; i++)
            {
                double dst = 0.0;
                Vector3 hitPos = new Vector3();
                Vector3 hitNormal = new Vector3();
                bool hit = SphereIntersection.Intersect(ray, sceneData.Spheres[i], ref hitPos, ref hitNormal, ref dst);
                if(hit && (dst < dstSphere))
                {
                    hitSphere = true;
                    dstSphere = dst;
                    hitPosSphere = hitPos;
                    hitNormalSphere = hitNormal;
                    sphere = sceneData.Spheres[i];
                }
            }

            double dstPlane = double.MaxValue;
            bool hitPlane = false;
            Vector3 hitPosPlane = new Vector3();
            Vector3 hitNormalPlane = new Vector3();
            for (int i = 0; i < sceneData.Planes.Count; i++)
            {
                double dst = 0.0;
                Vector3 hitPos = sceneData.Planes[i].Position;
                Vector3 hitNormal = sceneData.Planes[i].Normal;
                bool hit = PlaneIntersection.Intersect(ray, sceneData.Planes[i], ref hitPos, ref hitNormal, ref dst);
                if (hit && (dst < dstPlane))
                {
                    hitPlane = true;
                    dstPlane = dst;
                    hitPosPlane = hitPos;
                    hitNormalPlane = hitNormal;
                }
            }

            if (hitTriangle && hitSphere && hitPlane) //TODO: Handle des plane
            {
                if (dstTriangle < dstSphere && dstTriangle < dstPlane) //TODO: Trouver le triangle intersect.
                {
                    return new HitInfo(true, ray, hitPosTriangle, hitNormalTriangle, dstTriangle, Color.White, new DefaultMaterial());
                }
                else
                {
                    return ProcessSphereHit(ray, hitPosSphere, hitNormalSphere, dstSphere, sphere);
                }
            }
            else
            {
                if (hitTriangle)
                {
                    return new HitInfo(true, ray, hitPosTriangle, hitNormalTriangle, dstTriangle, Color.White, new DefaultMaterial());
                }
                else if (hitSphere)
                {
                    return ProcessSphereHit(ray, hitPosSphere, hitNormalSphere, dstSphere, sphere);
                } else if(hitPlane)
                {
                    return new HitInfo(true, ray, hitPosPlane, hitNormalPlane, dstPlane, Color.White, new Lambertian(1f, Color.White));
                }
            }

            return new HitInfo(false, ray, new Vector3(), new Vector3(), 0f, Color.Black, new DefaultMaterial());
        }

        private HitInfo ProcessSphereHit(Ray ray, Vector3 hitPosSphere, Vector3 hitNormalSphere, double dstSphere, PotatoSphere sphere)
        {
            //Color hitColor = GetSphereTextureColor(sphere, hitNormalSphere); //TODO: A reactiver quand on pourra additionner les couleurs.
            //hitColor = hitColor.Add(sphere.Material.Color);

            Color hitColor = sphere.Material.Color;
            return new HitInfo(true, ray, hitPosSphere, hitNormalSphere, dstSphere, hitColor, sphere.Material);
        }

        private Color GetSphereTextureColor(PotatoSphere sphere, Vector3 normal)
        {
            string path = sphere.Material.AlbedoTexturePath;
            Bitmap texture = textureManager.GetTexture(path);
            return textureManager.GetTextureColor(sphere.GetUV(normal, texture), path);
        }
    }
}

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

            if (hitTriangle && hitSphere)
            {
                if (dstTriangle < dstSphere) //TODO: Trouver le triangle intersect.
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
                }
            }

            return new HitInfo(false, ray, new Vector3(), new Vector3(), 0f, Color.White, new DefaultMaterial());
        }

        private HitInfo ProcessSphereHit(Ray ray, Vector3 hitPosSphere, Vector3 hitNormalSphere, double dstSphere, PotatoSphere sphere)
        {
            Color textureColor = GetSphereTextureUV(sphere, hitNormalSphere);
            return new HitInfo(true, ray, hitPosSphere, hitNormalSphere, dstSphere, textureColor, sphere.Material);
        }

        private Color GetSphereTextureUV(PotatoSphere sphere, Vector3 normal)
        {
            string path = sphere.GetTexturePath();
            Bitmap texture = textureManager.GetTexture(sphere.GetTexturePath());
            return textureManager.GetTextureColor(sphere.GetUV(normal, texture), path);
        }
    }
}

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
                if(hit && dst < dstSphere)
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
                if (dstTriangle < dstSphere)
                {
                    return new HitInfo(true, hitPosTriangle, hitNormalTriangle, dstTriangle, Color.White, Color.Red);
                }
                else
                {
                    return ProcessSphereHit(hitNormalTriangle, dstSphere, sphere, hitPosSphere, hitNormalSphere);
                }
            }
            else
            {
                if (hitTriangle)
                {
                    return new HitInfo(true, hitPosTriangle, hitNormalTriangle, dstTriangle, Color.White, Color.Red);
                }
                else if (hitSphere)
                {
                    return ProcessSphereHit(hitNormalTriangle, dstSphere, sphere, hitPosSphere, hitNormalSphere);
                }
            }

            return new HitInfo(false, new Vector3(), new Vector3(), dstTriangle, Color.White, Color.Red);
        }

        private HitInfo ProcessSphereHit(Vector3 hitNormalTriangle, double dstSphere, PotatoSphere sphere, Vector3 hitPosSphere, Vector3 hitNormalSphere)
        {
            Color textureColor = GetSphereTextureUV(sphere, hitNormalTriangle);
            return new HitInfo(true, hitPosSphere, hitNormalSphere, dstSphere, textureColor, Color.Red);
        }

        private Color GetSphereTextureUV(PotatoSphere sphere, Vector3 normal)
        {
            string path = sphere.GetTexturePath();
            Bitmap texture = textureManager.GetTexture(sphere.GetTexturePath());
            return textureManager.GetTextureColor(sphere.GetUV(normal, texture), path);
        }
    }
}

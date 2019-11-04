using System;
using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoTracer
    {
        private Vector3 hitPosition;
        private Vector3 hitNormal;
        private PotatoScene scene;
        private TextureManager textureManager;
        private PotatoObject objectRender;
        private Color pixelColor = Color.Black;

        private string objectRenderTexturePath = string.Empty;
        private Bitmap objectRenderTexture = null;

        public PotatoTracer(PotatoScene scene, TextureManager textureManager)
        {
            this.scene = scene;
            this.textureManager = textureManager;
        }

        public Color Trace(Ray renderRay, int lightIndex, int depth)
        {
            pixelColor = Color.Black;

            objectRender = GetIntersectionObject(renderRay, out hitPosition, out hitNormal);
            if (objectRender == null) return pixelColor;

            SetObjectRenderUVProperties();
            ProcessUVTexture();

            pixelColor = ComputeLight(pixelColor, hitPosition, hitNormal, objectRender, scene.GetPointLight(lightIndex));

            return pixelColor;
        }

        private void SetObjectRenderUVProperties()
        {
            objectRenderTexturePath = objectRender.GetTexturePath();
            objectRenderTexture = textureManager.GetTexture(objectRenderTexturePath);
        }

        private void ProcessUVTexture()
        {
            Vector2 UV = objectRender.GetUV(hitNormal, objectRenderTexture);
            pixelColor = textureManager.GetTextureColor((int)UV.X, (int)UV.Y, objectRenderTexturePath);
        }

        private Vector3 ReflectRay(Ray renderRay, Vector3 hitNormal)
        {
            return 2 * hitNormal * Vector3.Dot(hitNormal, renderRay.Direction) - hitNormal;
        }

        private Color ComputeLight(Color finalColor, Vector3 hitPosition, Vector3 hitNormal, PotatoObject objectRender, PotatoPointLight light)
        {
            Vector3 directionToLight = light.GetDirection(hitPosition);
            Ray shadowRay = new Ray(hitPosition, directionToLight);

            if (light.InRange(hitPosition))
            {
                float normalAng = DiffuseAngle(hitPosition, hitNormal, light);

                if (normalAng > 0)
                {
                    finalColor = Color.FromArgb((int)Math.Round((light.Color.R + finalColor.R) * 0.5f * normalAng * light.Intensity),
                                (int)Math.Round((light.Color.G + finalColor.G) * 0.5f * normalAng * light.Intensity),
                                (int)Math.Round((light.Color.B + finalColor.B) * 0.5f * normalAng * light.Intensity));
                }
                else
                {
                    finalColor = Color.Black;
                }
            }

            return finalColor;
        }

        private float GetIntersectionDiscriminent(PotatoObject objectToRender, Ray ray)
        {
            return objectToRender.Intersect(ray.Position, ray.Direction).Discriminent;
        }

        private Vector3 CalculateHitNormal(PotatoObject objectToRender, Vector3 hitPosition)
        {
            return objectToRender.GetNormal(hitPosition);
        }

        private Vector3 CalculateHitPosition(float discr, Ray ray)
        {
            return ray.Cast(ray.Position, discr);
        }


        private PotatoObject GetClosestIntersectObject(Ray ray)
        {
            IntersectResult intersectResult = new IntersectResult();
            PotatoObject obj = null;
            float minD = float.PositiveInfinity;

            for (int i = 0; i < scene.GetPotatoObjectsCount(); i++)
            {
                intersectResult = scene.GetPotatoObject(i).Intersect(ray);
                if (intersectResult.Intersect && intersectResult.Discriminent < minD)
                {
                    minD = intersectResult.Discriminent;
                    obj = scene.GetPotatoObject(i);
                }
            }

            return obj;
        }

        public bool IsIntersect(PotatoObject obj, Ray ray)
        {
            return obj.Intersect(ray).Intersect;
        }

        private PotatoObject GetIntersectionObject(Ray ray, out Vector3 hitPosition, out Vector3 hitNormal)
        {
            hitPosition = new Vector3();
            hitNormal = new Vector3();

            PotatoObject obj = GetClosestIntersectObject(ray);
            if (obj == null) return obj;

            CalcultateHitPositionHitNormal(ray, out hitPosition, out hitNormal, obj);
            return obj;
        }

        private void CalcultateHitPositionHitNormal(Ray ray, out Vector3 hitPosition, out Vector3 hitNornal, PotatoObject obj)
        {
            float discr = GetIntersectionDiscriminent(obj, ray);
            hitPosition = CalculateHitPosition(discr, ray);
            hitNornal = CalculateHitNormal(obj, hitPosition);
        }

        public bool IsIntersectObjectInTheScene(Ray ray)
        {
            PotatoObject obj = GetClosestIntersectObject(ray);
            return obj != null;
        }

        public bool IntersectObjectInTheSceneExcept(PotatoObject obj, Ray ray)
        {
            foreach (PotatoObject objs in scene.GetPotatoObjects())
            {
                if (objs == obj) continue;

                if (objs.Intersect(ray).Intersect) return true;
            }

            return false;
        }

        public static float DiffuseAngle(Vector3 hitPoint, Vector3 normal, PotatoPointLight light)
        {
            Vector3 dir = Vector3.Normalize(Vector3.Subtract(light.Position, hitPoint));
            return Vector3.Dot(dir, normal);
        }
    }
}

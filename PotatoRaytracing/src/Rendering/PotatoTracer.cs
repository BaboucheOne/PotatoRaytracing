using System;
using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoTracer
    {
        public PotatoSceneData sceneData;
        private readonly TextureManager textureManager;
        private readonly IntersectionHandler intersectionHandler;

        private Color pixelColor = Color.Black;
        private Color globalColor = Color.Black;

        private string objectRenderTexturePath = string.Empty;
        private Bitmap objectRenderTexture = null;

        public PotatoTracer(PotatoSceneData sceneData, TextureManager textureManager)
        {
            this.sceneData = sceneData;
            this.textureManager = textureManager;
            intersectionHandler = new IntersectionHandler(sceneData);
        }

        public Color Trace(Ray renderRay, int lightIndex)
        {
            RecursiveTrace(renderRay, lightIndex, 0);
            return globalColor;
        }

        private Color RecursiveTrace(Ray ray, int lightIndex, int depth)
        {
            if (depth >= sceneData.Option.RecursionDepth)
            {
                globalColor = sceneData.Cubemap.GetCubemapColor(ray.Direction);
                return globalColor;
            }
            ClosestEntityIntersection result = intersectionHandler.GetClosestEntity(ray);

            if (result.IsNull)
            {
                globalColor = sceneData.Cubemap.GetCubemapColor(ray.Direction);
                return globalColor;
            }

            ray.SetDirection(ReflectRay(ray.Direction, result.HitNormal));

            if (result.IsMesh)
            {
                Color c = TraceTriangle(lightIndex, result as ClosestTriangle);
                globalColor = Color.FromArgb((globalColor.R + (byte)(c.R * 0.8f)) / 2, (globalColor.G + (byte)(c.G * 0.8f)) / 2, (globalColor.B + (byte)(c.B * 0.8f)) / 2);
                RecursiveTrace(ray, lightIndex, depth + 1);
            } else
            {
                Color c = TraceSphere(lightIndex, result as ClosestSphere);
                globalColor = Color.FromArgb((globalColor.R + (byte)(c.R * 0.25f)) / 2, (globalColor.G + (byte)(c.G * 0.25f)) / 2, (globalColor.B + (byte)(c.B * 0.25f)) / 2);
                RecursiveTrace(ray, lightIndex, depth + 1);
            }

            return globalColor;
        }

        private Color TraceTriangle(int lightIndex, ClosestTriangle result)
        {
            pixelColor = result.Mesh.Color;
            pixelColor = ComputeLight(pixelColor, result.HitPosition, result.HitNormal, sceneData.Lights[lightIndex]);

            return pixelColor;
        }

        private Color TraceSphere(int lightIndex, ClosestSphere result)
        {
            pixelColor = result.Sphere.Color;

            SetSphereUVProperties(result.Sphere);
            ProcessSphereUVTexture(result.Sphere, result.HitNormal);
            pixelColor = ComputeLight(pixelColor, result.HitPosition, result.HitNormal, sceneData.Lights[lightIndex]);

            return pixelColor;
        }

        //public Color Trace(Ray renderRay, int lightIndex, int depth)
        //{
        //    depth -= 1;
        //    //TODO: Rework sphere tracer.
        //    if (objectRender == null)
        //    {
        //        pixelColor = Color.Black;
        //        return pixelColor;
        //    }

        //    //if (depth > 0)
        //    //{
        //    //    Vector3 reflectDirection = ReflectRay(renderRay.Direction, hitNormal);
        //    //    renderRay.Set(hitPosition, reflectDirection);
        //    //    Trace(renderRay, lightIndex, depth);
        //    //}

        //    //objectRender = GetIntersectionObject(renderRay, out hitPosition, out hitNormal);
        //    //if (objectRender == null)
        //    //{
        //    //    pixelColor = Color.Black;
        //    //    return pixelColor;
        //    //}

        //    //pixelColor = objectRender.Color;
        //    SetObjectRenderUVProperties();
        //    ProcessUVTexture();
        //    pixelColor = ComputeLight(pixelColor, hitPosition, hitNormal, scene.GetPointLight(lightIndex));

        //    return pixelColor;
        //}

        private void SetSphereUVProperties(PotatoSphere sphere)
        {
            objectRenderTexturePath = sphere.GetTexturePath();
            objectRenderTexture = textureManager.GetTexture(objectRenderTexturePath);
        }

        private void ProcessSphereUVTexture(PotatoSphere sphere, Vector3 hitNormal)
        {
            Vector2 UV = sphere.GetUV(hitNormal, objectRenderTexture);
            pixelColor = textureManager.GetTextureColor(UV, objectRenderTexturePath);
        }

        private Vector3 ReflectRay(Vector3 originDirection, Vector3 hitNormal)
        {
            return originDirection - 2 * Vector3.Dot(originDirection, hitNormal) * hitNormal;
        }

        private Color ComputeLight(Color finalColor, Vector3 hitPosition, Vector3 hitNormal, PotatoPointLight light)
        {
            Vector3 directionToLight = light.GetDirection(hitPosition);

            if (light.InRange(hitPosition))
            {
                double normalAng = DiffuseAngle(hitPosition, hitNormal, light);

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

        public static double DiffuseAngle(Vector3 hitPoint, Vector3 normal, PotatoPointLight light)
        {
            Vector3 dir = Vector3.Normalize(light.Position - hitPoint);
            return Vector3.Dot(dir, normal);
        }
    }
}

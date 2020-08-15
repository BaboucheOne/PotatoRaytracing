using System;
using System.Drawing;
using System.DoubleNumerics;
using PotatoRaytracing.Materials;

namespace PotatoRaytracing
{
    public class PotatoTracer
    {
        public PotatoSceneData sceneData;
        private readonly IntersectionHandler intersectionHandler;

        //Peut etre pour faire un recursif ?
        private Color pixelColor = Color.Black;
        private Color globalColor = Color.Black;

        public PotatoTracer(PotatoSceneData sceneData, TextureManager textureManager)
        {
            this.sceneData = sceneData;
            intersectionHandler = new IntersectionHandler(sceneData, textureManager);
        }

        public Color Trace(Ray renderRay, int lightIndex)
        {
            HitInfo hitInfo = intersectionHandler.Intersect(renderRay);

            if (hitInfo.Hit)
            {
                //return ComputeAmbientOcclusion(hitInfo);
                //Diffuse.
                return ComputeDirectLight(hitInfo, sceneData.Lights[lightIndex]);
            }

            if (sceneData.Option.UseSolidColor) return sceneData.Option.SolidColor;

            return sceneData.Cubemap.GetCubemapColor(renderRay.Direction);
        }

        private Color ComputeAmbientOcclusion(HitInfo hit)
        {
            int sample = 256;
            float grey = 0;
            float pdf = 1f / (2f * (float)Math.PI);
            Ray r = new Ray();

            for (int i = 0; i < sample; i++)
            {
                Vector3 direction = Material.RandomUnitHemisphere(hit.HitNormal);
                r.Set(hit.HitPosition, direction);
                HitInfo hitAO = intersectionHandler.Intersect(r);

                if (hitAO.Hit)
                {
                    //grey += (int)Math.Max(0.0, hit.HitNormal.Angle(direction) * (1.0 / (1.0 + hitAO.Distance)));
                    grey += ((1f / sample) * (float)hit.HitNormal.Angle(direction) / pdf);
                }

            }

            grey /= sample;
            grey = grey.Clamp(0f, 255f) * 255;
            //Console.WriteLine(grey);
            return Color.FromArgb((int)grey, (int)grey, (int)grey);
        }

        private Color ComputeDirectLight(HitInfo hitInfo, PotatoLight light)
        {
            Vector3 dir2light = light.DirectionToLight(hitInfo.HitPosition);

            if (light.Intensity > 0 && light.IsInRange(hitInfo.HitPosition))
            {
                Ray shadowRay = new Ray(hitInfo.HitPosition + hitInfo.HitNormal * sceneData.Option.Bias, dir2light);
                HitInfo hitShadow = intersectionHandler.Intersect(shadowRay);

                if (hitShadow.Hit) return Color.Black;

                double lightIntensity = light.IntensityOverDistance(hitInfo.HitPosition);
                double diffuse = hitInfo.Material.Diffuse * Math.Max(0.0, Vector3.Dot(dir2light, hitInfo.HitNormal));

                Vector3 reflect = Vector3.Reflect(-dir2light, hitInfo.HitNormal);
                double specular = hitInfo.Material.Specular * Math.Pow(Math.Max(0.0, Vector3.Dot(-hitInfo.Ray.Direction, reflect)), hitInfo.Material.SpecularExp);

                double grey = lightIntensity * (diffuse + specular);
                grey = grey.Clamp(0.0, 255.0) / 255.0;

                return Color.FromArgb((int)Math.Round((hitInfo.Material.Color.R + light.Color.R) * 0.5 * grey),
                      (int)Math.Round((hitInfo.Material.Color.G + light.Color.G) * 0.5 * grey),
                      (int)Math.Round((hitInfo.Material.Color.B + light.Color.B) * 0.5 * grey));
            }

            return Color.Black;
        }

        //private Color RecursiveTrace(Ray ray, int lightIndex, int depth)
        //{
        //    if (depth == sceneData.Option.RecursionDepth)
        //    {
        //        globalColor = sceneData.Cubemap.GetCubemapColor(ray.Direction);
        //        return globalColor;
        //    }
        //    ClosestEntityIntersection result = intersectionHandler.GetClosestEntity(ray);

        //    if (result.IsNull)
        //    {
        //        globalColor = sceneData.Cubemap.GetCubemapColor(ray.Direction);
        //        return globalColor;
        //    }

        //    ray.SetDirection(ReflectRay(ray.Direction, result.HitNormal));

        //    if (result.IsMesh)
        //    {
        //        Color c = TraceTriangle(lightIndex, result as ClosestTriangle);
        //        globalColor = Color.FromArgb((globalColor.R + (byte)(c.R * 0.8f)) / 2, (globalColor.G + (byte)(c.G * 0.8f)) / 2, (globalColor.B + (byte)(c.B * 0.8f)) / 2);
        //        RecursiveTrace(ray, lightIndex, depth + 1);
        //    } else
        //    {
        //        Color c = TraceSphere(lightIndex, result as ClosestSphere);
        //        globalColor = Color.FromArgb((globalColor.R + (byte)(c.R * 0.25f)) / 2, (globalColor.G + (byte)(c.G * 0.25f)) / 2, (globalColor.B + (byte)(c.B * 0.25f)) / 2);
        //        RecursiveTrace(ray, lightIndex, depth + 1);
        //    }

        //    return globalColor;
        //}
    }
}

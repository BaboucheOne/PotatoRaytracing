using System;
using System.Drawing;
using System.DoubleNumerics;

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
                return ComputeLights(hitInfo.Color, hitInfo.HitPosition, hitInfo.HitNormal, sceneData.Lights[lightIndex]);
            }

            if (sceneData.Option.UseSolidColor) return sceneData.Option.SolidColor;

            return sceneData.Cubemap.GetCubemapColor(renderRay.Direction);
        }

        private Color ComputeLights(Color finalColor, Vector3 hitPosition, Vector3 hitNormal, PotatoLight light)
        {
            Vector3 directionToLight = light.DirectionToLight(hitPosition);
            Ray shadowRay = new Ray(hitPosition + hitNormal * sceneData.Option.Bias, directionToLight);

            if (light.Intensity > 0 && light.IsInRange(hitPosition))
            {
                HitInfo hit = intersectionHandler.Intersect(shadowRay);

                if (hit.Hit)
                {
                    Vector3 dir2light = light.DirectionToLight(hitPosition);
                    double normalAngleToLight = Vector3.Dot(dir2light, hitNormal);

                    if (normalAngleToLight > 0)
                    {
                        float lightPower = (float)(normalAngleToLight * light.IntensityOverDistance(hitPosition));
                        float albedo = 1f; //Toujours en dessous de 1.
                        float lightReflected = albedo / (float)Math.PI;

                        int r = (int)Math.Round((light.Color.R + finalColor.R) * 0.5f * lightPower * lightReflected);
                        int g = (int)Math.Round((light.Color.G + finalColor.G) * 0.5f * lightPower * lightReflected);
                        int b = (int)Math.Round((light.Color.B + finalColor.B) * 0.5f * lightPower * lightReflected);

                        return Color.FromArgb(r.Clamp(0, 255), g.Clamp(0, 255), b.Clamp(0, 255));
                    }
                }
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

        private Vector3 ReflectRay(Vector3 originDirection, Vector3 hitNormal)
        {
            return originDirection - 2 * Vector3.Dot(originDirection, hitNormal) * hitNormal;
        }

        public static double DiffuseAngle(Vector3 hitPoint, Vector3 normal, PotatoLight light)
        {
            Vector3 dir = Vector3.Normalize(light.Position - hitPoint);
            return Vector3.Dot(dir, normal);
        }
    }
}

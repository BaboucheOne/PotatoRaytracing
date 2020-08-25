using System;
using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoTracer
    {
        public PotatoSceneData sceneData;
        private readonly IntersectionHandler intersectionHandler;

        public PotatoTracer(PotatoSceneData sceneData, TextureManager textureManager)
        {
            this.sceneData = sceneData;
            intersectionHandler = new IntersectionHandler(sceneData, textureManager);
        }

        public Color Trace(Ray renderRay, int lightIndex, int depth)
        {
            if (depth > sceneData.Option.RecursionDepth) return GetBackgroundColor(renderRay);

            HitInfo hitInfo = intersectionHandler.Intersect(renderRay);

            if (hitInfo.Hit)
            {
                //return ComputeAmbientOcclusion(hitInfo);
                //Diffuse.
                return ComputeDirectLight(hitInfo, lightIndex, depth);
            }

            return GetBackgroundColor(renderRay);
        }

        private Color GetBackgroundColor(Ray renderRay)
        {
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

        private Color ComputeDirectLight(HitInfo hitInfo, int lightIndex, int depth)
        {
            PotatoLight light = sceneData.Lights[lightIndex];
            Vector3 dir2light = light.DirectionToLight(hitInfo.HitPosition);

            if (light.Intensity > 0 && light.IsInRange(hitInfo.HitPosition))
            {
                if (hitInfo.Material.Type == Material.MaterialType.Lit)
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

                    return Color.FromArgb((int)Math.Round((hitInfo.HitColor.R + light.Color.R) * 0.5 * grey),
                            (int)Math.Round((hitInfo.HitColor.G + light.Color.G) * 0.5 * grey),
                            (int)Math.Round((hitInfo.HitColor.B + light.Color.B) * 0.5 * grey));
                }

                if(hitInfo.Material.Type == Material.MaterialType.Reflective)
                {
                    double lightIntensity = light.IntensityOverDistance(hitInfo.HitPosition);
                    Vector3 reflectSpec = Vector3.Reflect(-dir2light, hitInfo.HitNormal);
                    double specular = hitInfo.Material.Specular * Math.Pow(Math.Max(0.0, Vector3.Dot(-hitInfo.Ray.Direction, reflectSpec)), hitInfo.Material.SpecularExp);

                    Vector3 reflectMiror = Vector3.Reflect(hitInfo.Ray.Direction, hitInfo.HitNormal);
                    Color reflectionColor = Trace(new Ray(hitInfo.HitPosition + hitInfo.HitNormal * sceneData.Option.Bias, reflectMiror), lightIndex, depth + 1).Multiply(hitInfo.Material.ReflectionWeight);

                    Vector3 customColor = new Vector3(lightIntensity * specular) + new Vector3(reflectionColor.R, reflectionColor.G, reflectionColor.B);
                    return Color.FromArgb((int)customColor.X.Clamp(0, 255), (int)customColor.Y.Clamp(0, 255), (int)customColor.Z.Clamp(0, 255));
                }

                if (hitInfo.Material.Type == Material.MaterialType.Refractive)
                {
                    bool inside = false;
                    Vector3 nHit = hitInfo.HitNormal;
                    if (Vector3.Dot(hitInfo.Ray.Direction, nHit) > 0.0)
                    {
                        nHit = -nHit;
                        inside = true;
                    }

                    if (depth < sceneData.Option.RecursionDepth)
                    {
                        float facingratio = (float)Vector3.Dot(-hitInfo.Ray.Direction, nHit);
                        float fresneleffect = (float)Mix(Math.Pow(1 - facingratio, 3), 1, 0.1);

                        Vector3 refldir = Vector3.Reflect(hitInfo.Ray.Direction, hitInfo.HitNormal);
                        Color reflection = Trace(new Ray(hitInfo.HitPosition + nHit * sceneData.Option.Bias, refldir), lightIndex, depth + 1);
                        Color refraction = Color.Black;

                        if (hitInfo.Material.Transparency > 0f)
                        {
                            double eta = inside ? hitInfo.Material.IndexOfRefraction : 1.0 / hitInfo.Material.IndexOfRefraction;

                            double cosi = Vector3.Dot(-nHit, hitInfo.Ray.Direction);
                            double k = 1.0 - eta * eta * (1.0 - cosi * cosi);
                            Vector3 refrdir = Vector3.Normalize(hitInfo.Ray.Direction * eta + nHit * (eta * cosi - Math.Sqrt(k)));
                            refraction = Trace(new Ray(hitInfo.HitPosition - nHit * sceneData.Option.Bias, refrdir), lightIndex, depth + 1);
                        }

                        Vector3 testColor = new Vector3(reflection.R, reflection.G, reflection.B) * fresneleffect;
                        testColor += new Vector3(refraction.R, refraction.G, refraction.B) * ((1.0 - fresneleffect) * hitInfo.Material.Transparency);

                        return Color.FromArgb((int)testColor.X.Clamp(0, 255), (int)testColor.Y.Clamp(0, 255), (int)testColor.Z.Clamp(0, 255));
                    }
                }
            }

            return Color.Black;
        }

        private static double Mix(double a, double b, double mix)
        {
            return b * mix + a * (1 - mix);
        }

        public static Vector3 Refract(Vector3 incendent, Vector3 normal, float refractionIndex)
        {
            refractionIndex = 2.0f - refractionIndex;
            double cosi = Vector3.Dot(normal, incendent);
            return Vector3.Normalize(incendent * refractionIndex - (normal * (-cosi + refractionIndex * cosi)));
        }
    }
}

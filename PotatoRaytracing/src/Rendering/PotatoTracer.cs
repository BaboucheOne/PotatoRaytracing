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
            if (depth > sceneData.Option.RecursionDepth) return Color.Black; //return GetBackgroundColor(renderRay);

            HitInfo hitInfo = intersectionHandler.Intersect(renderRay);

            if (hitInfo.Hit)
            {
                return ComputeDirectLight(hitInfo, lightIndex, depth);
            }

            return Color.FromArgb(255, 255, 255);
            //return GetBackgroundColor(renderRay);
        }

        private Color GetBackgroundColor(Ray renderRay)
        {
            if (sceneData.Option.UseSolidColor) return sceneData.Option.SolidColor;
            return sceneData.Cubemap.GetCubemapColor(renderRay.Direction);
        }

        private Vector3 ComputeGlobalIllumination(HitInfo hitInfo, int lightIndex, int depth)
        {
            if (depth == sceneData.Option.RecursionDepth) return Vector3.Zero; //TODO: Ajouter global illumination au option.

            int aoSample = 50;
            double pdf = 1 / (2 * Math.PI);
            Random rand = new Random();
            Vector3 indirectLight = new Vector3();
            Vector3 Nt = new Vector3();
            Vector3 Nb = new Vector3();
            for (int i = 0; i < aoSample; i++)
            {
                Material.CreateCoordinateSystem(hitInfo.HitNormal, ref Nt, ref Nb);

                double rand1 = rand.NextDouble();
                Vector3 sample = Material.UniformSampleHemisphere(rand1, rand.NextDouble());

                Vector3 sampleWorld = new Vector3(
                    sample.X * Nb.X + sample.Y * hitInfo.HitNormal.X + sample.Z * Nt.X,
                            sample.X * Nb.Y + sample.Y * hitInfo.HitNormal.Y + sample.Z * Nt.Y,
                            sample.X * Nb.Z + sample.Y * hitInfo.HitNormal.Z + sample.Z * Nt.Z);

                Color hitColor = Trace(new Ray(hitInfo.HitPosition + sampleWorld * sceneData.Option.Bias, sample), lightIndex, depth + 1);
                indirectLight += rand1 * new Vector3(hitColor.R, hitColor.G, hitColor.B) / pdf;
            }

            indirectLight = Vector3.Divide(indirectLight, aoSample);
            return indirectLight;
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
                    //if (hitShadow.Hit) return Color.Black;

                    double lightIntensity = light.IntensityOverDistance(hitInfo.HitPosition);
                    double diffuse = Convert.ToDouble(!hitShadow.Hit) * hitInfo.Material.Diffuse * Math.Max(0.0, Vector3.Dot(dir2light, hitInfo.HitNormal));

                    Vector3 reflect = Vector3.Reflect(-dir2light, hitInfo.HitNormal);
                    double specular = Convert.ToDouble(!hitShadow.Hit) * hitInfo.Material.Specular * Math.Pow(Math.Max(0.0, Vector3.Dot(-hitInfo.Ray.Direction, reflect)), hitInfo.Material.SpecularExp);

                    double grey = lightIntensity * (diffuse + specular);
                    grey = grey.Clamp(0.0, 255.0) / 255.0;

                    Vector3 directLight = new Vector3((hitInfo.HitColor.R + light.Color.R) * 0.5f * grey,
                            (hitInfo.HitColor.G + light.Color.G) * 0.5f * grey,
                            (hitInfo.HitColor.B + light.Color.B) * 0.5f * grey);

                    //Color colorDIRECT = Color.FromArgb((int)directLight.X, (int)directLight.Y, (int)directLight.Z);

                    //Color directColor = Color.FromArgb((int)Math.Round((hitInfo.HitColor.R + light.Color.R) * 0.5 * grey),
                    //        (int)Math.Round((hitInfo.HitColor.G + light.Color.G) * 0.5 * grey),
                    //        (int)Math.Round((hitInfo.HitColor.B + light.Color.B) * 0.5 * grey));

                    Vector3 indirectLight = ComputeGlobalIllumination(hitInfo, lightIndex, depth);
                    //return indirectLight.ToColor();

                    Vector3 globalLight = indirectLight;//(directLight / Math.PI + 2 * indirectLight) * 0.25;
                    globalLight.X = globalLight.X.Clamp(0, 255);
                    globalLight.Y = globalLight.Y.Clamp(0, 255);
                    globalLight.Z = globalLight.Z.Clamp(0, 255);
                    return Color.FromArgb((int)globalLight.X, (int)globalLight.Y, (int)globalLight.Z);

                    indirectLight.X = indirectLight.X.Clamp(0, 255);
                    indirectLight.Y = indirectLight.Y.Clamp(0, 255);
                    indirectLight.Z = indirectLight.Z.Clamp(0, 255);
                    //Console.WriteLine(indirectColor);
                    Color aoNowColor = Color.FromArgb((int)indirectLight.X, (int)indirectLight.Y, (int)indirectLight.Z);
                    return aoNowColor;
                    //return colorDIRECT.Add(aoNowColor);
                    //return aoNowColor;

                    //Vector3 endColor = (directLight / Math.PI + 2 * indirectLight) * 0.5f;

                    //endColor.X = indirectLight.X.Clamp(0, 255);
                    //endColor.Y = indirectLight.Y.Clamp(0, 255);
                    //endColor.Z = indirectLight.Z.Clamp(0, 255);
                    //return Color.FromArgb((int)endColor.X, (int)endColor.Y, (int)endColor.Z);
                    //return directColor.Add(aoNowColor);
                }

                if (hitInfo.Material.Type == Material.MaterialType.Reflective)
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

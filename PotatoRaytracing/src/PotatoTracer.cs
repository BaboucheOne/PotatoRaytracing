﻿using System;
using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoTracer
    {
        private Vector3 hitPosition;
        private Vector3 hitNormal;
        private bool isIntersect;
        private Triangle triangleIntersect;
        private PotatoMesh meshIntersect;

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

        public Color Trace(Ray renderRay, int lightIndex)
        {
            pixelColor = Color.Black;
            isIntersect = false;
            triangleIntersect = null;

            ClosestIntersectionResult result = GetClosestTriangleIntersection(ref renderRay, ref isIntersect, ref hitPosition, ref hitNormal);

            if (isIntersect)
            {
                triangleIntersect = result.Triangle;
                meshIntersect = result.Mesh;

                pixelColor = meshIntersect.Color;
                pixelColor = ComputeLight(pixelColor, hitPosition, hitNormal, scene.GetPointLight(lightIndex));
            } else
            {
                pixelColor = Color.Black;
            }

            return pixelColor;
        }

        private ClosestIntersectionResult GetClosestTriangleIntersection(ref Ray ray, ref bool intersect, ref Vector3 outHitPosition, ref Vector3 outHitNormal)
        {
            Triangle triangleInt = null;
            PotatoMesh meshInt = null;
            double distance = float.PositiveInfinity;
            intersect = false;
            double t = 0;

            for (int i = 0; i < scene.MeshCout; i++)
            {
                PotatoMesh mesh = scene.GetPotatoMesh(i);

                for (int j = 0; j < mesh.GetTrianglesCount; j++)
                {
                    if (TriangleIntersection.RayIntersectsTriangle(ray.Origin, ray.Direction, mesh.GetTriangle(j), ref outHitPosition, ref outHitNormal, ref t))
                    {
                        if (t < distance)
                        {
                            distance = t;
                            triangleInt = mesh.GetTriangle(j);
                            meshInt = mesh;
                        }

                        intersect = true;
                    }
                }
            }

            return new ClosestIntersectionResult(triangleInt, meshInt);
        }

        public Color Trace(Ray renderRay, int lightIndex, int depth)
        {
            depth -= 1;
            //TODO: Rework sphere tracer.
            if (objectRender == null)
            {
                pixelColor = Color.Black;
                return pixelColor;
            }

            //if (depth > 0)
            //{
            //    Vector3 reflectDirection = ReflectRay(renderRay.Direction, hitNormal);
            //    renderRay.Set(hitPosition, reflectDirection);
            //    Trace(renderRay, lightIndex, depth);
            //}

            //objectRender = GetIntersectionObject(renderRay, out hitPosition, out hitNormal);
            //if (objectRender == null)
            //{
            //    pixelColor = Color.Black;
            //    return pixelColor;
            //}

            //pixelColor = objectRender.Color;
            SetObjectRenderUVProperties();
            ProcessUVTexture();
            pixelColor = ComputeLight(pixelColor, hitPosition, hitNormal, scene.GetPointLight(lightIndex));

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

        private Vector3 ReflectRay(Vector3 originDirection, Vector3 hitNormal)
        {
            return originDirection - 2 * Vector3.Dot(originDirection, hitNormal) * hitNormal;
        }

        private Color ComputeLight(Color finalColor, Vector3 hitPosition, Vector3 hitNormal, PotatoPointLight light)
        {
            Vector3 directionToLight = light.GetDirection(hitPosition);
            //Ray shadowRay = new Ray(hitPosition, directionToLight);

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

        //private PotatoObject GetClosestIntersectObject(Ray ray)
        //{
        //    IntersectResult intersectResult = new IntersectResult();
        //    PotatoObject obj = null;
        //    float minD = float.PositiveInfinity;

        //    for (int i = 0; i < scene.PotatoObjectsCount; i++)
        //    {
        //        intersectResult = scene.GetPotatoObject(i).Intersect(ray);
        //        if (intersectResult.Intersect && intersectResult.Discriminent < minD)
        //        {
        //            minD = intersectResult.Discriminent;
        //            obj = scene.GetPotatoObject(i);
        //        }
        //    }

        //    return obj;
        //}

        public static double DiffuseAngle(Vector3 hitPoint, Vector3 normal, PotatoPointLight light)
        {
            Vector3 dir = Vector3.Normalize(light.Position - hitPoint);
            return Vector3.Dot(dir, normal);
        }

        public Vector3 GetHitPosition() => hitPosition;
        public Vector3 GetHitNormal() => hitNormal;
    }

    internal struct ClosestIntersectionResult
    {
        public readonly Triangle Triangle;
        public readonly PotatoMesh Mesh;

        public ClosestIntersectionResult(Triangle triangle, PotatoMesh mesh)
        {
            Triangle = triangle;
            Mesh = mesh;
        }
    }
}

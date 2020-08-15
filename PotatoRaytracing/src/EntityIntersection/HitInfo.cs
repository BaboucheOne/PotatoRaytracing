﻿using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public struct HitInfo
    {
        public readonly bool Hit;
        public readonly Ray Ray;
        public readonly Vector3 HitPosition;
        public readonly Vector3 HitNormal;
        public readonly double Distance;
        public readonly Color TextureColor;
        public readonly Material Material;

        public HitInfo(bool hit, Ray ray, Vector3 hitPosition, Vector3 hitNormal, double distance, Color textureColor, Material material)
        {
            Hit = hit;
            Ray = ray;
            HitPosition = hitPosition;
            HitNormal = hitNormal;
            Distance = distance;
            TextureColor = textureColor;
            Material = material;
        }
    }
}
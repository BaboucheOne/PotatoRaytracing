﻿using System.Drawing;

namespace PotatoRaytracing.Materials
{
    public class Lambertian : Material
    {
        public Lambertian(float diffuse, Color color) : base(diffuse, 0f, color, 0)
        {
        }
    }
}
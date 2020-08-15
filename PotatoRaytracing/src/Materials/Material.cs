using System;
using System.Drawing;
using System.DoubleNumerics;
using PotatoRaytracing.Materials;

namespace PotatoRaytracing
{
    public abstract class Material
    {
        private float specular = 0.2f;
        private float diffuse = 0.8f;
        private int specularExp = 10;

        public readonly Color Color = Color.White;
        public readonly float Albedo = 0.18f;
        public int SpecularExp
        {
            get
            {
                return specularExp;
            }
            set
            {
                if (value > 1200) specularExp = 1200;
            }
        }
        public float Diffuse
        {
            get
            {
                return diffuse;
            }
            set
            {
                if (value > 1f) diffuse = 1f;
            }
        }
        public float Specular
        {
            get
            {
                return specular;
            }
            set
            {
                if (value > 1f) specular = 1f;
            }
        }

        public Material()
        {
        }

        public Material(float diffuse, float specular, Color color, int specularExp = 10, float albedo = 0.18f)
        {
            this.diffuse = diffuse;
            this.specular = specular;
            this.specularExp = specularExp;
            Color = color;
            Albedo = albedo;
        }

        public virtual HitInfo Scatter(Ray ray) //TODO: Ajouter la position de base.
        {
            return new HitInfo(false, ray, new Vector3(), new Vector3(), 0.0, Color.White, new DefaultMaterial());
        }

        public static Vector3 RandomUnitSphere()
        {
            Random r = new Random();
            Vector3 p = new Vector3();

            while (p.LengthSquared() < 1.0) p = 2.0 * new Vector3(r.NextDouble(), r.NextDouble(), r.NextDouble()) - Vector3.One;

            return p;
        }

        public static Vector3 RandomUnitHemisphere(Vector3 normal)
        {
            Vector3 randomSphere = RandomUnitSphere();
            if (Vector3.Dot(randomSphere, normal) > 0.0) return randomSphere;
            return -randomSphere;
        }
    }
}

using System;
using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public abstract class Material
    {
        private const string defaultAlbedoTexturePath = @"Resources\Textures\default.bmp";

        public string AlbedoTexturePath = string.Empty;
        public string NormalMapPath = string.Empty;

        public enum MaterialType { Lit, Reflective, Refractive }

        public Color Color = Color.White;
        public float Albedo = 0.18f;

        protected float specular = 0.2f;
        protected float diffuse = 0.8f;
        protected int specularExp = 10;

        protected float reflectionWeight = 1f;
        protected float transparency = 1f;
        protected double indexOfRefraction = 1f;

        public MaterialType Type = MaterialType.Lit;
        public double IndexOfRefraction
        {
            get
            {
                return indexOfRefraction;
            }
            set
            {
                indexOfRefraction.Clamp(1f, double.MaxValue);
            }
        }
        public float ReflectionWeight
        {
            get
            {
                return reflectionWeight;
            }
            set
            {
                reflectionWeight.Clamp(0f, 1f);
            }
        }
        public float Transparency
        {
            get
            {
                return transparency;
            }
            set
            {
                transparency.Clamp(0f, 1f);
            }
        }

        public int SpecularExp
        {
            get
            {
                return specularExp;
            }
            set
            {
                specularExp.Clamp(0, 1200);
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
                diffuse.Clamp(0f, 1f);
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
                specular.Clamp(0f, 1f);
            }
        }

        public Material()
        {
            AlbedoTexturePath = defaultAlbedoTexturePath;
        }

        public Material(MaterialType type)
        {
            Type = type;
            AlbedoTexturePath = defaultAlbedoTexturePath;
        }

        public Material(float diffuse, float specular, Color color, MaterialType type = MaterialType.Lit, int specularExp = 10, float albedo = 0.18f)
        {
            Type = type;
            this.diffuse = diffuse;
            this.specular = specular;
            this.specularExp = specularExp;
            Color = color;
            Albedo = albedo;

            AlbedoTexturePath = defaultAlbedoTexturePath;
        }

        public bool HasNormalMap => !string.IsNullOrEmpty(NormalMapPath);

        public virtual HitInfo Scatter(HitInfo info) //TODO: Ajouter la position de base.
        {
            return info;
        }

        public static void CreateCoordinateSystem(Vector3 N, ref Vector3 Nt, ref Vector3 Nb)
        {
            if (Math.Abs(N.X) > Math.Abs(N.Y))
            {
                Nt = new Vector3(N.Z, 0, -N.X) / Math.Sqrt(N.X * N.X + N.Z * N.Z);
            }
            else
            {
                Nt = new Vector3(0, -N.Z, N.Y) / Math.Sqrt(N.Y * N.Y + N.Z * N.Z);
            }

            Nb = Vector3.Cross(N, Nt);
        }

        public static Vector3 UniformSampleHemisphere(double r1, double r2)
        {
            double sinTheta = Math.Sqrt(1 - r1 * r1);
            double phi = 2.0 * Math.PI * r2;
            double x = sinTheta * Math.Cos(phi);
            double z = sinTheta * Math.Sin(phi);

            return new Vector3(x, r1, z);
        }

        //public static Vector3 RandomUnitSphere() //TODO: Deplacer dans un autre fichier.
        //{
        //    Random r = new Random();
        //    Vector3 p = new Vector3();

        //    while (p.LengthSquared() < 1.0) p = 2.0 * new Vector3(r.NextDouble(), r.NextDouble(), r.NextDouble()) - Vector3.One;

        //    return p;
        //}

        //public static Vector3 RandomUnitHemisphere(Vector3 normal) //TODO: Deplacer dans un autre fichier.
        //{
        //    //Vector3 randomSphere = RandomUnitSphere();
        //    //if (Vector3.Dot(randomSphere, normal) > 0.0) return randomSphere;
        //    //return -randomSphere;

        //    Random rand = new Random();

        //    double u = rand.NextDouble();
        //    double v = rand.NextDouble();

        //    double phi = v * 2.0 * Math.PI;
        //    double cosTheta = Math.Sqrt(1.0 - u);
        //    double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);
        //    return new Vector3(Math.Cos(phi) * sinTheta, Math.Sin(phi) * sinTheta, cosTheta);

        //    //double randY = rand.NextDouble();
        //    //double xt = rand.NextDouble() * 2 * Math.PI; //expand to 0 to 2PI
        //    //double yt = Math.Sqrt(1.0 - randY);

        //    //double x = Math.Cos(xt) * yt;
        //    //double y = Math.Sqrt(randY);
        //    //double z = Math.Sin(xt) * yt;

        //    //Vector3 randomSphere = new Vector3(x, y, z);
        //    //if (Vector3.Dot(randomSphere, normal) > 0.0) return randomSphere;
        //    //return -randomSphere;
        //}
    }
}

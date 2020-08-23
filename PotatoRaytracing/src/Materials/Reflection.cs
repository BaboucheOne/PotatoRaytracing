using System.DoubleNumerics;

namespace PotatoRaytracing.Materials
{
    public class Reflection : Material
    {
        public Reflection() : base(0f, 0.5f, System.Drawing.Color.White, MaterialType.Reflective)
        {
            specularExp = 150;
        }

        public Reflection(float reflectionWeight, float specular, int specularExp) : base(MaterialType.Reflective)
        {
            this.reflectionWeight = reflectionWeight;
            this.specular = specular;
            this.specularExp = specularExp;
        }

        public override HitInfo Scatter(HitInfo info)
        {
            Vector3 reflect = Vector3.Reflect(info.Ray.Direction, info.HitNormal);
            return info;
        }
    }
}

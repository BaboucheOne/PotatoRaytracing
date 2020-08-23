namespace PotatoRaytracing.Materials
{
    public class Refraction : Material
    {
        public Refraction() : base(0f, 0f, System.Drawing.Color.White, MaterialType.Refractive)
        {
        }

        public Refraction(double indexOfRefraction, float transparency = 1f) : base(MaterialType.Refractive)
        {
            this.indexOfRefraction = indexOfRefraction;
            this.transparency = transparency;
        }
    }
}

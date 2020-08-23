using System.Drawing;

namespace PotatoRaytracing.Materials
{
    public class DefaultMaterial : Material
    {
        public DefaultMaterial()
        {
        }

        public DefaultMaterial(float diffuse, float specular, Color color, int specularExp, float albedo) : base(diffuse, specular, color, MaterialType.Lit, specularExp, albedo)
        {
        }
    }
}

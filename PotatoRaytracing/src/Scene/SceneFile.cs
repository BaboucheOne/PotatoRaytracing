using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class SceneFile
    {
        public PotatoSphere[] Spheres { get; set; }
        public PotatoMesh[] Meshes { get; set; }
        public PotatoPlane[] Planes { get; set; }
        public PotatoPointLight[] PointLights { get; set; }
        public PotatoDirectionalLight[] DirectionalLights { get; set; }

        public PotatoLight[] GetLigths()
        {
            List<PotatoLight> ligths = new List<PotatoLight>();
            ligths.AddRange(PointLights);
            ligths.AddRange(DirectionalLights);

            return ligths.ToArray();
        }
    }
}

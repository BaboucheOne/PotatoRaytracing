namespace PotatoRaytracing
{
    public class SceneFile
    {
        public PotatoSphere[] Spheres;
        public PotatoMesh[] Meshes;
        public PotatoLight[] PointLights;

        public SceneFile()
        {
        }

        public SceneFile(PotatoSphere[] spheres, PotatoMesh[] meshes, PotatoLight[] pointLights)
        {
            Spheres = spheres;
            Meshes = meshes;
            PointLights = pointLights;
        }
    }
}

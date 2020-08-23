namespace PotatoRaytracing
{
    public class SceneFile
    {
        public PotatoSphere[] Spheres;
        public PotatoMesh[] Meshes;
        public PotatoPointLight[] PointLights;

        public SceneFile()
        {
        }

        public SceneFile(PotatoSphere[] spheres, PotatoMesh[] meshes, PotatoPointLight[] pointLights)
        {
            Spheres = spheres;
            Meshes = meshes;
            PointLights = pointLights;
        }
    }
}

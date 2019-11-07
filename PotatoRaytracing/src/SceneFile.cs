namespace PotatoRaytracing
{
    public class SceneFile
    {
        public PotatoMesh[] Meshes;
        public PotatoPointLight[] PointLights;

        public SceneFile()
        {
        }

        public SceneFile(PotatoMesh[] meshes, PotatoPointLight[] pointLights)
        {
            Meshes = meshes;
            PointLights = pointLights;
        }
    }
}

using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class ObjBaker
    {
        private readonly PotatoObjParser potatoObjParser = new PotatoObjParser();

        public ObjBaker()
        {
        }

        public void Build(ref List<PotatoMesh> meshes)
        {
            if (meshes.Count == 0) return;
            BakeMeshes(meshes);
        }

        private void BakeMeshes(List<PotatoMesh> meshs)
        {
            for (int i = 0; i < meshs.Count; i++)
            {
                BakeMesh(meshs[i]);
            }
        }

        private void BakeMesh(PotatoMesh mesh)
        {
            potatoObjParser.Parse(ref mesh);
            mesh.SetPosition(mesh.Position);
        }
    }
}

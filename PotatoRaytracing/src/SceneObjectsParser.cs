using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace PotatoRaytracing
{
    public class SceneObjectsParser
    {
        private IObjLoader objLoader = new ObjLoaderFactory().Create();
        private LoadResult loadResult = null;

        public SceneObjectsParser()
        {
        }

        public void Parse(ref List<PotatoMesh> meshes)
        {
            BakeAllMeshes(meshes);
        }

        private void BakeAllMeshes(List<PotatoMesh> meshes)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                PotatoMesh mesh = GetMesh(meshes[i].ObjectPath);
                meshes[i].SetTriangles(mesh.GetTriangles());
                meshes[i].BakeMesh();
            }
        }

        private PotatoMesh GetMesh(string path)
        {
            List<Triangle> triangles = new List<Triangle>();
            ReadAndLoadObjectFile(path);
            AttributeTrianglesVertexToMesh(triangles);

            return new PotatoMesh(triangles.ToArray());
        }

        private void AttributeTrianglesVertexToMesh(List<Triangle> triangles)
        {
            for (int i = 0; i < loadResult.Vertices.Count - 3; i += 3)
            {
                triangles.Add(new Triangle(VertexToVector3(loadResult.Vertices[i]), VertexToVector3(loadResult.Vertices[i + 1]), VertexToVector3(loadResult.Vertices[i + 2]),
                                                        NormalToVector3(loadResult.Normals[i]), NormalToVector3(loadResult.Normals[i + 1]), NormalToVector3(loadResult.Normals[i + 2])));
            }
        }

        private void ReadAndLoadObjectFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            loadResult = objLoader.Load(fileStream);
            fileStream.Close();
        }

        private Vector3 VertexToVector3(Vertex vertex)
        {
            return new Vector3(vertex.X, vertex.Y, vertex.Z);
        }

        private Vector3 NormalToVector3(Normal normal)
        {
            return new Vector3(normal.X, normal.Y, normal.Z);
        }
    }
}

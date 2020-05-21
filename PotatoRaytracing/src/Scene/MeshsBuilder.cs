using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using System.Collections.Generic;
using System.IO;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class MeshsBuilder
    {
        private IObjLoader loadFactory = null;
        private LoadResult loadResult = null;
        private const int verticesCount = 3;
        private const int verticesGap = 1;

        public MeshsBuilder()
        {
        }

        public void Build(ref List<PotatoMesh> meshes)
        {
            if (meshes.Count == 0) return;

            loadFactory = new ObjLoaderFactory().Create();
            BakeAllMeshes(meshes);
        }

        private void BakeAllMeshes(List<PotatoMesh> meshs)
        {
            ReadAllObjFiles(meshs);
            BuildMeshs(meshs);
        }

        private void ReadAllObjFiles(List<PotatoMesh> meshs)
        {
            for (int i = 0; i < meshs.Count; i++)
            {
                ReadAndLoadObjFileInLoadFactory(meshs[i].ObjectPath);
            }
        }

        private void BuildMeshs(List<PotatoMesh> meshs)
        {
            List<Triangle> triangles = new List<Triangle>();

            for (int i = 0; i < loadResult.Groups.Count; i++)
            {
                Group group = loadResult.Groups[i];
                int faces = group.Faces.Count;

                triangles.Clear();
                for (int j = 0; j < faces; j++)
                {
                    Vector3[] triangleVertices = new Vector3[3];
                    Vector3[] triangleNormals = new Vector3[3];
                    for (int k = 0; k < verticesCount; k++)
                    {
                        int vertexIndex = group.Faces[j][k].VertexIndex - verticesGap;
                        int normalIndex = group.Faces[j][k].NormalIndex - verticesGap;

                        triangleVertices[k] = VertexToVector3(loadResult.Vertices[vertexIndex]);
                        //triangleNormals[k] = VertexToVector3(loadResult.Vertices[normalIndex]);
                    }

                    triangles.Add(new Triangle(triangleVertices, triangleNormals));
                }

                meshs[i].SetTriangles(triangles.ToArray());
                meshs[i].SetPosition(meshs[i].Position);
            }
        }

        private void ReadAndLoadObjFileInLoadFactory(string path)
        {
            
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            loadResult = loadFactory.Load(fileStream);
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

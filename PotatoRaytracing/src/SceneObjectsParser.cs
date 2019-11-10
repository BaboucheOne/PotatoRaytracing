using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace PotatoRaytracing
{
    public class SceneObjectsParser
    {
        private LoadResult loadResult = null;
        private const int verticesCount = 3;
        private const int verticesGap = 1;

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
            for (int i = 0; i < loadResult.Groups.Count; i++)
            {
                Group group = loadResult.Groups[i];
                int faces = group.Faces.Count;
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
            }
        }

        private void ReadAndLoadObjectFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            loadResult = new ObjLoaderFactory().Create().Load(fileStream);
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

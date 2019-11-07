using System.Numerics;

namespace PotatoRaytracing
{
    public class Triangle
    {
        private Vector3[] vertices = new Vector3[3];
        private Vector3[] normals = new Vector3[3];

        public Triangle(Vector3[] vertices, Vector3[] normals)
        {
            this.vertices = vertices;
            this.normals = normals;
        }

        public Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1, Vector3 normal2)
        {
            vertices[0] = vertex0;
            vertices[1] = vertex1;
            vertices[2] = vertex2;

            normals[0] = normal0;
            normals[1] = normal1;
            normals[2] = normal2;
        }

        public void SetPosition(Vector3 position)
        {
            for(int i = 0; i < 3; i++)
            {
                SetVertexPosition(i, position);
            }
        }

        private void SetVertexPosition(int vertexID, Vector3 position)
        {
            vertices[vertexID] = Vector3.Add(vertices[vertexID], position);
        }

        public Vector3 GetVertex0() => vertices[0];
        public Vector3 GetVertex1() => vertices[1];
        public Vector3 GetVertex2() => vertices[2];

        public Vector3 GetNormal0() => normals[0];
        public Vector3 GetNormal1() => normals[1];
        public Vector3 GetNormal2() => normals[2];
    }
}
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class Triangle
    {
        private Vector3[] vertices = new Vector3[3];
        private Vector3[] normals = new Vector3[3];
        private Vector3 normal = new Vector3();

        public Triangle(Vector3[] vertices)
        {
            this.vertices = vertices;

            ComputeNormal();
        }

        public Triangle(Vector3[] vertices, Vector3[] normals)
        {
            this.vertices = vertices;
            this.normals = normals;

            ComputeNormal();
        }

        public Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1, Vector3 normal2)
        {
            vertices[0] = vertex0;
            vertices[1] = vertex1;
            vertices[2] = vertex2;

            normals[0] = normal0;
            normals[1] = normal1;
            normals[2] = normal2;

            ComputeNormal();
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

            ComputeNormal();
        }

        private void ComputeNormal()
        {
            Vector3 edge1 = Vector3.Subtract(vertices[1], vertices[0]);
            Vector3 edge2 = Vector3.Subtract(vertices[2], vertices[0]);
            normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
        }

        public Vector3 GetVertex0() => vertices[0];
        public Vector3 GetVertex1() => vertices[1];
        public Vector3 GetVertex2() => vertices[2];

        public Vector3 GetVertex0Normal() => normals[0];
        public Vector3 GetVertex1Normal() => normals[1];
        public Vector3 GetVertex2Normal() => normals[2];

        public Vector3 GetNormal() => normal;
    }
}
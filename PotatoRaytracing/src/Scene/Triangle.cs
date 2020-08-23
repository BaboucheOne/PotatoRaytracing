using System;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class Triangle
    {
        private Vector3[] vertices = new Vector3[3];
        private Vector3[] normals = new Vector3[3];
        public Vector3 Normal { get; private set; } = new Vector3();

        public Vector3 Min = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
        public Vector3 Max = new Vector3();
        public Vector3 Center = new Vector3();

        public Vector3 V0 => vertices[0];
        public Vector3 V1 => vertices[1];
        public Vector3 V2 => vertices[2];

        public Vector3 N0 => normals[0];
        public Vector3 N1 => normals[1];
        public Vector3 N2 => normals[2];

        public Triangle(Vector3[] vertices)
        {
            this.vertices = vertices;
            RecalculateAll();
        }

        public Triangle(Vector3[] vertices, Vector3[] normals) : this(vertices)
        {
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

            RecalculateAll();
        }

        private void RecalculateAll()
        {
            ComputeBounds();
            ComputeCentroid();
            ComputeNormal();
        }

        public void SetVerticesPosition(Vector3 position)
        {
            for(int i = 0; i < 3; i++)
            {
                SetVertexPosition(i, position);
            }
        }

        private void SetVertexPosition(int vertexID, Vector3 position)
        {
            vertices[vertexID] = Vector3.Add(vertices[vertexID], position);
            RecalculateAll();
        }

        private void ComputeNormal()
        {
            Vector3 edge1 = Vector3.Subtract(vertices[1], vertices[0]);
            Vector3 edge2 = Vector3.Subtract(vertices[2], vertices[0]);
            Normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
        }

        private void ComputeCentroid()
        {
            Center = (Max + Min) * 0.5f;
        }

        public Vector3 MidPoint => new Vector3
        {
            X = (vertices[0].X + vertices[1].X + vertices[2].X) / 3.0,
            Y = (vertices[0].Y + vertices[1].Y + vertices[2].Y) / 3.0,
            Z = (vertices[0].Z + vertices[1].Z + vertices[2].Z) / 3.0,
        };

        private void ComputeBounds()
        {
            Min.X = Math.Min(vertices[0].X, Math.Min(vertices[1].X, vertices[2].X));
            Min.Y = Math.Min(vertices[0].Y, Math.Min(vertices[1].Y, vertices[2].Y));
            Min.Z = Math.Min(vertices[0].Z, Math.Min(vertices[1].Z, vertices[2].Z));

            Max.X = Math.Max(vertices[0].X, Math.Max(vertices[1].X, vertices[2].X));
            Max.Y = Math.Max(vertices[0].Y, Math.Max(vertices[1].Y, vertices[2].Y));
            Max.Z = Math.Max(vertices[0].Z, Math.Max(vertices[1].Z, vertices[2].Z));
        }
    }
}


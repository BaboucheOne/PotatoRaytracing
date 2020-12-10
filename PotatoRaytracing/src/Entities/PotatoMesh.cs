using System.Drawing;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class PotatoMesh : PotatoEntity
    {
        private Triangle[] triangles;
        public string TexturePath;
        public string ObjectPath;
        public Color Color = Color.White;
        public Vector3 Center { get; private set; }

        public Triangle GetTriangle(int index) => triangles[index];
        public Triangle[] GetTriangles() => triangles;
        public int GetTrianglesCount() => triangles.Length;

        public PotatoMesh() { }

        public PotatoMesh(string texturePath)
        {
            TexturePath = texturePath;
        }

        public PotatoMesh(Triangle[] triangles)
        {
            this.triangles = triangles;
        }

        public PotatoMesh(Vector3 position, Triangle[] triangles) : base(position)
        {
            this.triangles = triangles;
        }

        public PotatoMesh(Vector3 position, Quaternion rotation, Triangle[] triangles) : base(position, rotation)
        {
            this.triangles = triangles;
        }

        public void SetTriangles(Triangle[] triangles)
        {
            this.triangles = triangles;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;

            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].SetVerticesPosition(Position);
            }

            CalculateCenter();
        }

        private void CalculateCenter()
        {
            Center = Vector3.Zero;
            double sumArea = 0.0;

            for (int i = 0; i < triangles.Length; i++)
            {
                Vector3 center = Vector3.Divide(triangles[i].V0 + triangles[i].V1 + triangles[i].V2, 3);
                double length = Vector3.Cross(triangles[i].V1 - triangles[i].V0, triangles[i].V2 - triangles[i].V0).Length();
                double area = 0.5 * length;
                Center += area * center;
                sumArea += area;
            }

            Center = Vector3.Divide(Center, sumArea);
        }
    }
}

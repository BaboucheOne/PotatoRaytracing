using System.Drawing;
using System.Numerics;

namespace PotatoRaytracing
{
    public class PotatoMesh : PotatoEntity
    {
        private Triangle[] triangles;
        public string TexturePath;
        public string ObjectPath;
        public Color Color = Color.White;

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

        public void SetPosition()
        {
            BakeMesh();
        }

        public void BakeMesh()
        {
            for(int i = 0; i < triangles.Length; i++)
            {
                triangles[i].SetPosition(Position);
            }
        }

        public Triangle GetTriangle(int index) => triangles[index];
        public Triangle[] GetTriangles() => triangles;
        public int GetTrianglesCount => triangles.Length;
    }
}

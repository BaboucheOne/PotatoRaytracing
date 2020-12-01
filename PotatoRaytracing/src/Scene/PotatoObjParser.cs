using System;
using System.IO;
using System.Collections.Generic;
using System.DoubleNumerics;
using System.Text;

namespace PotatoRaytracing
{
    public class PotatoObjParser
    {
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Triangle> triangles = new List<Triangle>();

        private const int objIndexOffset = 1; //Index of vertice start at 1 but to acces 1 in our list, we need to do -1.

        public void Parse(ref PotatoMesh mesh)
        {
            if (!File.Exists(mesh.ObjectPath)) throw new FileNotFoundException();

            FileStream fileStream = new FileStream(mesh.ObjectPath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    ParseLine(line);
                }
            }

            mesh.SetTriangles(triangles.ToArray());

            vertices.Clear();
            normals.Clear();
            triangles.Clear();
        }

        public void ParseLine(string line)
        {
            if (line.Length < 2) return;

            string prefix = string.Concat(line[0], line[1]);
            switch (prefix)
            {
                case "v ":
                    vertices.Add(SplitLineAsVector3(line));
                    break;

                case "f ":
                    if (!line.Contains("//"))
                    {
                        ParseFaceAsVerticeTextureNormalMethod(line);
                    }
                    else
                    {
                        ParseFaceAsVerticeNormalMethod(line);
                    }
                    break;

                case "vn":
                    normals.Add(SplitLineAsVector3(line));
                    break;

                case "vt":
                    break;
            }
        }

        //f v1/vt1/vn1 v2/vt2/vn2 v3/vt3/vn3 --VerticeTextureNormalMethod
        private void ParseFaceAsVerticeTextureNormalMethod(string line)
        {
            string[] split = line.Split(' ');
            Vector3[] faceVectices = new Vector3[3];
            Vector3[] faceNormals = new Vector3[3];
            char[] separator = new char[] { '/' };
            for (int i = 1; i < 4; i++)
            {
                string[] elements = split[i].Split(separator, 3, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length > 0)
                {
                    faceVectices[i - 1] = vertices[int.Parse(elements[0]) - objIndexOffset]; //Car commence a 1 les faces et non a 0.
                    faceNormals[i - 1] = normals[int.Parse(elements[2]) - objIndexOffset]; //Car commence a 1 les faces et non a 0.
                }
            }

            triangles.Add(new Triangle(faceVectices, faceNormals));
        }

        //f v1//vn1 v2//vn2 v3//vn3 --VerticeNormalMethod
        private void ParseFaceAsVerticeNormalMethod(string line)
        {
            string[] split = line.Split(' ');
            Vector3[] faceVectices = new Vector3[3];
            Vector3[] faceNormals = new Vector3[3];
            string[] separator = new string[] { "//" };
            for (int i = 1; i < 4; i++)
            {
                string[] elements = split[i].Split(separator, 3, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length > 0)
                {
                    faceVectices[i - 1] = vertices[int.Parse(elements[0]) - objIndexOffset];
                    faceNormals[i - 1] = normals[int.Parse(elements[1]) - objIndexOffset];
                }
            }

            triangles.Add(new Triangle(faceVectices, faceNormals));
        }

        private static Vector3 SplitLineAsVector3(string line)
        {
            string[] split = line.Split(' ');

            for (int i = 1; i < split.Length; i++)
            {
                split[i] = split[i].Replace('.', ',');
            }

            return new Vector3(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
        }
    }
}

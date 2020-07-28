using System;
using System.Collections.Generic;
using System.DoubleNumerics;

namespace PotatoRaytracing
{
    public class KDTree
    {
        public KDNode Root;

        public KDTree(List<Triangle> triangles)
        {
            Root = Split(triangles);
        }

        public static KDNode Split(List<Triangle> triangles)
        {
            KDNode node = new KDNode(triangles);
            if (triangles.Count == 0) return node;

            Vector3 Min = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
            Vector3 Max = new Vector3();
            foreach (Triangle t in triangles)
            {
                Min.X = Math.Min(Min.X, t.Min.X);
                Min.Y = Math.Min(Min.Y, t.Min.Y);
                Min.Z = Math.Min(Min.Z, t.Min.Z);

                Max.X = Math.Max(Max.X, t.Max.X);
                Max.Y = Math.Max(Max.Y, t.Max.Y);
                Max.Z = Math.Max(Max.Z, t.Max.Z);
            }

            double width = Max.X - Min.X;
            double height = Max.Y - Min.Y;
            double ddepth = Max.Z - Min.Z;
            node.Bbox = new BoundingBox((Max + Min) * 0.5, width, height, ddepth);

            List<Triangle> leftTriangles = new List<Triangle>();
            List<Triangle> rightTriangles = new List<Triangle>();

            int axis = node.Bbox.GetLongestAxis();
            foreach (Triangle triangle in triangles)
            {
                switch(axis)
                {
                    case 0:
                        if (triangle.MidPoint.X <= node.Bbox.Position.X) 
                        { 
                            leftTriangles.Add(triangle);
                        } else
                        {
                            rightTriangles.Add(triangle);
                        }
                        break;

                    case 1:
                        if (triangle.MidPoint.Y <= node.Bbox.Position.Y)
                        {
                            leftTriangles.Add(triangle);
                        }
                        else
                        {
                            rightTriangles.Add(triangle);
                        }
                        break;

                    case 2:
                        if (triangle.MidPoint.Z <= node.Bbox.Position.Z)
                        {
                            leftTriangles.Add(triangle);
                        }
                        else
                        {
                            rightTriangles.Add(triangle);
                        }
                        break;
                }
            }

            if (leftTriangles.Count > 0 && rightTriangles.Count != 0)
            {
                node.Left = Split(leftTriangles);
                node.Left.Parent = node;
            }

            if(rightTriangles.Count > 0 && leftTriangles.Count != 0)
            {
                node.Right = Split(rightTriangles);
                node.Right.Parent = node;
            }

            return node;
        }

        public void ClearTree(KDNode root)
        {
            if(root != null)
            {
                ClearTree(root.Left);
                ClearTree(root.Right);

                Root = null;
            }
        }

        public static void GetNodes(KDNode root, List<KDNode> nodes)
        {
            if (root != null)
            {
                nodes.Add(root);
                GetNodes(root.Left, nodes);
                GetNodes(root.Right, nodes);
            }
        }
    }
}

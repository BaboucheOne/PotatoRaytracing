using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class KDTree
    {
        public KDNode Root;

        public KDTree()
        {
            Root = new KDNode();
        }

        public KDTree(List<Triangle> triangles)
        {
            Root = Split(triangles);
        }

        public static KDNode Split(List<Triangle> triangles)
        {
            KDNode node = new KDNode(triangles);

            if (triangles.Count == 0) return node;

            List<Triangle> leftTriangles = new List<Triangle>();
            List<Triangle> rightTriangles = new List<Triangle>();

            int axis = node.Bbox.GetBiggerAxis(); //TODO: Le coter le plus grand.
            foreach (Triangle triangle in triangles)
            {
                switch(axis)
                {
                    case 0:
                        if (triangle.Center.X <= node.Bbox.Center.X) 
                        { 
                            leftTriangles.Add(triangle);
                        } else
                        {
                            rightTriangles.Add(triangle);
                        }
                        break;

                    case 1:
                        if (triangle.Center.Y <= node.Bbox.Center.Y)
                        {
                            leftTriangles.Add(triangle);
                        }
                        else
                        {
                            rightTriangles.Add(triangle);
                        }
                        break;

                    case 2:
                        if (triangle.Center.Z <= node.Bbox.Center.Z)
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

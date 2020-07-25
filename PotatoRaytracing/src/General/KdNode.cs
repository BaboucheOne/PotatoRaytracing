using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class KDNode
    {
        public KDNode Parent = null;
        public KDNode Left = null;
        public KDNode Right = null;
        public BoundingBox Bbox = new BoundingBox();
        public List<Triangle> Triangles = new List<Triangle>();

        public KDNode()
        {
        }

        public KDNode(Triangle triangle)
        {
            Triangles.Add(triangle);
            UpdateBbox();
        }

        public KDNode(List<Triangle> triangles)
        {
            Triangles = triangles;
            UpdateBbox();
        }

        public KDNode GetRoot()
        {
            KDNode node = this;

            while(node.Parent != null)
            {
                node = node.Parent;
            }

            return node;
        }

        public void UpdateTriangleBbox(Triangle triangle)
        {
            MergeBbox(new BoundingBox(triangle));
        }

        public void MergeBbox(BoundingBox box)
        {
            Bbox.Min.X = Bbox.Min.X > box.Min.X ? box.Min.X : Bbox.Min.X;
            Bbox.Min.Y = Bbox.Min.Y > box.Min.Y ? box.Min.Y : Bbox.Min.Y;
            Bbox.Min.Z = Bbox.Min.Z > box.Min.Z ? box.Min.Z : Bbox.Min.Z;

            Bbox.Max.X = Bbox.Max.X < box.Max.X ? box.Max.X : Bbox.Max.X;
            Bbox.Max.Y = Bbox.Max.Y < box.Max.Y ? box.Max.Y : Bbox.Max.Y;
            Bbox.Max.Z = Bbox.Max.Z < box.Max.Z ? box.Max.Z : Bbox.Max.Z;

            Bbox.UpdateCentroid();
        }

        public BoundingBox UpdateBbox()
        {
            int numTris = Triangles.Count;

            if (numTris > 0) Bbox.SetBounds(Triangles[0]);

            for (int i = 1; i < numTris; i++)
            {
                UpdateTriangleBbox(Triangles[i]);
            }

            if (Left != null)
            {
                MergeBbox(Left.UpdateBbox());
            }

            if (Right != null)
            {
                MergeBbox(Right.UpdateBbox());
            }

            const double pad = 0.001;
            Bbox.Min.X -= pad;
            Bbox.Min.Y -= pad;
            Bbox.Min.Z -= pad;

            Bbox.Max.X += pad;
            Bbox.Max.Y += pad;
            Bbox.Max.Z += pad;

            return Bbox;
        }
    }
}

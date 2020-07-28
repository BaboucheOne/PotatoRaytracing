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

        public KDNode(List<Triangle> triangles)
        {
            Triangles = triangles;
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
    }
}

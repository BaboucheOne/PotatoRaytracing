using System.Collections.Generic;
using System.DoubleNumerics;
using System.Linq;

namespace PotatoRaytracing
{
    public class BoundingBoxTree
    {
        public BoundingBoxNode Root;

        public BoundingBoxTree()
        {
        }

        public BoundingBoxTree(BoundingBoxNode child)
        {
            Root = child;
        }


        public void Build(ref List<BoundingBoxNode> nodes)
        {
            while (nodes.Count > 1)
            {
                double minimalDistance = double.PositiveInfinity;
                BoundingBoxNode nodeA = null;
                BoundingBoxNode nodeB = null;

                for (int i = 0; i < nodes.Count; i++)
                {
                    for (int j = nodes.Count - 1; j >= 0; j--)
                    {
                        if (nodes[i] == nodes[j]) continue;

                        if (BoxIntersection.Stack(nodes[i].Box, nodes[j].Box))
                        {
                            nodes[i].Childs.Add(nodes[j]);
                            nodes.Remove(nodes[j]);
                        }
                    }
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        if (nodes[i] == nodes[j]) continue;

                        double dst = Vector3.Distance(nodes[i].Box.Position, nodes[j].Box.Position);

                        if (dst < minimalDistance)
                        {
                            minimalDistance = dst;
                            nodeA = nodes[i];
                            nodeB = nodes[j];
                        }
                    }
                }

                PotatoBox box = BoxIntersection.MergePotatoBox(nodeA.Box, nodeB.Box);
                BoundingBoxNode parentbbnode = new BoundingBoxNode(box, null, false, null);

                nodeA.Parent = parentbbnode;
                nodeB.Parent = parentbbnode;

                parentbbnode.Childs.Add(nodeA);
                parentbbnode.Childs.Add(nodeB);

                nodes.Add(parentbbnode);

                nodes.Remove(nodeA);
                nodes.Remove(nodeB);
            }

            Root = nodes[0];
        }
    }
}

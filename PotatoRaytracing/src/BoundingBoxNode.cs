using System.Collections.Generic;

namespace PotatoRaytracing
{
    public class BoundingBoxNode
    {
        public BoundingBoxNode Parent;

        public readonly bool IsMesh;
        public readonly PotatoBox Box;
        public readonly PotatoEntity Entity;

        public List<BoundingBoxNode> Childs = new List<BoundingBoxNode>();

        public BoundingBoxNode(BoundingBoxNode parent)
        {
            Parent = parent;
        }

        public BoundingBoxNode(PotatoBox box, PotatoEntity entity, bool isMesh)
        {
            Box = box;
            Entity = entity;
            IsMesh = isMesh;
        }

        public BoundingBoxNode(PotatoBox box, PotatoEntity entity, bool isMesh, BoundingBoxNode parent) : this(parent)
        {
            Box = box;
            Entity = entity;
            IsMesh = isMesh;
        }

        public bool HasChildren => Childs.Count > 0;
    }
}

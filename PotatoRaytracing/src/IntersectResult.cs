namespace PotatoRaytracing
{
    public struct IntersectResult
    {
        public readonly bool Intersect;
        public readonly float Discriminent;

        public IntersectResult(bool intersect, float discriminent)
        {
            Intersect = intersect;
            Discriminent = discriminent;
        }
    }
}

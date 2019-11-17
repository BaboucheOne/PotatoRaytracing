namespace PotatoRaytracing
{
    public struct IntersectResult
    {
        public readonly bool Intersect;
        public readonly double Discriminent;

        public IntersectResult(bool intersect, double discriminent)
        {
            Intersect = intersect;
            Discriminent = discriminent;
        }
    }
}

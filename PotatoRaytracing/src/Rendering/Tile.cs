namespace PotatoRaytracing
{
    public struct Tile
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Size;

        public Tile(int x, int y, int size)
        {
            X = x;
            Y = y;
            Size = size;
        }
    }
}

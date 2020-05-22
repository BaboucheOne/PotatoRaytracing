using System;
using System.DoubleNumerics;
using System.Drawing;
using System.Drawing.Imaging;

namespace PotatoRaytracing
{
    public class Cubemap
    {
        public Bitmap Top;
        public Bitmap Left;
        public Bitmap Front;
        public Bitmap Right;
        public Bitmap Back;
        public Bitmap Down;

        private int bmpSize = 0;
        private int bmpSizeMinusOne;

        public Cubemap()
        {
        }

        public Cubemap(string path) => LoadCubemap(path);

        public Cubemap(Bitmap cubemapSource) => BuildCubemap(cubemapSource);

        public Cubemap(Bitmap top, Bitmap left, Bitmap front, Bitmap right, Bitmap back, Bitmap down)
        {
            Top = top;
            Left = left;
            Front = front;
            Right = right;
            Back = back;
            Down = down;

            bmpSize = Top.Width;
            bmpSizeMinusOne = bmpSize - 1;
        }

        public Cubemap DeepCopy()
        {
            Rectangle cloneRect = new Rectangle(0, 0, bmpSize, bmpSize);
            PixelFormat format = Top.PixelFormat;

            Bitmap TopCopy = Top.Clone(cloneRect, format);
            Bitmap LeftCopy = Left.Clone(cloneRect, format);
            Bitmap RightCopy = Right.Clone(cloneRect, format);
            Bitmap BackCopy = Back.Clone(cloneRect, format);
            Bitmap FrontCopy = Front.Clone(cloneRect, format);
            Bitmap DownCopy = Down.Clone(cloneRect, format);

            return new Cubemap(TopCopy, LeftCopy, FrontCopy, RightCopy, BackCopy, DownCopy);
        }

        public void LoadCubemap(string path)
        {
            Bitmap bmp = new Bitmap(path);
            BuildCubemap(bmp);
        }

        public void BuildCubemap(Bitmap cubemap)
        {
            bmpSize = cubemap.Height / 3;
            bmpSizeMinusOne = bmpSize - 1;

            Rectangle cloneRectTop = new Rectangle(bmpSize, 0, bmpSize, bmpSize);
            Rectangle cloneRectLeft = new Rectangle(0, bmpSize, bmpSize, bmpSize);
            Rectangle cloneRectFront = new Rectangle(bmpSize, bmpSize, bmpSize, bmpSize);
            Rectangle cloneRectRight = new Rectangle(bmpSize * 2, bmpSize, bmpSize, bmpSize);
            Rectangle cloneRectBack = new Rectangle(bmpSize * 3, bmpSize, bmpSize, bmpSize);
            Rectangle cloneRectDown = new Rectangle(bmpSize, bmpSize * 2, bmpSize, bmpSize);


            Top = cubemap.Clone(cloneRectTop, cubemap.PixelFormat);
            Left = cubemap.Clone(cloneRectLeft, cubemap.PixelFormat);
            Front = cubemap.Clone(cloneRectFront, cubemap.PixelFormat);
            Right = cubemap.Clone(cloneRectRight, cubemap.PixelFormat);
            Back = cubemap.Clone(cloneRectBack, cubemap.PixelFormat);
            Down = cubemap.Clone(cloneRectDown, cubemap.PixelFormat);
        }

        public Color GetCubemapColor(Vector3 direction)
        {
            double absX = Math.Abs(direction.X);
            double absY = Math.Abs(direction.Y);
            double absZ = Math.Abs(direction.Z);

            if ((absX >= absY) && (absX >= absZ))
            {
                if (direction.X > 0.0f)
                {
                    int u = (int)((direction.Z / direction.X + 1.0) * 0.5 * bmpSizeMinusOne);
                    int v = (int)((direction.Y / direction.X + 1.0) * 0.5 * bmpSizeMinusOne);
                    return Right.GetPixel(u, v);
                }
                else if (direction.X < 0.0f)
                {
                    int u = (int)((direction.Z / direction.X + 1.0) * 0.5 * bmpSizeMinusOne);
                    int v = (int)((1.0 - (direction.Y / direction.X + 1.0) * 0.5) * bmpSizeMinusOne);
                    return Left.GetPixel(u, v);
                }
            }
            else if ((absY >= absX) && (absY >= absZ))
            {
                if (direction.Y > 0.0f)
                {
                    int u = (int)((direction.X / direction.Y + 1.0) * 0.5 * bmpSizeMinusOne);
                    int v = (int)((direction.Z / direction.Y + 1.0) * 0.5 * bmpSizeMinusOne);
                    return Top.GetPixel(u, v);
                }
                else if (direction.Y < 0.0f)
                {
                    int u = (int)((1.0 - (direction.X / direction.Y + 1.0) * 0.5) * bmpSizeMinusOne);
                    int v = (int)((direction.Z / direction.Y + 1.0) * 0.5 * bmpSizeMinusOne);
                    return Down.GetPixel(u, v);
                }
            }
            else if ((absZ >= absX) && (absZ >= absY))
            {
                if (direction.Z > 0.0f)
                {
                    int u = (int)((1.0 - (direction.X / direction.Z + 1.0) * 0.5) * bmpSizeMinusOne);
                    int v = (int)((direction.Y / direction.Z + 1.0) * 0.5 * bmpSizeMinusOne);
                    return Front.GetPixel(u, v);
                }
                else if (direction.Z < 0.0f)
                {
                    int u = (int)((direction.X / direction.Z + 1.0) * 0.5 * bmpSizeMinusOne);
                    int v = (int)((1.0 - (direction.Y / direction.Z + 1.0) * 0.5) * bmpSizeMinusOne);
                    return Back.GetPixel(u, v);
                }
            }

            return Color.Black;
        }
    }
}

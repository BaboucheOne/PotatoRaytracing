using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PotatoRaytracing
{
    public class ImageBlender
    {
        private readonly List<Bitmap> imagesRendered = new List<Bitmap>();
        private const int minimunImageCountToBlend = 1;

        public int GetRenderedImagesCount() => imagesRendered.Count;
        public List<Bitmap> GetRenderedImages() => imagesRendered;

        private Option option;

        public ImageBlender(Option option)
        {
            this.option = option;
        }

        public void Clear()
        {
            DisposeRenderedImages();

            imagesRendered.Clear();
        }

        private void DisposeRenderedImages()
        {
            for (int i = 0; i < imagesRendered.Count; i++)
            {
                imagesRendered[i].Dispose();
            }
        }

        public void AddImage(Bitmap image)
        {
            imagesRendered.Add(image);
        }

        public Bitmap GetFinalImageRender()
        {
            Bitmap resultImage = imagesRendered[0];

            if (IsAvaibleToBlend())
            {
                resultImage = BlendAllImagesTogether(resultImage);
            }

            return resultImage;
        }

        private Bitmap BlendAllImagesTogether(Bitmap resultImage)
        {
            for (int i = 1; i < imagesRendered.Count; i++)
            {
                resultImage = BlendImage(resultImage, imagesRendered[i]);
            }

            ApplyGammaCorrection(resultImage);
            return resultImage;
        }

        private bool IsAvaibleToBlend()
        {
            return imagesRendered.Count > minimunImageCountToBlend;
        }

        private unsafe Bitmap BlendImage(Bitmap bmpA, Bitmap bmpB)
        {
            Bitmap bmp = new Bitmap(bmpA);

            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            BitmapData bDataA = bmpA.LockBits(new Rectangle(0, 0, bmpA.Width, bmpA.Height), ImageLockMode.ReadWrite, bmpA.PixelFormat);
            BitmapData bDataB = bmpB.LockBits(new Rectangle(0, 0, bmpB.Width, bmpB.Height), ImageLockMode.ReadWrite, bmpB.PixelFormat);

            byte bitsPerPixel = (byte)Image.GetPixelFormatSize(bmp.PixelFormat);

            //Get pointers.
            byte* scan0 = (byte*)bData.Scan0.ToPointer();
            byte* scan0A = (byte*)bDataA.Scan0.ToPointer();
            byte* scan0B = (byte*)bDataB.Scan0.ToPointer();

            for (int i = 0; i < bData.Width; ++i)
            {
                for (int j = 0; j < bData.Height; ++j)
                {
                    byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;
                    byte* dataA = scan0A + i * bDataA.Stride + j * bitsPerPixel / 8;
                    byte* dataB = scan0B + i * bDataB.Stride + j * bitsPerPixel / 8;

                    for (int k = 0; k < 3; k++)
                    {
                        data[k] = (byte)((dataA[k] + dataB[k]) * 0.5f);
                    }
                }
            }

            bmp.UnlockBits(bData);

            return bmp;
        }

        private unsafe void ApplyGammaCorrection(Bitmap bmp)
        {
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            byte bitsPerPixel = (byte)Image.GetPixelFormatSize(bmp.PixelFormat);

            //Get pointers.
            byte* scan0 = (byte*)bData.Scan0.ToPointer();

            for (int i = 0; i < bData.Width; ++i)
            {
                for (int j = 0; j < bData.Height; ++j)
                {
                    byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;

                    for (int k = 0; k < 3; k++)
                    {
                        data[k] = (byte)(Math.Pow(data[k] / 255f, 1f / option.Gamma) * 255f);
                    }
                }
            }

            bmp.UnlockBits(bData);
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PotatoRaytracing
{
    public class ImageBlender
    {
        private List<Bitmap> imagesRendered = new List<Bitmap>();
        private const int minimunImageCountToBlend = 1;

        public int GetRenderedImagesCount() => imagesRendered.Count;
        public List<Bitmap> GetRenderedImages() => imagesRendered;

        public void Clear()
        {
            imagesRendered.Clear();
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

            return resultImage;
        }

        private bool IsAvaibleToBlend()
        {
            return imagesRendered.Count > minimunImageCountToBlend;
        }

        private unsafe Bitmap BlendImage(Bitmap bmpA, Bitmap bmpB)
        {
            Bitmap bmpC = bmpA;

            for (int y = 0; y < bmpA.Height; y++)
            {
                for (int x = 0; x < bmpA.Width; x++)
                {
                    Color cA = bmpA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    Color cC = Color.FromArgb(cA.A, cA.R + cB.R, cA.G + cB.G, cA.B + cB.B);
                    bmpC.SetPixel(x, y, cC);
                }
            }

            return bmpC;
        }
    }
}

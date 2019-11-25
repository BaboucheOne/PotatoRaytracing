using System.Collections.Generic;
using System.Drawing;

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

            return resultImage;
        }

        private bool IsAvaibleToBlend()
        {
            return imagesRendered.Count > minimunImageCountToBlend;
        }

        private Bitmap BlendImage(Bitmap bmpA, Bitmap bmpB)
        {
            Bitmap bmpC = bmpA;

            for (int y = 0; y < bmpA.Height; y++)
            {
                for (int x = 0; x < bmpA.Width; x++)
                {
                    Color cA = bmpA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    Color cC = Color.FromArgb(cA.A, (int)((cA.R + cB.R) * 0.5f),
                                                    (int)((cA.G + cB.G) * 0.5f),
                                                    (int)((cA.B + cB.B) * 0.5f));
                    bmpC.SetPixel(x, y, cC);
                }
            }

            return bmpC;
        }
    }
}

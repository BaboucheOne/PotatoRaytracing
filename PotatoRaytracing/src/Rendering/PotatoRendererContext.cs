using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Video.FFMPEG;

namespace PotatoRaytracing
{
    public class PotatoRenderContext
    {
        public Option Option { get; private set; }
        public PotatoScene Scene { get; private set; }
        private PotatoTasksSceneRenderer tasksSceneRenderer;

        private readonly Stopwatch watch = new Stopwatch();

        public long GetRenderTime => watch.ElapsedMilliseconds;

        public PotatoRenderContext(Option option)
        {
            Option = option;

            Scene = new PotatoScene(option);
        }

        public void MakeImage(string imageName)
        {
            tasksSceneRenderer = new PotatoTasksSceneRenderer(Scene.PotatoSceneData);
            watch.Start();
            Bitmap image = tasksSceneRenderer.Run();
            watch.Stop();

            SaveAndOpenImage(image, imageName);

            ClearRenderContext();
        }

        public void MakeVideo(string videoName)
        {
            //watch.Start();
            //int imgCount = Option.VideoFPS * Option.VideoDuration;
            //Bitmap[] image_sequence = new Bitmap[imgCount];

            //tasksSceneRenderer = new PotatoTasksSceneRenderer(Scene.PotatoSceneData);

            //for (int i = 0; i < imgCount; i++)
            //{
            //    Bitmap[] imgs = tasksSceneRenderer.Run();

            //    BlendAllRenderedImageContainInTasksResult(imgs);

            //    Bitmap finalImage = imageBlender.GetFinalImageRender();
            //    image_sequence[i] = finalImage.Clone(new Rectangle(0, 0, finalImage.Width, finalImage.Height), finalImage.PixelFormat);

            //    Scene.PotatoSceneData.Camera.Position = Scene.PotatoSceneData.Camera.Position + new System.DoubleNumerics.Vector3(1, 0, 0);
            //    for (int j = 0; j < Scene.PotatoSceneData.Spheres.Count; j++)
            //    {
            //        Scene.PotatoSceneData.Spheres[j].Position = Scene.PotatoSceneData.Spheres[j].Position + new System.DoubleNumerics.Vector3(1, 0, 0);
            //    }

            //    imageBlender.Clear();
            //}

            //ClearRenderContext();
            //CreateVideo(videoName, image_sequence);

            //watch.Stop();
        }

        private void CreateVideo(string videoName, Bitmap[] inputImageSequence)
        {
            using (VideoFileWriter vFWriter = new VideoFileWriter())
            {
                vFWriter.Open(videoName, Option.Width, Option.Height, Option.VideoFPS, VideoCodec.Raw);

                foreach (Bitmap img in inputImageSequence)
                {
                    vFWriter.WriteVideoFrame(img);
                }

                vFWriter.Close();
            }
        }

        private void ClearRenderContext()
        {
            tasksSceneRenderer.Clear();
            System.GC.Collect();
        }

        private void SaveAndOpenImage(Bitmap bitmap, string imageName)
        {
            SaveImage(bitmap, imageName);
            OpenImage(imageName);
        }

        private void SaveImage(Bitmap bitmap, string imgageName)
        {
            bitmap.Save(imgageName, ImageFormat.Bmp);
        }

        private static void OpenImage(string imageName)
        {
            Process photoViewer = new Process();
            photoViewer.StartInfo.FileName = imageName;
            photoViewer.Start();
        }
    }
}

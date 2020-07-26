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
        private readonly ImageBlender imageBlender;
        private PotatoTasksSceneRenderer tasksSceneRenderer;

        private readonly Stopwatch watch = new Stopwatch();

        public long GetRenderTime => watch.ElapsedMilliseconds;

        public PotatoRenderContext(Option option)
        {
            Option = option;

            Scene = new PotatoScene(option);
            imageBlender = new ImageBlender();
        }


        public void MakeImage(string imageName)
        {
            watch.Start();
            tasksSceneRenderer = new PotatoTasksSceneRenderer(Scene.PotatoSceneData);
            Bitmap[] imgs = tasksSceneRenderer.Run();
            BlendAllRenderedImageContainInTasksResult(imgs);

            SaveAndOpenImage(imageName);

            ClearRenderContext();

            watch.Stop();
        }

        public void MakeVideo(string videoName)
        {
            watch.Start();
            int imgCount = Option.VideoFPS * Option.VideoDuration;
            Bitmap[] image_sequence = new Bitmap[imgCount];

            tasksSceneRenderer = new PotatoTasksSceneRenderer(Scene.PotatoSceneData);

            for (int i = 0; i < imgCount; i++)
            {
                Bitmap[] imgs = tasksSceneRenderer.Run();

                BlendAllRenderedImageContainInTasksResult(imgs);

                Bitmap finalImage = imageBlender.GetFinalImageRender();
                image_sequence[i] = finalImage.Clone(new Rectangle(0, 0, finalImage.Width, finalImage.Height), finalImage.PixelFormat);

                Scene.PotatoSceneData.Camera.Position = Scene.PotatoSceneData.Camera.Position + new System.DoubleNumerics.Vector3(1, 0, 0);
                for (int j = 0; j < Scene.PotatoSceneData.Spheres.Count; j++)
                {
                    Scene.PotatoSceneData.Spheres[j].Position = Scene.PotatoSceneData.Spheres[j].Position + new System.DoubleNumerics.Vector3(1, 0, 0);
                }

                imageBlender.Clear();
            }

            ClearRenderContext();
            CreateVideo(videoName, image_sequence);

            watch.Stop();
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

        private void SaveAndOpenImage(string imageName)
        {
            SaveImage(imageName);
            OpenImage(imageName);
        }

        private void BlendAllRenderedImageContainInTasksResult(Bitmap[] images)
        {
            for (int i = 0; i < images.Length; i++)
            {
                imageBlender.AddImage(images[i]);
            }
        }

        private void SaveImage(string imgageName)
        {
            Bitmap finalImage = imageBlender.GetFinalImageRender();
            finalImage.Save(imgageName, ImageFormat.Bmp);
        }

        private static void OpenImage(string imageName)
        {
            Process photoViewer = new Process();
            photoViewer.StartInfo.FileName = imageName;
            photoViewer.Start();
        }
    }
}

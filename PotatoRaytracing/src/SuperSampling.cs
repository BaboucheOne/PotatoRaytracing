using System.Drawing;

namespace PotatoRaytracing
{
    public class SuperSampling
    {
        private int redChannel = 0;
        private int greenChannel = 0;
        private int blueChannel = 0;
        private Color samplingColor;

        private int resolution = 0;
        private int halfResolution = 0;
        private float samplingSubPixelDivision = 0;
        private int samplingAverage = 0;

        private PotatoTracer tracer;
        private PotatoScene scene;

        public SuperSampling(int resolution, float samplingSubPixelDivision, PotatoScene scene, PotatoTracer tracer)
        {
            this.resolution = resolution;
            this.samplingSubPixelDivision = samplingSubPixelDivision;
            this.tracer = tracer;
            this.scene = scene;

            halfResolution = resolution / 2;
            samplingAverage = (int)(samplingSubPixelDivision * samplingSubPixelDivision);
        }

        public Color GetSampleColor(Ray ray, int lightIndex, int pixelPositionX, int pixelPositionY)
        {
            ResetColorChannel();

            LoopSubPixel(ray, lightIndex, pixelPositionX, pixelPositionY);

            ReportColorChannelsToUsableRGBValues();

            return Color.FromArgb(255, redChannel, greenChannel, blueChannel);
        }

        private void LoopSubPixel(Ray ray, int lightIndex, int pixelPositionX, int pixelPositionY)
        {
            for (int i = 0; i < samplingSubPixelDivision; i++)
            {
                float divisionPixelX = pixelPositionX + i / samplingSubPixelDivision;
                for (int j = 0; j < samplingSubPixelDivision; j++)
                {
                    float divisionPixelY = pixelPositionY + j / samplingSubPixelDivision;

                    TraceSubPixel(ray, lightIndex, pixelPositionX, pixelPositionY, divisionPixelX, divisionPixelY);
                }
            }
        }

        private void TraceSubPixel(Ray ray, int lightIndex, int pixelPositionX, int pixelPositionY, float divisionPixelX, float divisionPixelY)
        {
            PotatoRenderer.SetRayDirectionByPixelPosition(ref ray, scene, pixelPositionX + (divisionPixelX - halfResolution) / halfResolution, pixelPositionY + (divisionPixelY - halfResolution) / halfResolution);
            samplingColor = tracer.Trace(ray, lightIndex);
            AddSamplingColorToColorChannels();
        }

        private void AddSamplingColorToColorChannels()
        {
            redChannel += samplingColor.R;
            greenChannel += samplingColor.G;
            blueChannel += samplingColor.B;
        }

        private void ReportColorChannelsToUsableRGBValues()
        {
            redChannel /= samplingAverage;
            greenChannel /= samplingAverage;
            blueChannel /= samplingAverage;
        }

        private void ResetColorChannel()
        {
            redChannel = 0;
            greenChannel = 0;
            blueChannel = 0;
        }
    }
}

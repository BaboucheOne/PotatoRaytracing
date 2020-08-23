using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public class SuperSampling
    {
        private int redChannel = 0;
        private int greenChannel = 0;
        private int blueChannel = 0;
        private Color samplingColor;

        private readonly int halfResolution = 0;
        private readonly float samplingSubPixelDivision = 0;
        private readonly int samplingAverage = 0;

        private readonly PotatoTracer tracer;
        private PotatoSceneData sceneData;

        public SuperSampling(int resolution, float samplingSubPixelDivision, PotatoSceneData sceneData, PotatoTracer tracer)
        {
            this.samplingSubPixelDivision = samplingSubPixelDivision;
            this.tracer = tracer;
            this.sceneData = sceneData;

            halfResolution = resolution / 2;
            samplingAverage = (int)(samplingSubPixelDivision * samplingSubPixelDivision);
        }

        public Color GetSampleColor(int lightIndex, int pixelPositionX, int pixelPositionY)
        {
            ResetColorChannel();

            LoopSubPixel(lightIndex, pixelPositionX, pixelPositionY);

            ReportColorChannelsToUsableRGBValues();

            return Color.FromArgb(255, redChannel, greenChannel, blueChannel);
        }

        private void LoopSubPixel(int lightIndex, int pixelPositionX, int pixelPositionY)
        {
            for (int i = 0; i < samplingSubPixelDivision; i++)
            {
                float divisionPixelX = pixelPositionX + i / samplingSubPixelDivision;
                for (int j = 0; j < samplingSubPixelDivision; j++)
                {
                    float divisionPixelY = pixelPositionY + j / samplingSubPixelDivision;

                    TraceSubPixel(lightIndex, pixelPositionX, pixelPositionY, divisionPixelX, divisionPixelY);
                }
            }
        }

        private void TraceSubPixel(int lightIndex, int pixelPositionX, int pixelPositionY, float divisionPixelX, float divisionPixelY)
        {
            Vector2 screenCoord = new Vector2((2.0 * (pixelPositionX + (divisionPixelX - halfResolution)) / sceneData.Option.Width) - 1.0, (-2.0 * (pixelPositionY + (divisionPixelY - halfResolution)) / sceneData.Option.Height) + 1.0);
            samplingColor = tracer.Trace(sceneData.Camera.CreateRay(screenCoord.X, screenCoord.Y), lightIndex, 0);
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

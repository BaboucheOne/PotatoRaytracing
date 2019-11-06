using System;
using System.Diagnostics;

namespace PotatoRaytracing
{
    public class Program
    {
        public static Process CurrentProcess = Process.GetCurrentProcess();
        public static PotatoRenderContext RenderContext;

        static void Main(string[] args)
        {
            CurrentProcess = Process.GetCurrentProcess();

            Option option = OptionFactory.CreateOption();
            RenderContext = new PotatoRenderContext(option);

            Console.WriteLine("--- Potato Raytracing ---");
            Console.WriteLine("Configurations:");
            Console.WriteLine(RenderContext.GetScene().GetOptions().ToString());

            Console.Write("Press to continue any key to beggin.");
            Console.ReadLine();
            
            double timeStart = CurrentProcess.UserProcessorTime.TotalMilliseconds;

            RenderContext.Start("potatoImage.bmp");

            Console.WriteLine("Render time: {0} s", (CurrentProcess.UserProcessorTime.TotalMilliseconds - timeStart) % 60000 / 1000);

            Console.Write("Press to continue any key to exit.");
            Console.ReadLine();
        }
    }
}

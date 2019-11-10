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

            RenderContext.Scene.LoadRandomScene();

            Console.WriteLine("--- Potato Raytracing ---");
            Console.WriteLine("Scene parameters:");
            Console.WriteLine(RenderContext.Scene.ToString());
            Console.WriteLine(string.Empty);
            Console.WriteLine("Configurations:");
            Console.WriteLine(RenderContext.Scene.GetOptions().ToString());

            Console.Write("Press to continue any key to beggin.");
            Console.ReadLine();

            RenderContext.Start("potatoImage.bmp");

            Console.WriteLine("Render time: {0} seconds", RenderContext.GetRenderTime / Constants.millis);

            Console.Write("Press to continue any key to exit.");
            Console.ReadLine();
        }
    }
}

using System;
using System.Diagnostics;

namespace PotatoRaytracing
{
    public class Program
    {
        public static Process CurrentProcess = Process.GetCurrentProcess();
        public static PotatoRender Renderer = new PotatoRender();

        static void Main(string[] args)
        {
            CurrentProcess = Process.GetCurrentProcess();

            PotatoLoadStruct st = new PotatoLoadStruct();

            Console.WriteLine("--- Potato Raytracing ---");
            Console.WriteLine("Configurations:");
            Renderer.Init();
            Console.WriteLine(Renderer.GetOptions().ToString());
            
            string val;
            Console.Write("Press to continue any key to beggin.");
            val = Console.ReadLine();

            Renderer.RenderScene();

            Console.Write("Press to continue any key to exit.");
            val = Console.ReadLine();
        }
    }
}

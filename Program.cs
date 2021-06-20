using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace HelloDotNetCoreTK
{

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello TK!");
            var winSetting = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Hello TK 4.6.5",
            };
            using (var window = new Window(GameWindowSettings.Default, winSetting))
            {
                window.Run();
            }
        }
    }
}

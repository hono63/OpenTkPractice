using System;
using System.Drawing;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace HelloDotNetCoreTK
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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

        public class Window : GameWindow
        {
            public Window(GameWindowSettings gameWinSet, NativeWindowSettings nativeWinSet) 
                : base(gameWinSet, nativeWinSet)
            { }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                if (KeyboardState.IsKeyDown(Keys.Escape))
                {
                    Close();
                }

                base.OnUpdateFrame(e);
            }
        }
    }
}

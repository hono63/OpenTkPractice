using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using HelloDotNetCoreTK;

namespace HelloDotNetCoreTK
{
    public class Window : GameWindow
    {
        // 三角形の頂点 NDC (Normalized Device Coordinates)
        private readonly float[] _vertex =
        {
            -0.5f, -0.5f, 0.0f, // 左下
            +0.5f, -0.5f, 0.0f, // 右下
             0.0f, +0.5f, 0.0f, // 上
        };
        private int _vertexBufObj;
        private int _vertexArrayObj;
        private Shader _shader;

        public Window(GameWindowSettings gameWinSet, NativeWindowSettings nativeWinSet)
            : base(gameWinSet, nativeWinSet)
        { }

        protected override void OnLoad()
        {
            // OpenGL初期化
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _vertexBufObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufObj);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertex.Length * sizeof(float), _vertex, BufferUsageHint.StaticDraw);
            _vertexArrayObj = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObj);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _shader = new Shader(@"..\..\..\Shaders\shader.vert", @"..\..\..\Shaders\shader.frag");
            _shader.Use();
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // レンダーループを作る
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObj);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            // C#では実際必要ないけど、明示的にリソースを開放する
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            GL.DeleteBuffer(_vertexBufObj);
            GL.DeleteVertexArray(_vertexArrayObj);
            GL.DeleteProgram(_shader.Handle);
            base.OnUnload();
        }
    }
}

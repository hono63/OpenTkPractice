using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using HelloDotNetCoreTK;

namespace HelloDotNetCoreTK
{
    public class Window : GameWindow
    {
        // 三角形の頂点 NDC (Normalized Device Coordinates)
        private readonly float[] _vertexTriangle =
        {
            -0.5f, -0.5f, 0.0f, // 左下
            +0.5f, -0.5f, 0.0f, // 右下
             0.0f, +0.5f, 0.0f, // 上
        };
        // 四角形
        private readonly float[] _vertexRectangle =
        {
            +0.5f, +0.5f, 0.0f, // 右上
            +0.5f, -0.5f, 0.0f, // 右下
            -0.5f, -0.5f, 0.0f, // 左下
            -0.5f, +0.5f, 0.0f, // 左上
        };
        // 色付き三角形頂点
        private readonly float[] _vertexGradation =
        {   // position           // color
            -0.5f, -0.5f, 0.0f,   1f, 0f, 0f, // 左下
            +0.5f, -0.5f, 0.0f,   0f, 1f, 0f, // 右下
             0.0f, +0.5f, 0.0f,   0f, 0f, 1f, // 上
        };
        // テクスチャ付き四角形
        private readonly float[] _vertexTextured =
        {   // pos                // texture coordinate
            +0.5f, +0.5f, 0.0f,   1f, 0f, // 右上
            +0.5f, -0.5f, 0.0f,   1f, 1f, // 右下
            -0.5f, -0.5f, 0.0f,   0f, 1f, // 左下
            -0.5f, +0.5f, 0.0f,   0f, 0f, // 左上
        };
        // EBO (Element Buffer Object)が_vertexRectangleのどの頂点を使うかのインデックス？
        private readonly uint[] _index =
        { // 0から始まることに注意
            0, 1, 3,
            1, 2, 3,
        };
        private Stopwatch _timer;
        private int _vertexBufObj;
        private int _vertexArrayObj;
        private int _elementBufObj;
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;

        public Window(GameWindowSettings gameWinSet, NativeWindowSettings nativeWinSet)
            : base(gameWinSet, nativeWinSet)
        { }

        /// <summary>
        /// 読み込み時の初期化
        /// </summary>
        protected override void OnLoad()
        {
            // OpenGL初期化
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            PrepareTexturedRec();
            // shader
            _shader = new Shader(@"..\..\..\Shaders\shader.vert", @"..\..\..\Shaders\shader.frag");
            _shader.Use();
            InitMultiTexture();
            StartStopwatch();
            base.OnLoad();
        }

        // 配列オブジェクトを生成
        private void GenArray(float[] vertices)
        {
            _vertexBufObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufObj);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            _vertexArrayObj = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObj);
        }

        // EBO (element buffer object) を生成
        private void GenEBO(uint[] indices)
        {
            _elementBufObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufObj);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        // 三角形を準備
        private void PrepareTri()
        {
            GenArray(_vertexTriangle);
            // 頂点pointer
            int idx = 0, size = 3, stride = 3 * sizeof(float), offset = 0;
            VertexAttribPointerType typ = VertexAttribPointerType.Float;
            bool is_normalized = false;
            GL.VertexAttribPointer(idx, size, typ, is_normalized, stride, offset);
            GL.EnableVertexAttribArray(idx);
        }
        // 色付き三角形
        private void PrepareGradTri()
        {
            GenArray(_vertexGradation);
            // 頂点pointer
            int idx = 0, size = 3, stride = 6 * sizeof(float), offset = 0;
            VertexAttribPointerType typ = VertexAttribPointerType.Float;
            bool is_normalized = false;
            GL.VertexAttribPointer(idx, size, typ, is_normalized, stride, offset);
            GL.EnableVertexAttribArray(idx);
            // Color用のpointer
            idx++;
            offset = 3 * sizeof(float);
            GL.VertexAttribPointer(idx, size, typ, is_normalized, stride, offset);
            GL.EnableVertexAttribArray(idx);
        }

        // テクスチャ付き四角形
        private void PrepareTexturedRec()
        {
            GenArray(_vertexTextured);
            GenEBO(_index);
        }

        private void InitTexture()
        {
            int size = 3, stride = 5 * sizeof(float), offset = 0;
            VertexAttribPointerType typ = VertexAttribPointerType.Float;
            bool is_normalized = false;
            // 四角形頂点
            var vertLoc = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertLoc);
            GL.VertexAttribPointer(vertLoc, size, typ, is_normalized, stride, offset);
            // テクスチャ頂点
            size = 2;
            offset = 3 * sizeof(float);
            var texLoc = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texLoc);
            GL.VertexAttribPointer(texLoc, size, typ, is_normalized, stride, offset);
            _texture = Texture.LoadFromFile(@"..\..\..\Resources\container.png");
            _texture.Use(TextureUnit.Texture0);
        }

        private void InitMultiTexture()
        {
            InitTexture();
            _texture2 = Texture.LoadFromFile(@"..\..\..\Resources\awesomeface.png");
            _texture2.Use(TextureUnit.Texture1);
            _shader.SetInt("texture0", 0);
            _shader.SetInt("texture1", 1);
        }

        private void PrepareEBO()
        {
            GenArray(_vertexRectangle);
            GenEBO(_index);
            // 頂点pointer
            int idx = 0, size = 3, stride = 3 * sizeof(float), offset = 0;
            VertexAttribPointerType typ = VertexAttribPointerType.Float;
            bool is_normalized = false;
            GL.VertexAttribPointer(idx, size, typ, is_normalized, stride, offset);
            GL.EnableVertexAttribArray(idx);
        }

        private void StartStopwatch()
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        private void SupportedMaxVertex()
        {
            // サポートする最大頂点数を知る
            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxCount);
            Debug.WriteLine($"Maximum vertex support: {maxCount}");
        }

        /// <summary>
        /// レンダーループ
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            //ChangeColorByTimer();
            GL.BindVertexArray(_vertexArrayObj);
            //int first = 0, count = 3;
            //GL.DrawArrays(PrimitiveType.Triangles, first, count);
            GL.DrawElements(PrimitiveType.Triangles, _index.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        // 時間経過で色を変える
        void ChangeColorByTimer()
        {
            double timeValue = _timer.Elapsed.TotalSeconds;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            // fragシェーダーの uni4Color に値を代入
            int location = GL.GetUniformLocation(_shader.Handle, "uni4Color");
            float[] val = { 0f, greenValue, 0f, 1f };
            GL.Uniform4(location, val[0], val[1], val[2], val[3]); // 4つの値を代入
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
            GL.DeleteBuffer(_elementBufObj);
            GL.DeleteVertexArray(_vertexArrayObj);
            GL.DeleteProgram(_shader.Handle);
            GL.DeleteTexture(_texture.Handle);
            GL.DeleteTexture(_texture2.Handle);
            base.OnUnload();
        }
    }
}

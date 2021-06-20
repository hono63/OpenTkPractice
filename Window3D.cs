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
    public class Window3D : GameWindow
    {
        // キューブ
        private readonly float[] _vertexCube =
        {
            -0.5f, -0.5f, -0.5f, // Front face
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f, // Back face
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f, // Left face
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

             0.5f,  0.5f,  0.5f, // Right face
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f, // Bottom face
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f, // Top face
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
        };
        // Normal付きCube
        private readonly float[] _vertex =
{
             // Position          Normal
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };
        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
        private int _vertexBufObj;
        private int _vaoModel;
        private int _vaoLamp;
        private Shader _lampShader;
        private Shader _lightingShader;
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private Texture _texture;
        private Texture _texture2;

        const int SIZE = 3;
        const int STRIDE6 = 6 * sizeof(float);
        const int OFFSET0 = 0;
        const int OFFSET3 = 3 * sizeof(float);
        const bool NOT_NORM = false;
        const VertexAttribPointerType TYPE = VertexAttribPointerType.Float;

        public Window3D(GameWindowSettings gameWinSet, NativeWindowSettings nativeWinSet)
            : base(gameWinSet, nativeWinSet)
        { }

        /// <summary>
        /// 読み込み時の初期化
        /// </summary>
        protected override void OnLoad()
        {
            // OpenGL初期化
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            InitBuf(_vertex);
            // shader
            _lightingShader = new Shader(@"..\..\..\Shaders\shader.vert", @"..\..\..\Shaders\lighting.frag");
            _lampShader = new Shader(@"..\..\..\Shaders\shader.vert", @"..\..\..\Shaders\shader.frag");
            _vaoModel = InitNormalVAO(_lightingShader);
            _vaoLamp  = InitVAO(_lampShader);
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            CursorGrabbed = true;
            base.OnLoad();
        }

        // バッファーを生成
        private void InitBuf(float[] vertices)
        {
            _vertexBufObj = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufObj);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        private int InitVAO(Shader shader)
        {
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            var vertLoc = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertLoc);
            GL.VertexAttribPointer(vertLoc, SIZE, TYPE, NOT_NORM, STRIDE6, OFFSET0);
            return vao;
        }

        private int InitNormalVAO(Shader shader)
        {
            int vao = InitVAO(shader);
            var normalLoc = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLoc);
            GL.VertexAttribPointer(normalLoc, SIZE, TYPE, NOT_NORM, STRIDE6, OFFSET3);
            return vao;
        }

        /// <summary>
        /// レンダーループ
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            // Draw Cube
            _lightingShader.SetVector3("objectColor", new Vector3(1f, 0.5f, 0.31f));
            _lightingShader.SetVector3("lightColor", new Vector3(1f, 1f, 1f));
            _lightingShader.SetVector3("lightPos", _lightPos);
            _lightingShader.SetVector3("viewPos", _camera.Position);
            DrawShader(_lightingShader, _vaoModel, Matrix4.Identity);

            // Draw Lamp
            Matrix4 scale = Matrix4.CreateScale(0.2f);
            Matrix4 lampMatrix = scale * Matrix4.CreateTranslation(_lightPos);
            DrawShader(_lampShader, _vaoLamp, lampMatrix);

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        void DrawShader(Shader shader, int vao, Matrix4 modelPos)
        {
            GL.BindVertexArray(vao);
            shader.Use();
            shader.SetMatrix4("model", modelPos);
            shader.SetMatrix4("view", _camera.GetViewMatrix());
            shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float camspd = 1.5f;
            const float sensit = 0.2f;

            if (input.IsKeyDown(Keys.W))         _camera.Position += _camera.Front * camspd * (float)e.Time;
            if (input.IsKeyDown(Keys.S))         _camera.Position -= _camera.Front * camspd * (float)e.Time;
            if (input.IsKeyDown(Keys.A))         _camera.Position -= _camera.Right * camspd * (float)e.Time;
            if (input.IsKeyDown(Keys.D))         _camera.Position += _camera.Right * camspd * (float)e.Time;
            if (input.IsKeyDown(Keys.Space))     _camera.Position += _camera.Up * camspd * (float)e.Time;
            if (input.IsKeyDown(Keys.LeftShift)) _camera.Position -= _camera.Up * camspd * (float)e.Time;

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var dX = mouse.X - _lastPos.X;
                var dY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += dX * sensit;
                _camera.Pitch -= dY * sensit;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _camera.Fov -= e.OffsetY;
            base.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            // C#では実際必要ないけど、明示的にリソースを開放する
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            GL.DeleteBuffer(_vertexBufObj);
            GL.DeleteProgram(_lightingShader.Handle);
            GL.DeleteProgram(_lampShader.Handle);
            //GL.DeleteTexture(_texture.Handle);
            //GL.DeleteTexture(_texture2.Handle);
            base.OnUnload();
        }
    }
}

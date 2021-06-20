using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // NugetでSystem.Drawing.Commonを入れる。windowsでしか使えないらしいが。。
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace HelloDotNetCoreTK
{
    public class Shader
    {
        public readonly int Handle;
        private readonly Dictionary<string, int> _uniformLocations;

        public Shader(string vertPath, string fragPath)
        {
            // vertex shaderを読み込んでコンパイル
            var shaderSrc = File.ReadAllText(vertPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSrc);
            CompileShader(vertexShader);
            // fragment shaderについてもコンパイル
            shaderSrc = File.ReadAllText(fragPath);
            var fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShader, shaderSrc);
            CompileShader(fragShader);
            // 以上２つのshaderはshaderプログラムにマージしないといけない
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragShader);
            LinkProgram(Handle);
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragShader);
            GL.DeleteShader(fragShader);
            GL.DeleteShader(vertexShader);
            // shader uniform locationをキャッシュする
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var uniformNum);
            _uniformLocations = new Dictionary<string, int>();
            for (var i = 0; i < uniformNum; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occured while compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error occured while linking Program({program}).\n\n{infoLog}");
            }
        }

        /// <summary>
        /// Shaderプログラムを有効にする
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attrName)
        {
            return GL.GetAttribLocation(Handle, attrName);
        }

        // uniformのsetter

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetFloag(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        /// <summary>
        /// 4x4の行列をセット
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }
    }

    /// <summary>
    /// テクスチャ クラス
    /// </summary>
    public class Texture
    {
        public readonly int Handle;
        private const TextureTarget TARGET2D = TextureTarget.Texture2D;
        public static Texture LoadFromFile(string path)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TARGET2D, handle);
            // .Netの System.Drawing ライブラリを使う
            using (var img = new Bitmap(path))
            {
                var data = img.LockBits(
                    new Rectangle(0, 0, img.Width, img.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                // テクスチャ生成 
                GL.TexImage2D(
                    TARGET2D,
                    0, // level
                    PixelInternalFormat.Rgba,
                    img.Width,
                    img.Height,
                    0, // border
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }
            // 設定
            GL.TexParameter(TARGET2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TARGET2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TARGET2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat); // X軸
            GL.TexParameter(TARGET2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat); // Y軸
            // Mipmap (小さいテクスチャ?)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return new Texture(handle);
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        /// <summary>
        /// Activate Texture
        /// </summary>
        /// <param name="unit"></param>
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TARGET2D, Handle);
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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
}

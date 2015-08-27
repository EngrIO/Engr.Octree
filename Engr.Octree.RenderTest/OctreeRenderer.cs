using System;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engr.Octree.RenderTest
{
    public class OctreeRenderer<T> : IRenderer
    {
        private readonly Octree<T> _tree;
        private readonly Func<IOctreeNode<T>, Color> _getColorFunc;
        public OctreeRenderer(Octree<T> tree, Func<IOctreeNode<T>, Color> getColorFunc)
        {
            _tree = tree;
            _getColorFunc = getColorFunc;
        }

        private int _program;
        private int _vbo;
        private int _vao;
        private int _num;
        private int _mvpLocation;

        private Matrix4 _model;
        private Matrix4 _view;
        private Matrix4 _projection;


        private Matrix4 MVP { set { GL.UniformMatrix4(_mvpLocation, false, ref value); } }

        private void SetCamera()
        {
            Matrix4 modelView;
            Matrix4 modelViewProjection;
            Matrix4.Mult(ref _model, ref _view, out modelView);
            Matrix4.Mult(ref modelView, ref _projection, out modelViewProjection);
            MVP = modelViewProjection;
        }

        public void Load(int width, int height)
        {
            _model = Matrix4.Identity;
            _view = Matrix4.CreateTranslation(0, 0, -10);
            _projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, width / (float)height, 1.0f, 64.0f);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);


            var nodes = _tree.GetAllNodes().ToList();
            _num = nodes.Count;
            var vertices = nodes.Select(node => new Vertex()
            {
                Position = node.Center.ToVector3(),
                Colour = _getColorFunc(node).ToVector4(),
                Size = (float)node.Size - 0.1f
            });
            var data = vertices.SelectMany(vertex => new[] { vertex.Position.X, vertex.Position.Y, vertex.Position.Z, vertex.Colour.X, vertex.Colour.Y, vertex.Colour.Z, vertex.Colour.W, vertex.Size }).ToArray();

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);

            _program = GL.CreateProgram();

            GL.AttachShader(_program, CreateShader(ShaderType.VertexShader, @"Shaders\Octree.vert"));
            GL.AttachShader(_program, CreateShader(ShaderType.FragmentShader, @"Shaders\Octree.frag"));
            GL.AttachShader(_program, CreateShader(ShaderType.GeometryShader, @"Shaders\Octree.geo"));
            

            GL.LinkProgram(_program);

            GL.UseProgram(_program);

            var posAttrib = GL.GetAttribLocation(_program, "position");
            var colorAttrib = GL.GetAttribLocation(_program, "color");
            var sizeAttrib = GL.GetAttribLocation(_program, "size");
            const int stride = sizeof(float) * 8;
            GL.VertexAttribPointer(posAttrib, 3, VertexAttribPointerType.Float, false, stride, new IntPtr(0));
            GL.EnableVertexAttribArray(posAttrib);
            GL.VertexAttribPointer(colorAttrib, 4, VertexAttribPointerType.Float, false, stride, new IntPtr(sizeof(float) * 3));
            GL.EnableVertexAttribArray(colorAttrib);
            GL.VertexAttribPointer(sizeAttrib, 1, VertexAttribPointerType.Float, false, stride, new IntPtr(sizeof(float) * 7));
            GL.EnableVertexAttribArray(sizeAttrib);
            _mvpLocation = GL.GetUniformLocation(_program, "mvp");
            SetCamera();
        }

        private Matrix4 _rotate = Matrix4.CreateRotationY(0.01f);

        public void Render(int width, int height)
        {
            Matrix4.Mult(ref _model, ref _rotate, out _model);
            SetCamera();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));
            GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
            GL.DrawArrays(PrimitiveType.Points, 0, _num);

        }

        public void Resize(int width, int height)
        {

        }

        private int CreateShader(ShaderType type, string path)
        {
            var shader = GL.CreateShader(type);
            GL.ShaderSource(shader, File.ReadAllText(path));
            GL.CompileShader(shader);
            int compileStatus;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
            if (compileStatus != 1)
            {
                Console.WriteLine(GL.GetShaderInfoLog(shader));
            }
            return shader;
        }
    }
}
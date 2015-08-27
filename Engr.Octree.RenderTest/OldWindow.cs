using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Engr.Maths.Matrices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engr.Octree.RenderTest
{
    public class OldWindow<T> : GameWindow
    {
        private readonly Func<IOctreeNode<T>, Color> _getColorFunc;
        private readonly Octree<T> _tree;

        private int _program;
        private int _mvpUniform;

        private int _vbo;
        private int _vao;
        private int _ubo;


        private int _num;
        //private Matrix4 MVP
        //{
        //    set
        //    {
        //        //GL.UniformMatrix4(_mvpUniform, false, ref value);
        //    }
        //}

        private UBOData UBO
        {
            set
            {
                GL.BindBuffer(BufferTarget.UniformBuffer, _ubo);
                GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)(Marshal.SizeOf(default(UBOData))), ref value, BufferUsageHint.StreamDraw);
            }
        }

        public OldWindow(Octree<T> tree, Func<IOctreeNode<T>, Color> getColorFunc)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "Engr.Octree")
        {
            _tree = tree;
            _getColorFunc = getColorFunc;
        }

        private Matrix4 CreateCamera(int width, int height)
        {
           
            return (Mat4.CreateOrthographic(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, -1000f, 1000f) * Mat4.Scale(25)).ToMatrix4();
        }

        protected override void OnLoad(EventArgs e)
        {

            GL.PointSize(1f);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);


            _ubo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, _ubo);
            GL.BufferData(BufferTarget.UniformBuffer, (IntPtr)(Marshal.SizeOf(default(UBOData))), (IntPtr)(null), BufferUsageHint.StreamDraw);



            //GL.BindBufferRange(BufferRangeTarget.UniformBuffer, GL.GetUniformBlockIndex(_program, "Camera"), _ubo, (IntPtr)0, (IntPtr)(Marshal.SizeOf(default(UBOData))));
            var err2 = GL.GetError();
            if (err2 != ErrorCode.NoError)
            {
                Console.WriteLine("Error at preProgram: " + err2.ToString());
            }
            


            var nodes = _tree.GetAllNodes().ToList();
            _num = nodes.Count;
            var vertices = nodes.Select(node => new Vertex()
            {
                Position = node.Center.ToVector3(),
                Colour = _getColorFunc(node).ToVector4(),
                Size = (float)node.Size
            });
            var data = vertices.SelectMany(vertex => new[] { vertex.Position.X, vertex.Position.Y, vertex.Position.Z, vertex.Colour.X, vertex.Colour.Y, vertex.Colour.Z, vertex.Colour.W, vertex.Size}).ToArray();

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);




            _program = GL.CreateProgram();

            GL.AttachShader(_program, CreateShader(ShaderType.VertexShader, @"Shaders\Octree.vert"));
            GL.AttachShader(_program, CreateShader(ShaderType.FragmentShader, @"Shaders\Octree.frag"));
            GL.AttachShader(_program, CreateShader(ShaderType.GeometryShader, @"Shaders\Octree.geo"));
            
            GL.LinkProgram(_program);

            var uboIndex = GL.GetUniformBlockIndex(_program, "shader_data");

            

            int linkResult;
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out linkResult);

            if (linkResult != 1)
            {
                string info;
                GL.GetProgramInfoLog(_program, out info);
                Console.WriteLine(info);
                //throw new Exception(String.Join("\n--------\n", _logs.ToArray()));
            }

            GL.UseProgram(_program);
            GL.UniformBlockBinding(_program, uboIndex, _ubo);

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


            var errp = GL.GetError();
            if (errp != ErrorCode.NoError)
            {
                Console.WriteLine("Error at program: " + errp.ToString());
            }




            UBO = new UBOData() { MVP = Matrix4.CreateTranslation(0,0,-20) };

            //MVP = CreateCamera(Width, Height);


            //P:\OpenCAD\OpenCAD.OpenGL\Shaders
            

            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("Error at Load: " + err.ToString());
            }
        }



        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));


            GL.DrawArrays(PrimitiveType.Points, 0, _num);


            SwapBuffers();
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("Error at Swapbuffers: " + err.ToString());
            }
            Title = String.Format(" FPS:{0} Mouse<{1},{2}>", 1.0 / e.Time, Mouse.X, Height - Mouse.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            //MVP = CreateCamera(Width, Height);

            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("Error at Resize: " + err.ToString());
            }
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
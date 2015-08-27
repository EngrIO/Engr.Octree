using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Engr.Maths.Matrices;
using Engr.Maths.Vectors;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engr.Octree.RenderTest
{

    public class Window<T> : GameWindow
    {
        


        private IRenderer _renderer;

        public Window(Octree<T> tree, Func<IOctreeNode<T>, Color> getColorFunc)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "Engr.Octree")
        {

            _renderer = new OctreeRenderer<T>(tree, getColorFunc);
        }

        protected override void OnLoad(EventArgs e)
        {
            _renderer.Load(Width, Height);
            CheckErrors("OnLoad");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _renderer.Render(Width, Height);
            SwapBuffers();
            CheckErrors("OnRenderFrame");
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _renderer.Resize(Width, Height);
            CheckErrors("OnResize");
        }

        private void CheckErrors(string location)
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("Error at {0}: {1}", location, err);
            }
        }
    }


    public interface IRenderer
    {
        void Resize(int width, int height);
        void Load(int width, int height);
        void Render(int width, int height);
    }


    public struct UBOData
    {
        public Matrix4 MVP;
    }

    public struct Vertex
    {
        public Vector3 Position;
        public Vector4 Colour;
        public float Size;
    }
    public static class Extensions
    {
        public static Vector4 ToVector4(this Color4 col)
        {

            return new Vector4(col.R, col.G, col.B, col.A);
        }

        public static Vector4 ToVector4(this Color col)
        {
            return new Vector4(col.R / 255f, col.G / 255f, col.B / 255f, col.A / 255f);
        }

        public static Vector3 ToVector3(this Vect3 v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Matrix4 ToMatrix4(this Mat4 m)
        {
            var md = m.Transpose(); //http://www.opentk.com/node/2771
            return new Matrix4(
                (float)md[1, 1], (float)md[1, 2], (float)md[1, 3], (float)md[1, 4],
                (float)md[2, 1], (float)md[2, 2], (float)md[2, 3], (float)md[2, 4],
                (float)md[3, 1], (float)md[3, 2], (float)md[3, 3], (float)md[3, 4],
                (float)md[4, 1], (float)md[4, 2], (float)md[4, 3], (float)md[4, 4]);
        }

        public static Mat4 ToMat4(this Matrix4 m)
        {
            return new Mat4(new double[,]
                {
                    {m.M11, m.M21, m.M31, m.M41},
                    {m.M12, m.M22, m.M32, m.M42},
                    {m.M13, m.M23, m.M33, m.M43},
                    {m.M14, m.M24, m.M34, m.M44}
                });
        }
        public static float[] ToColumnMajorArrayFloat(this Mat4 m)
        {
            return new[]{
                (float) m[1, 1],(float) m[2, 1],(float) m[3, 1],(float) m[4, 1],
                (float) m[1, 2],(float) m[2, 2],(float) m[3, 2],(float) m[4, 2],
                (float) m[1, 3],(float) m[2, 3],(float) m[3, 3],(float) m[4, 3],
                (float) m[1, 4],(float) m[2, 4],(float) m[3, 4],(float) m[4, 4]};
        }
    }
}
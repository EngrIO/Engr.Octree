using System;
using System.Drawing;
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
}
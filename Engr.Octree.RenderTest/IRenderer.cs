namespace Engr.Octree.RenderTest
{
    public interface IRenderer
    {
        void Resize(int width, int height);
        void Load(int width, int height);
        void Render(int width, int height);
    }
}
namespace Engr.Octree.Operations.Selectors
{
    public interface INodeSelector<T>
    {
        IOctreeNode<T> Choose(IOctreeNode<T> a, IOctreeNode<T> b);
    }
}
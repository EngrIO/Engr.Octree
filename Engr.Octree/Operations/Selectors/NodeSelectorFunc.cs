using System;

namespace Engr.Octree.Operations.Selectors
{
    public class NodeSelectorFunc<T> : INodeSelector<T>
    {
        private readonly Func<IOctreeNode<T>, IOctreeNode<T>, IOctreeNode<T>> _func;

        public NodeSelectorFunc(Func<IOctreeNode<T>, IOctreeNode<T>, IOctreeNode<T>> func)
        {
            _func = func;
        }

        public IOctreeNode<T> Choose(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return _func(a, b);
        }
    }
}
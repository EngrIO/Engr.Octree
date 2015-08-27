using System.Linq;
using Engr.Octree.Operations.Selectors;

namespace Engr.Octree.Operations
{
    public class IntersectOperation<T> : BaseNodeOperation<T>
    {
        private readonly INodeSelector<T> _selector;

        public IntersectOperation(INodeSelector<T> selector)
        {
            _selector = selector;
        }

        public override IOctreeNode<T> EmptyEmpty(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return a;
        }

        public override IOctreeNode<T> EmptyLeaf(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return a;
        }

        public override IOctreeNode<T> EmptyPartial(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return a;
        }

        public override IOctreeNode<T> LeafEmpty(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return b;
        }

        public override IOctreeNode<T> LeafLeaf(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return _selector.Choose(a, b);
        }

        public override IOctreeNode<T> LeafPartial(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Split().Children.Zip(b.Children, Run).ToList());
        }

        public override IOctreeNode<T> PartialEmpty(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return b;
        }

        public override IOctreeNode<T> PartialLeaf(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth, b.Split().Children.Zip(a.Children, Run).ToList());
        }

        public override IOctreeNode<T> PartialPartial(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Children.Zip(b.Children, Run).ToList());
        }
    }
}
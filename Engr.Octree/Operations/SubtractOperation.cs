using System.Linq;

namespace Engr.Octree.Operations
{
    public class SubtractOperation<T> : BaseNodeOperation<T>
    {
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
            return a;
        }

        public override IOctreeNode<T> LeafLeaf(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth);
        }

        public override IOctreeNode<T> LeafPartial(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Split().Zip(b.Children, Run).ToList());
        }

        public override IOctreeNode<T> PartialEmpty(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return a;
        }

        public override IOctreeNode<T> PartialLeaf(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth);
        }

        public override IOctreeNode<T> PartialPartial(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Children.Zip(b.Children, Run).ToList());
        }
    }
}
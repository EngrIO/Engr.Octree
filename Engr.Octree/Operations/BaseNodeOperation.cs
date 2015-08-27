using System;

namespace Engr.Octree.Operations
{
    public abstract class BaseNodeOperation<T> : INodeOperation<T>
    {
        public IOctreeNode<T> Run(IOctreeNode<T> a, IOctreeNode<T> b)
        {
            // CASE 1
            if (a.IsEmpty() && b.IsEmpty())
            {
                return EmptyEmpty(a, b);
            }
            // CASE 2
            if (a.IsEmpty() && b.IsLeaf())
            {
                return EmptyLeaf(a, b);
            }
            //CASE 3
            if (a.IsEmpty() && b.IsPartial())
            {
                return EmptyPartial(a, b);
            }
            //CASE 4
            if (a.IsLeaf() && b.IsEmpty())
            {
                return LeafEmpty(a, b);
            }
            //CASE 5
            if (a.IsLeaf() && b.IsLeaf())
            {
                return LeafLeaf(a, b);
            }
            //CASE 6
            if (a.IsLeaf() && b.IsPartial())
            {
                return LeafPartial(a, b);
            }
            //CASE 7
            if (a.IsPartial() && b.IsEmpty())
            {
                return PartialEmpty(a, b);
            }
            //CASE 8
            if (a.IsPartial() && b.IsLeaf())
            {
                return PartialLeaf(a, b);
            }
            //CASE 9
            if (a.IsPartial() && b.IsPartial())
            {
                return PartialPartial(a, b);
            }
            throw new Exception();
        }

        public abstract IOctreeNode<T> EmptyEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> EmptyLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> EmptyPartial(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> LeafEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> LeafLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> LeafPartial(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> PartialEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> PartialLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        public abstract IOctreeNode<T> PartialPartial(IOctreeNode<T> a, IOctreeNode<T> b);
    }
}
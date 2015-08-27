namespace Engr.Octree.Operations
{
    public interface INodeOperation<T>
    {
        IOctreeNode<T> Run(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> EmptyEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> EmptyLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> EmptyPartial(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> LeafEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> LeafLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> LeafPartial(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> PartialEmpty(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> PartialLeaf(IOctreeNode<T> a, IOctreeNode<T> b);
        IOctreeNode<T> PartialPartial(IOctreeNode<T> a, IOctreeNode<T> b);
    }
}
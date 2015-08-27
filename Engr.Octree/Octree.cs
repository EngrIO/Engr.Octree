using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engr.Maths.Vectors;
using Engr.Octree.Operations;
using Engr.Octree.Operations.Selectors;

namespace Engr.Octree
{
    public class Octree<T>
    {
        public IOctreeNode<T> Root { get; private set; }
        public int MaxDepth { get; private set; }
        public Octree(IOctreeNode<T> root, int maxDepth = 5)
        {
            Root = root;
            MaxDepth = maxDepth;
        }


        //public Octree<T> Interset(Func<IOctreeNode<T>, bool> test, int maxLevel)
        //{
        //    return new Octree<T>(IntersectNode(Root, test, maxLevel));
        //}


        public Octree<T> Subtract(Octree<T> other)
        {
            return new Octree<T>(PerformOperation(new SubtractOperation<T>(), Root, other.Root), MaxDepth);
        }

        public Octree<T> Union(Octree<T> other)
        {
            return Union(other, Selectors<T>.First);
        }

        public Octree<T> Union(Octree<T> other, INodeSelector<T> chooser)
        {
            return new Octree<T>(PerformOperation(new AdditionOperation<T>(chooser), Root, other.Root), MaxDepth);
        }

        public Octree<T> Intersect(Octree<T> other)
        {
            return Intersect(other, Selectors<T>.First);
        }

        public Octree<T> Intersect(Octree<T> other, INodeSelector<T> chooser)
        {
            return new Octree<T>(PerformOperation(new IntersectOperation<T>(chooser), Root, other.Root), MaxDepth);
        }

        public Octree<T> Intersect(Func<IOctreeNode<T>, bool> func, T data)
        {

            return new Octree<T>(Intersect(Root,func,MaxDepth, data));
        }

        private IOctreeNode<T> Intersect(IOctreeNode<T> node, Func<IOctreeNode<T>, bool> func, int maxDepth, T data)
        {
            if (func(node))
            {
                if (node.Depth >= maxDepth) return OctreeNode<T>.Filled(node, data) ;
                switch (node.State)
                {
                    case NodeState.Empty:
                        return new OctreeNode<T>(node.Center, node.Size, node.Depth, node.Split().Children.Select(c => Intersect(c, func, maxDepth, data)).ToList());
                    case NodeState.Leaf:
                        return node;
                    case NodeState.Partial:
                        return new OctreeNode<T>(node.Center, node.Size, node.Depth, node.Children.Select(c => Intersect(c, func, maxDepth, data)).ToList());
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return node;
        }




        private IOctreeNode<T> PerformOperation(INodeOperation<T> operation, IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return operation.Run(a, b);
        }

        //private IOctreeNode<T> UnionNode(IOctreeNode<T> a, IOctreeNode<T> b, INodeChooser<T> chooser)
        //{
        //    // CASE 1
        //    if (a.IsEmpty() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    // CASE 2
        //    if (a.IsEmpty() && b.IsLeaf())
        //    {
        //        return b;
        //    }
        //    //CASE 3
        //    if (a.IsEmpty() && b.IsPartial())
        //    {
        //        return b;
        //    }
        //    //CASE 4
        //    if (a.IsLeaf() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    //CASE 5
        //    if (a.IsLeaf() && b.IsLeaf())
        //    {
        //        return chooser.Choose(a, b);
        //    }
        //    //CASE 6
        //    if (a.IsLeaf() && b.IsPartial())
        //    {
        //        return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Split().Zip(b.Children, (childA, childb) => UnionNode(childA, childb, chooser)).ToList());
        //    }
        //    //CASE 7
        //    if (a.IsPartial() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    //CASE 8
        //    if (a.IsPartial() && b.IsLeaf())
        //    {
        //        return new OctreeNode<T>(a.Center, a.Size, a.Depth, a.Children.Zip(b.Split(), (childA, childb) => UnionNode(childA, childb, chooser)).ToList());
        //    }
        //    //CASE 9
        //    if (a.IsPartial() && b.IsPartial())
        //    {
        //        return new OctreeNode<T>(a.Center,a.Size,a.Depth,a.Children.Zip(b.Children,(childA, childb) => UnionNode(childA,childb,chooser)).ToList());
        //    }

        //}
        //private IOctreeNode<T> IntersectNode(IOctreeNode<T> node, Func<IOctreeNode<T>, bool> test, int maxLevel)
        //{
        //    if (test(node))
        //    {
        //        switch (node.State)
        //        {
        //            case NodeState.Empty:
        //                return node.Clone(CreateChildren(node).Where(test));
        //            case NodeState.Leaf:
        //                return node;
        //            case NodeState.Partial:
        //                return node.Clone(node.Children.())
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        return node;
        //    }
        //}

        //public Octree<T> Union(Octree<T> other)
        //{
            
        //}
        
        //public Octree<T> Do(Func<OctreeNode<T>, NodeState> func)
        //{
        //    return new Octree<T>();
        //}

        //public static Octree<T> FromImages(params Bitmap[] images)
        //{
        //    //images.FirstOrDefault().GetPixel()
        //}



        public static Octree<T> Empty(double size, int maxDepth = 5)
        {
            return new Octree<T>(OctreeNode<T>.Empty(Vect3.Zero, size, 0), maxDepth);
        }

        public IEnumerable<IOctreeNode<T>> GetAllNodes()
        {
            return GetAllNodes(Root);
        }

        public static IEnumerable<IOctreeNode<T>> GetAllNodes(IOctreeNode<T> node)
        {
            yield return node;
            if (node.IsPartial())
            {
                foreach (var child in node.Children)
                {
                    foreach (var c in GetAllNodes(child))
                    {
                        yield return c;
                    }
                }
            }
        }
    }
}

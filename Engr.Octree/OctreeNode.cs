using System;
using System.Collections.Generic;
using System.Linq;
using Engr.Geometry.Primatives;
using Engr.Maths.Vectors;

namespace Engr.Octree
{
    public class OctreeNode<T> : AABB, IOctreeNode<T>
    {
        public double Size { get; private set; }
        public int Depth { get; private set; }
        public T Data { get; private set; }

        public IList<IOctreeNode<T>> Children { get; private set; }

        public OctreeNode(Vect3 center, double size, int depth, T data = default(T)) 
            : base(center, size)
        {
            Depth = depth;
            Data = data;
            Children = Enumerable.Empty<IOctreeNode<T>>().ToList();
            State = CalculateNodeState();
            Size = size;
        }


        public OctreeNode(Vect3 center, double size, int depth, IList<IOctreeNode<T>> children)
            : base(center, size)
        {
            Depth = depth;
            Data = default(T);
            Children = children ?? Enumerable.Empty<IOctreeNode<T>>().ToList();
            State = CalculateNodeState();
            Size = size;
        }


        private NodeState CalculateNodeState()
        {
            if (!EqualityComparer<T>.Default.Equals(Data, default(T)))
            {
                return NodeState.Leaf;
            }
            if (!Children.Any())
            {
                return NodeState.Empty;
            }
            return NodeState.Partial;
        }

        public bool IsEmpty()
        {
            return State == NodeState.Empty;
        }

        public bool IsLeaf()
        {
            return State == NodeState.Leaf;
        }

        public bool IsPartial()
        {
            return State == NodeState.Partial;
        }

        public IOctreeNode<T> Split()
        {
            var newSize = Size / 2.0;
            var half = Size / 4.0;
            return new OctreeNode<T>(Center, Size, Depth, new List<IOctreeNode<T>>
            {
                //top-front-right
                new OctreeNode<T>(Center + new Vect3(+half, +half, +half),newSize,Depth + 1, Data),
                //top-back-right
                new OctreeNode<T>(Center + new Vect3(-half, +half, +half),newSize,Depth + 1, Data),
                //top-back-left
                new OctreeNode<T>(Center + new Vect3(-half, -half, +half),newSize,Depth + 1, Data),
                //top-front-left
                new OctreeNode<T>(Center + new Vect3(+half, -half, +half),newSize,Depth + 1, Data),
                //bottom-front-right
                new OctreeNode<T>(Center + new Vect3(+half, +half, -half),newSize,Depth + 1, Data),
                //bottom-back-right
                new OctreeNode<T>(Center + new Vect3(-half, +half, -half),newSize,Depth + 1, Data),
                //bottom-back-left
                new OctreeNode<T>(Center + new Vect3(-half, -half, -half),newSize,Depth + 1, Data),
                //bottom-front-left
                new OctreeNode<T>(Center + new Vect3(+half, -half, -half),newSize,Depth + 1, Data)
            });
        }

        //public IOctreeNode<T> Clone(IList<IOctreeNode<T>> children)
        //{
        //    return new OctreeNode<T>(Center, Size, Depth, Data, children);
        //}
        
        public NodeState State { get; private set; }

        //public IOctreeNode<T> Clone(NodeState state, T data = default(T))
        //{
        //    switch (state)
        //    {
        //        case NodeState.Empty:
        //            return new OctreeNode<T>(Center, Size, Depth, default(T));
        //        case NodeState.Leaf:
        //            return new OctreeNode<T>(Center, Size, Depth, data);
        //        case NodeState.Partial:
        //            return new OctreeNode<T>(Center, Size, Depth, Children);
        //        default:
        //            throw new ArgumentOutOfRangeException("state");
        //    }
        //}

        public static OctreeNode<T> Filled(Vect3 center, double size, int depth, T data)
        {
            return new OctreeNode<T>(center, size, depth, data);
        }

        public static OctreeNode<T> Filled(IOctreeNode<T> node, T data)
        {
            return new OctreeNode<T>(node.Center, node.Size, node.Depth, data);
        }


        public static OctreeNode<T> Empty(Vect3 center, double size, int depth)
        {
            return new OctreeNode<T>(center,size, depth);
        }

        public static OctreeNode<T> Empty(IOctreeNode<T> node)
        {
            return new OctreeNode<T>(node.Center, node.Size, node.Depth);
        }


    }
}
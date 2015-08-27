using System.Collections.Generic;
using Engr.Geometry.Primatives;

namespace Engr.Octree
{
    public interface IOctreeNode<T>:IAABB
    {
        double Size { get; }
        int Depth { get; }
        T Data { get; }

        IList<IOctreeNode<T>> Children { get; }

        bool IsEmpty();
        bool IsLeaf();
        bool IsPartial();
        //IOctreeNode<T> Clone(IList<IOctreeNode<T>> children);

        IList<IOctreeNode<T>> Split();

        NodeState State { get; }

        //IOctreeNode<T> Clone(NodeState state, T data = default(T));


    }
    public enum NodeState : byte { Empty = 0, Leaf = 1, Partial = 2 }
}
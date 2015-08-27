namespace Engr.Octree.Operations.Selectors
{
    public static class Selectors<T>
    {
        public static INodeSelector<T> First { get { return new NodeSelectorFunc<T>((a, b) => a); } }
        public static INodeSelector<T> Second { get { return new NodeSelectorFunc<T>((a, b) => b); } }
    }
}
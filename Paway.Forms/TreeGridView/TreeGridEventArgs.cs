using System;

namespace Paway.Forms
{
    public class ExpandedEventArgs : TreeGridEventArgs
    {
        public ExpandedEventArgs(TreeGridNode node) : base(node) { }
    }
    public class CollapsedEventArgs : TreeGridEventArgs
    {
        public CollapsedEventArgs(TreeGridNode node) : base(node) { }
    }
    public class TreeGridEventArgs
    {
        public TreeGridNode Node { get; }
        public TreeGridEventArgs(TreeGridNode node)
        {
            this.Node = node;
        }
    }
}


using System;
using System.ComponentModel;

namespace Paway.Forms
{
    public class ExpandingEventArgs : TreeGridCancelEventArgs
    {
        public ExpandingEventArgs(TreeGridNode node) : base(node) { }
    }
    public class CollapsingEventArgs : TreeGridCancelEventArgs
    {
        public CollapsingEventArgs(TreeGridNode node) : base(node) { }
    }
    public class TreeGridCancelEventArgs : CancelEventArgs
    {
        public TreeGridNode Node { get; }
        private TreeGridCancelEventArgs() { }
        public TreeGridCancelEventArgs(TreeGridNode node)
        {
            this.Node = node;
        }
    }
}


using System;
using System.ComponentModel;

namespace Paway.Forms
{
    /// <summary>
    /// 节点展开事件消息
    /// </summary>
    public class ExpandingEventArgs : TreeGridCancelEventArgs
    {
        /// <summary>
        /// 节点展开事件消息
        /// </summary>
        public ExpandingEventArgs(TreeGridNode node) : base(node) { }
    }
    /// <summary>
    /// 节点关闭事件消息
    /// </summary>
    public class CollapsingEventArgs : TreeGridCancelEventArgs
    {
        /// <summary>
        /// 节点关闭事件消息
        /// </summary>
        public CollapsingEventArgs(TreeGridNode node) : base(node) { }
    }
    /// <summary>
    /// 节点取消基础事件消息
    /// </summary>
    public class TreeGridCancelEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 节点
        /// </summary>
        public TreeGridNode Node { get; }
        private TreeGridCancelEventArgs() { }
        /// <summary>
        /// 节点取消基础事件消息
        /// </summary>
        public TreeGridCancelEventArgs(TreeGridNode node)
        {
            this.Node = node;
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Paway.Forms
{
    /// <summary>
    /// TreeGridNodeCollection
    /// </summary>
    public class TreeGridNodeCollection : IList<TreeGridNode>, ICollection<TreeGridNode>, IEnumerable<TreeGridNode>, IEnumerable, IList, ICollection
    {
        internal List<TreeGridNode> _list;
        internal TreeGridNode _owner;

        internal TreeGridNodeCollection(TreeGridNode owner)
        {
            this._owner = owner;
            this._list = new List<TreeGridNode>();
        }

        #region public
        /// <summary>
        /// </summary>
        public TreeGridNode Add(params object[] values)
        {
            TreeGridNode item = new TreeGridNode();
            this.AddValue(item, values);
            return item;
        }
        /// <summary>
        /// </summary>
        public void Add(TreeGridNode item)
        {
            AddValue(item);
        }
        private void AddValue(TreeGridNode item, params object[] values)
        {
            item._grid = this._owner._grid;
            item._owner = this;
            this._list.Add(item);
            item.Update(values);
            this._owner.AddChildNode(item);
            if (!this._owner.HasChildren && this._owner.IsSited)
            {
                this._owner._grid.InvalidateRow(this._owner.RowIndex);
            }
        }
        /// <summary>
        /// </summary>
        public TreeGridNode Add(string text)
        {
            TreeGridNode item = new TreeGridNode();
            this.Add(item);
            item.Cells[0].Value = text;
            return item;
        }
        /// <summary>
        /// </summary>
        public void Clear()
        {
            this._owner.ClearNodes();
            this._list.Clear();
        }
        /// <summary>
        /// </summary>
        public bool Contains(TreeGridNode item) => this._list.Contains(item);
        /// <summary>
        /// </summary>
        public IEnumerator<TreeGridNode> GetEnumerator() => this._list.GetEnumerator();
        /// <summary>
        /// </summary>
        public int IndexOf(TreeGridNode item) => this._list.IndexOf(item);
        /// <summary>
        /// </summary>
        public void Insert(int index, TreeGridNode item)
        {
            item._grid = this._owner._grid;
            item._owner = this;
            this._list.Insert(index, item);
            this._owner.InsertChildNode(index, item);
        }
        /// <summary>
        /// </summary>
        public bool Remove(TreeGridNode item)
        {
            this._owner.RemoveChildNode(item);
            item._grid = null;
            return this._list.Remove(item);
        }
        /// <summary>
        /// </summary>
        public void RemoveAt(int index)
        {
            TreeGridNode node = this._list[index];
            this._owner.RemoveChildNode(node);
            node._grid = null;
            this._list.RemoveAt(index);
        }
        /// <summary>
        /// </summary>
        public int Count => this._list.Count;
        /// <summary>
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// </summary>
        public TreeGridNode this[int index]
        {
            get
            {
                return this._list[index];
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region 显示接口
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        int IList.Add(object value)
        {
            TreeGridNode item = value as TreeGridNode;
            this.Add(item);
            return item.Index;
        }
        void IList.Clear()
        {
            this.Clear();
        }
        bool IList.Contains(object value) => this.Contains(value as TreeGridNode);
        int IList.IndexOf(object item) => this.IndexOf(item as TreeGridNode);
        void IList.Insert(int index, object value)
        {
            this.Insert(index, value as TreeGridNode);
        }
        void IList.Remove(object value)
        {
            this.Remove(value as TreeGridNode);
        }
        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        void ICollection<TreeGridNode>.Add(TreeGridNode item)
        {
            throw new NotImplementedException();
        }
        void ICollection<TreeGridNode>.Clear()
        {
            throw new NotImplementedException();
        }
        bool ICollection<TreeGridNode>.Contains(TreeGridNode item)
        {
            throw new NotImplementedException();
        }
        void ICollection<TreeGridNode>.CopyTo(TreeGridNode[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        bool ICollection<TreeGridNode>.Remove(TreeGridNode item)
        {
            throw new NotImplementedException();
        }
        IEnumerator<TreeGridNode> IEnumerable<TreeGridNode>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        int ICollection.Count => this.Count;
        bool ICollection.IsSynchronized
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        object ICollection.SyncRoot
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => this.IsReadOnly;
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}


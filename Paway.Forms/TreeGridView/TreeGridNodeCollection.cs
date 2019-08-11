using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Paway.Forms
{
    public class TreeGridNodeCollection : IList<TreeGridNode>, ICollection<TreeGridNode>, IEnumerable<TreeGridNode>, IEnumerable, IList, ICollection
    {
        internal List<TreeGridNode> _list;
        internal TreeGridNode _owner;

        internal TreeGridNodeCollection(TreeGridNode owner)
        {
            this._owner = owner;
            this._list = new List<TreeGridNode>();
        }

        public TreeGridNode Add(params object[] values)
        {
            TreeGridNode item = new TreeGridNode();
            this.AddValue(item, values);
            return item;
        }

        public void Add(TreeGridNode item)
        {
            AddValue(item);
        }
        public void AddValue(TreeGridNode item, params object[] values)
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

        public TreeGridNode Add(string text)
        {
            TreeGridNode item = new TreeGridNode();
            this.Add(item);
            item.Cells[0].Value = text;
            return item;
        }

        public void Clear()
        {
            this._owner.ClearNodes();
            this._list.Clear();
        }

        public bool Contains(TreeGridNode item) => this._list.Contains(item);

        public void CopyTo(TreeGridNode[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerator<TreeGridNode> GetEnumerator() => this._list.GetEnumerator();

        public int IndexOf(TreeGridNode item) => this._list.IndexOf(item);

        public void Insert(int index, TreeGridNode item)
        {
            item._grid = this._owner._grid;
            item._owner = this;
            this._list.Insert(index, item);
            this._owner.InsertChildNode(index, item);
        }

        public bool Remove(TreeGridNode item)
        {
            this._owner.RemoveChildNode(item);
            item._grid = null;
            return this._list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            TreeGridNode node = this._list[index];
            this._owner.RemoveChildNode(node);
            node._grid = null;
            this._list.RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

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

        public int Count => this._list.Count;

        public bool IsReadOnly => false;

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
    }
}


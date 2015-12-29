using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Paway.Custom
{
    //TypeConverter未解决
    //[DefaultProperty("Text"),TypeConverter(typeof(ChatListItemConverter))]
    public class ChatListItem
    {
        private bool isOpen;

        private ChatListSubItemCollection subItems;
        private string text = "Item";

        public ChatListItem()
        {
            if (text == null) text = string.Empty;
        }

        public ChatListItem(string text)
        {
            this.text = text;
        }

        public ChatListItem(string text, bool bOpen)
        {
            this.text = text;
            isOpen = bOpen;
        }

        public ChatListItem(ChatListSubItem[] subItems)
        {
            this.subItems.AddRange(subItems);
        }

        public ChatListItem(string text, ChatListSubItem[] subItems)
        {
            this.text = text;
            this.subItems.AddRange(subItems);
        }

        public ChatListItem(string text, bool bOpen, ChatListSubItem[] subItems)
        {
            this.text = text;
            isOpen = bOpen;
            this.subItems.AddRange(subItems);
        }

        /// <summary>
        ///     获取或者设置列表项的显示文本
        /// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (OwnerChatListBox != null)
                    OwnerChatListBox.Invalidate(Bounds);
            }
        }

        /// <summary>
        ///     获取或者设置列表项是否展开
        /// </summary>
        [DefaultValue(false)]
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                if (OwnerChatListBox != null)
                    OwnerChatListBox.Invalidate();
            }
        }

        /// <summary>
        ///     当前列表项下面闪烁图标的个数
        /// </summary>
        [Browsable(false)]
        public int TwinkleSubItemNumber { get; internal set; }

        internal bool IsTwinkleHide { get; set; }

        /// <summary>
        ///     获取列表项的显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds { get; internal set; }

        /// <summary>
        ///     获取列表项所在的控件
        /// </summary>
        [Browsable(false)]
        public ChatListBox OwnerChatListBox { get; internal set; }

        /// <summary>
        ///     获取当前列表项所有子项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChatListSubItemCollection SubItems
        {
            get
            {
                if (subItems == null)
                    subItems = new ChatListSubItemCollection(this);
                return subItems;
            }
        }

        //自定义列表子项的集合 注释同 自定义列表项的集合
        public class ChatListSubItemCollection : IList, ICollection, IEnumerable
        {
            private ChatListSubItem[] m_arrSubItems;
            private readonly ChatListItem owner;

            public ChatListSubItemCollection(ChatListItem owner)
            {
                this.owner = owner;
            }

            public int Count { get; private set; }

            /// <summary>
            ///     根据索引获取一个列表子项
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>列表子项</returns>
            public ChatListSubItem this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    return m_arrSubItems[index];
                }
                set
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    m_arrSubItems[index] = value;
                    if (owner.OwnerChatListBox != null)
                        owner.OwnerChatListBox.Invalidate(m_arrSubItems[index].Bounds);
                }
            }

            //接口实现
            int IList.Add(object value)
            {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                Add((ChatListSubItem)value);
                return IndexOf((ChatListSubItem)value);
            }

            void IList.Clear()
            {
                Clear();
            }

            bool IList.Contains(object value)
            {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                return Contains((ChatListSubItem)value);
            }

            int IList.IndexOf(object value)
            {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                return IndexOf((ChatListSubItem)value);
            }

            void IList.Insert(int index, object value)
            {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                Insert(index, (ChatListSubItem)value);
            }

            bool IList.IsFixedSize
            {
                get { return false; }
            }

            bool IList.IsReadOnly
            {
                get { return false; }
            }

            void IList.Remove(object value)
            {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                Remove((ChatListSubItem)value);
            }

            void IList.RemoveAt(int index)
            {
                RemoveAt(index);
            }

            object IList.this[int index]
            {
                get { return this[index]; }
                set
                {
                    if (!(value is ChatListSubItem))
                        throw new ArgumentException("Value cannot convert to ListSubItem");
                    this[index] = (ChatListSubItem)value;
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                CopyTo(array, index);
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return true; }
            }

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (int i = 0, Len = Count; i < Len; i++)
                    yield return m_arrSubItems[i];
            }

            /// <summary>
            ///     对列表进行排序
            /// </summary>
            public void Sort()
            {
                Array.Sort(m_arrSubItems, 0, Count, null);
                if (owner.OwnerChatListBox != null)
                    owner.OwnerChatListBox.Invalidate(owner.Bounds);
            }

            /// <summary>
            ///     获取在线人数
            /// </summary>
            /// <returns>在线人数</returns>
            public int GetOnLineNumber()
            {
                var num = 0;
                for (int i = 0, len = Count; i < len; i++)
                {
                    if (m_arrSubItems[i].Status != ChatListSubItem.UserStatus.OffLine)
                        num++;
                }
                return num;
            }

            //确认存储空间
            private void EnsureSpace(int elements)
            {
                if (m_arrSubItems == null)
                    m_arrSubItems = new ChatListSubItem[Math.Max(elements, 4)];
                else if (elements + Count > m_arrSubItems.Length)
                {
                    var arrTemp = new ChatListSubItem[Math.Max(m_arrSubItems.Length * 2, elements + Count)];
                    m_arrSubItems.CopyTo(arrTemp, 0);
                    m_arrSubItems = arrTemp;
                }
            }

            /// <summary>
            ///     获取索引位置
            /// </summary>
            /// <param name="subItem">要获取索引的子项</param>
            /// <returns>索引</returns>
            public int IndexOf(ChatListSubItem subItem)
            {
                return Array.IndexOf(m_arrSubItems, subItem);
            }

            /// <summary>
            ///     添加一个子项
            /// </summary>
            /// <param name="subItem">要添加的子项</param>
            public void Add(ChatListSubItem subItem)
            {
                if (subItem == null)
                    throw new ArgumentNullException("SubItem cannot be null");
                EnsureSpace(1);
                if (-1 == IndexOf(subItem))
                {
                    subItem.OwnerListItem = owner;
                    m_arrSubItems[Count++] = subItem;
                    if (owner.OwnerChatListBox != null)
                        owner.OwnerChatListBox.Invalidate();
                }
            }

            /// <summary>
            ///     添加一组子项
            /// </summary>
            /// <param name="subItems">要添加子项的数组</param>
            public void AddRange(ChatListSubItem[] subItems)
            {
                if (subItems == null)
                    throw new ArgumentNullException("SubItems cannot be null");
                EnsureSpace(subItems.Length);
                try
                {
                    foreach (var subItem in subItems)
                    {
                        if (subItem == null)
                            throw new ArgumentNullException("SubItem cannot be null");
                        if (-1 == IndexOf(subItem))
                        {
                            subItem.OwnerListItem = owner;
                            m_arrSubItems[Count++] = subItem;
                        }
                    }
                }
                finally
                {
                    if (owner.OwnerChatListBox != null)
                        owner.OwnerChatListBox.Invalidate();
                }
            }

            /// <summary>
            ///     根据在线状态添加一个子项
            /// </summary>
            /// <param name="subItem">要添加的子项</param>
            public void AddAccordingToStatus(ChatListSubItem subItem)
            {
                if (subItem.Status == ChatListSubItem.UserStatus.OffLine)
                {
                    Add(subItem);
                    return;
                }
                for (int i = 0, len = Count; i < len; i++)
                {
                    if (subItem.Status <= m_arrSubItems[i].Status)
                    {
                        Insert(i, subItem);
                        return;
                    }
                }
                Add(subItem);
            }

            /// <summary>
            ///     移除一个子项
            /// </summary>
            /// <param name="subItem">要移除的子项</param>
            public void Remove(ChatListSubItem subItem)
            {
                var index = IndexOf(subItem);
                if (-1 != index)
                    RemoveAt(index);
            }

            /// <summary>
            ///     根据索引移除一个子项
            /// </summary>
            /// <param name="index">要移除子项的索引</param>
            public void RemoveAt(int index)
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                Count--;
                for (int i = index, Len = Count; i < Len; i++)
                    m_arrSubItems[i] = m_arrSubItems[i + 1];
                if (owner.OwnerChatListBox != null)
                    owner.OwnerChatListBox.Invalidate();
            }

            /// <summary>
            ///     清空所有子项
            /// </summary>
            public void Clear()
            {
                Count = 0;
                m_arrSubItems = null;
                if (owner.OwnerChatListBox != null)
                    owner.OwnerChatListBox.Invalidate();
            }

            /// <summary>
            ///     根据索引插入一个子项
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="subItem">要插入的子项</param>
            public void Insert(int index, ChatListSubItem subItem)
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                if (subItem == null)
                    throw new ArgumentNullException("SubItem cannot be null");
                EnsureSpace(1);
                for (var i = Count; i > index; i--)
                    m_arrSubItems[i] = m_arrSubItems[i - 1];
                subItem.OwnerListItem = owner;
                m_arrSubItems[index] = subItem;
                Count++;
                if (owner.OwnerChatListBox != null)
                    owner.OwnerChatListBox.Invalidate();
            }

            /// <summary>
            ///     将集合类的子项拷贝至数组
            /// </summary>
            /// <param name="array">要拷贝的数组</param>
            /// <param name="index">拷贝的索引位置</param>
            public void CopyTo(Array array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException("Array cannot be null");
                m_arrSubItems.CopyTo(array, index);
            }

            /// <summary>
            ///     判断子项是否在集合内
            /// </summary>
            /// <param name="subItem">要判断的子项</param>
            /// <returns>是否在集合内</returns>
            public bool Contains(ChatListSubItem subItem)
            {
                return IndexOf(subItem) != -1;
            }
        }
    }
}
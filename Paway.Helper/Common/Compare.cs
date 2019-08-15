using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 比较方法
    /// 用法：new List(T).Distinct(new Compare<T>((x, y) => x.Value == y.Value))</T>
    /// </summary>
    public class Compare<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equalsComparer;

        /// <summary>
        /// 构造：传递委托方法
        /// </summary>
        public Compare(Func<T, T, bool> equalsComparer)
        {
            _equalsComparer = equalsComparer;
        }

        /// <summary>
        /// </summary>
        public bool Equals(T x, T y)
        {
            if (null != _equalsComparer)
                return _equalsComparer(x, y);
            return false;
        }

        /// <summary>
        /// </summary>
        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
    /// <summary>
    /// DataTable列排序(比例字符串中的数字)
    /// </summary>
    public class RowComparer : IComparer<DataRow>
    {
        /// <summary>
        /// 排序列
        /// </summary>
        public Dictionary<int, SortOrder> SortColumns { get; set; }
        /// <summary>
        /// 比较器
        /// </summary>
        public int Compare(DataRow x, DataRow y)
        {
            int returnValue = 0;
            foreach (int key in SortColumns.Keys)
            {
                int compareResult = x.ItemArray[key].TCompare(y.ItemArray[key]);
                if (compareResult != 0)
                {
                    returnValue = SortColumns[key] == SortOrder.Ascending ? compareResult : -compareResult;
                    break;
                }
            }
            return returnValue;
        }
    }
    /// <summary>
    /// 字符串比较
    /// </summary>
    public class StringComparer : IComparer<string>
    {
        /// <summary>
        /// 重写字符串比较方法
        /// </summary>
        public int Compare(string x, string y)
        {
            return x.TCompare(y);
        }
    }
}
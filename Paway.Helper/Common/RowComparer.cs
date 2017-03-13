using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Paway.Helper
{
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
                int compareResult = ExCompare(x.ItemArray[key], y.ItemArray[key]);
                if (compareResult != 0)
                {
                    returnValue = SortColumns[key] == SortOrder.Ascending ? compareResult : -compareResult;
                    break;
                }
            }
            return returnValue;
        }
        int ExCompare(Object obj1, Object obj2)
        {
            return obj1.TCompare(obj2);
        }
    }
}
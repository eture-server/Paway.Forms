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
                int compareResult;
                if (x.ItemArray[key] == DBNull.Value && y.ItemArray[key] == DBNull.Value)
                    compareResult = 0;
                else if (x.ItemArray[key] == DBNull.Value)
                    compareResult = -1;
                else if (y.ItemArray[key] == DBNull.Value)
                    compareResult = 1;
                else
                {
                    compareResult = ExCompare(x.ItemArray[key], y.ItemArray[key]);
                }
                if (compareResult != 0)
                {
                    returnValue = SortColumns[key] == SortOrder.Ascending ? compareResult : -compareResult;
                    break;
                }
            }
            return returnValue;
        }
        int ExCompare(Object x, Object y)
        {
            if (x == null || y == null)
                throw new ArgumentException("Parameters can't be null");
            string fileA = x as string;
            string fileB = y as string;
            char[] arr1 = fileA.ToCharArray();
            char[] arr2 = fileB.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (long.Parse(s1) > long.Parse(s2))
                    {
                        return 1;
                    }
                    if (long.Parse(s1) < long.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
        }
    }
}
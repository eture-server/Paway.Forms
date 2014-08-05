using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 比较委托
    /// </summary>
    public delegate bool EqualsComparer<T>(T x, T y);
    /// <summary>
    /// 比较方法
    /// 用法：new List<T>.Distinct(new Compare<T>((x, y) => x.Value == y.Value))
    /// </summary>
    public class Compare<T> : IEqualityComparer<T>
    {
        private EqualsComparer<T> _equalsComparer;

        /// <summary>
        /// </summary>
        public Compare(EqualsComparer<T> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }
        /// <summary>
        /// </summary>
        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
                return this._equalsComparer(x, y);
            else
                return false;
        }
        /// <summary>
        /// </summary>
        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}

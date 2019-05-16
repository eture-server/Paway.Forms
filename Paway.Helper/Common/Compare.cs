using System;
using System.Collections.Generic;

namespace Paway.Helper
{
    /// <summary>
    ///     比较方法
    ///     用法：new List(T).Distinct(new Compare<T>((x, y) => x.Value == y.Value))
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
}
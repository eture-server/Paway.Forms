using System.Collections.Generic;

namespace Paway.Helper
{
    /// <summary>
    ///     比较委托
    /// </summary>
    public delegate bool EqualsComparer<T>(T x, T y);

    /// <summary>
    ///     比较方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Compare<T> : IEqualityComparer<T>
    {
        //用法：new List(T).Distinct(new Compare<T>((x, y) => x.Value == y.Value))

        private readonly EqualsComparer<T> _equalsComparer;

        /// <summary>
        /// </summary>
        public Compare(EqualsComparer<T> equalsComparer)
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
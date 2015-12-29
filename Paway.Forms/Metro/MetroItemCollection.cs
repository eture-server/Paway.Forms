using System.Collections.Generic;

namespace Paway.Forms.Metro
{
    /// <summary>
    /// </summary>
    public class MetroItemCollection : List<MetroItem>
    {
        #region 变量

        /// <summary>
        /// </summary>
        private readonly MetroForm _owner;

        #endregion

        #region 构造函数

        /// <summary>
        /// </summary>
        /// <param name="owner"></param>
        public MetroItemCollection(MetroForm owner)
        {
            _owner = owner;
        }

        #endregion

        #region 属性

        /// <summary>
        /// </summary>
        public MetroForm Owner
        {
            get { return _owner; }
        }

        #endregion

        #region 方法

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        public new void Add(MetroItem item)
        {
            base.Add(item);
        }

        #endregion
    }
}
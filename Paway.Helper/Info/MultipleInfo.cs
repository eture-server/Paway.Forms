using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 多选接口数据
    /// </summary>
    [Serializable]
    public class MultipleInfo : IMultiple
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [NoShow]
        public int Id { get; set; }
        /// <summary>
        /// 多选字段
        /// </summary>
        [NoShow]
        public bool Selected { get; set; }
        /// <summary>
        /// 显示图片
        /// </summary>
        public Image Image { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构造-自动Id
        /// </summary>
        public MultipleInfo()
        {
            this.Id = GetHashCode();
        }
    }
    /// <summary>
    /// 多选接口
    /// </summary>
    public interface IMultiple : IId
    {
        /// <summary>
        /// 多选字段
        /// </summary>
        bool Selected { get; set; }
        /// <summary>
        /// 显示图片
        /// </summary>
        Image Image { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        string Name { get; set; }
    }
}

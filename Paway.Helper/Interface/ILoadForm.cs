using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     启动界面接口
    /// </summary>
    public interface ILoadForm
    {
        /// <summary>
        /// 更新标题
        /// </summary>
        /// <param name="title">标题</param>
        void Title(string title);
        /// <summary>
        /// 更新描述
        /// </summary>
        /// <param name="desc">描述</param>
        void Update(string desc);
        /// <summary>
        /// 设置大图显示
        /// </summary>
        /// <param name="mode">图像定位</param>
        void Update(PictureBoxSizeMode mode);
        /// <summary>
        /// 更新显示图片
        /// </summary>
        /// <param name="load">静态大图</param>
        /// <param name="load2">动态小图</param>
        void Update(Image load, Image load2);
    }
}
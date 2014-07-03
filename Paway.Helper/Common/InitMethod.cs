using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 全局初始化控件
    /// </summary>
    public abstract class InitMethod
    {
        /// <summary>
        /// 加载全局属性
        /// </summary>
        /// <param name="control"></param>
        public static void Init(IControl control)
        {
            if (InitConfig.BackColor != null)
            {
                control.BackColor = InitConfig.BackColor ?? Color.Empty;
            }
            if (InitConfig.Font.Name != "宋体" || InitConfig.Font.Size != 9f || InitConfig.Font.Style != FontStyle.Regular ||
                InitConfig.Font.Unit != GraphicsUnit.Point || InitConfig.Font.GdiCharSet != (byte)1)
            {
                control.Font = InitConfig.Font;
            }
            if (InitConfig.Trans != null && control.Trans == 255)
            {
                control.Trans = InitConfig.Trans ?? 255;
            }
            if (InitConfig.IMouseMove != null)
            {
                control.IMouseMove = InitConfig.IMouseMove ?? false;
            }
        }
    }
}

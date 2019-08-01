using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Helper;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace Paway.Forms
{
    /// <summary>
    /// 控制示例
    /// </summary>
    public partial class TBaseControl : MControl
    {
        private bool iQuerying;

        /// <summary>
        /// </summary>
        public TBaseControl()
        {
            InitializeComponent();
        }

        #region 界面切换事件
        /// <summary>
        /// 子界面返回消息
        /// </summary>
        protected virtual void OnChanged(EventArgs m) { }
        /// <summary>
        /// 切换主界面控件
        /// </summary>
        protected void ResetLoad(Control panel, Type type)
        {
            ResetLoad(panel, type, EventArgs.Empty);
        }
        /// <summary>
        /// 切换主界面控件
        /// </summary>
        protected void ResetLoad(Control panel, Type type, EventArgs e)
        {
            try
            {
                MControl.ReLoad(panel, type, e, TMDirection.None, new Action<object, EventArgs>(Changed));
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }
        private void Changed(object sender, EventArgs e)
        {
            try
            {
                OnChanged(e);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion

        #region 数据异步获取
        /// <summary>
        /// 异步获取查询参数
        /// </summary>
        protected virtual object OnFind(string find) { return null; }
        /// <summary>
        /// 获取结果
        /// </summary>
        protected virtual void OnFinished(object result) { }
        /// <summary>
        /// 开始获取
        /// </summary>
        protected virtual bool QueryStart(string find = null)
        {
            if (!iQuerying)
            {
                iQuerying = true;
                new Func<string, object>(BackQuery).BeginInvoke(find, QueryBack, null);
                return true;
            }
            return false;
        }
        private void QueryBack(IAsyncResult ar)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<IAsyncResult>(QueryBack), ar);
                return;
            }
            iQuerying = false;
            AsyncResult result = (AsyncResult)ar;
            var action = (Func<string, object>)result.AsyncDelegate;
            var value = action.EndInvoke(ar);
            if (this.Visible && value != null && !(value is IList))
            {
                ExceptionHelper.Show(value);
            }
            OnFinished(value);
        }
        private object BackQuery(string find)
        {
            try
            {
                return OnFind(find);
            }
            catch (Exception ex)
            {
                return ex.InnerMessage();
            }
        }

        #endregion

        #region 按钮事件
        /// <summary>
        /// 按钮响应
        /// </summary>
        protected virtual void OnItemClick(ToolItem item) { }
        /// <summary>
        /// 其它响应
        /// </summary>
        protected void ToolBar1_ItemClick(ToolItem item, EventArgs e)
        {
            try
            {
                OnItemClick(item);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion
    }
}

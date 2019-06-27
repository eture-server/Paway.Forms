using log4net;
using Paway.Forms;
using Paway.Helper;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class MainForm : QQForm
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
            this.Text = Config.Text;
            this.TextShow = string.Empty;
            this.toolTitle.Items[0].Text = Config.Text;
            this.TMouseMove(toolTitle);
            this.TMouseMove(toolSet);
            this.TMouseMove(lbDesc);
            this.TMouseMove(lbStatu);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitData method = new InitData();
            method.CompleteEvent += Method_CompleteEvent;
            method.Start(MType.Win);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ResetLoad(panel3, typeof(WaitControl));
            ToShow();
        }

        #region 菜单
        private void ToolBar1_SelectedItemChanged(ToolItem item, EventArgs e)
        {
            try
            {
                switch (item.Text)
                {
                    case "关于":
                        new AboutForm().ShowDialog(this);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion

        #region 初始化
        void Method_CompleteEvent(AsynEventArgs e)
        {
            if (!e.Result)
            {
                ExceptionHelper.Show(e.Message);
                ResetLoad(panel3, typeof(WaitControl));
                MControl.Current.Refresh(this, new WaitEventArgs(false, e.Message));
                return;
            }
            switch (e.MType)
            {
                case MType.WinDelay:
                    Checked();
                    break;
            }
        }
        private void Checked()
        {
            toolSet.ItemClick -= ToolBar1_SelectedItemChanged;
            toolSet.ItemClick += ToolBar1_SelectedItemChanged;
        }

        #endregion

        #region 界面切换
        /// <summary>
        /// 切换主界面控件
        /// </summary>
        private void ResetLoad(Control parent, Type type)
        {
            try
            {
                MControl.ReLoad(parent, type, EventArgs.Empty, TMDirection.None, new Action<object, EventArgs>(Changed));
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }
        private void Changed(object sender, EventArgs e)
        {
            if (!(e is MEventArgs m)) return;
            try
            {
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion

        #region 响应焦点
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                if (!Method.Ask("确认退出系统？"))
                {
                    e.Cancel = true;
                    return;
                }
            }
            base.OnFormClosing(e);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessage.WM_COPYDATA)
            {
                switch ((int)m.WParam)
                {
                    case 0:
                        ToShow();
                        break;
                }
            }
            base.DefWndProc(ref m);
        }
        private void ToShow()
        {
            this.Show();
            this.Activate();
            Win32Helper.ActiveForm(this.Handle);
        }

        #endregion
    }
}

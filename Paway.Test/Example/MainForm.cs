using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Paway.Helper;
using System.Security.Permissions;
using Paway.Win32;
using log4net;

namespace Paway.Test
{
    public partial class MainForm : Demo360
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Login logon;

        public MainForm()
        {
            InitializeComponent();
            this.Text = Config.Text;
            this.TextShow = Config.Text;
#if !DEBUG
            //可恢复的最大化
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            this.WindowState = FormWindowState.Maximized;
#endif
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width, 0);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitData method = new InitData();
            method.CompleteEvent += Method_CompleteEvent;
            method.Start(MType.Win);
            toolPad.Click += ToolPad_Click;
            toolAbout.Click += ToolAbout_Click;
        }

        #region 菜单
        private void ToolBar1_SelectedItemChanged(ToolItem item, EventArgs e)
        {
            switch (item.Text)
            {
                case "主页":
                    ResetLoad(panel3, typeof(WaitControl));
                    break;
                case "设置":
                    ResetLoad(panel3, typeof(SetControl));
                    break;
                case "用户":
                    ResetLoad(panel3, typeof(UsersControl));
                    break;
            }
        }

        #endregion

        #region 其它
        private void ToolAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }
        private void ToolPad_Click(object sender, EventArgs e)
        {
            new UserPadForm().ShowDialog(this);
        }

        #endregion

        #region 登陆
        protected override void OnShown(EventArgs e)
        {
            this.Hide();
            base.OnShown(e);
            ResetLoad(panel3, typeof(WaitControl));
            ToLogon();
        }
        private void ToLogon()
        {
            try
            {
                if (logon == null)
                    logon = new Login();
                Config.User = null;
                if (this.Visible)
                {
                    this.Hide();
                }
                else if (logon.Visible)
                {
                    return;
                }
                logon.ShowDialog(this);
                if (Config.User != null)
                {
                    ToShow();
                }
                else
                {
                    this.Close();
                    return;
                }
                this.CenterToScreen();
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
                case MType.Win:
                    MControl.Current.Refresh(this, new WaitEventArgs());
                    toolBar1.SelectedItemChanged -= ToolBar1_SelectedItemChanged;
                    toolBar1.SelectedItemChanged += ToolBar1_SelectedItemChanged;
                    this.toolBar1.TClickItem("主页");
                    break;
            }
        }

        #endregion

        #region 界面切换
        private void ResetLoad(Control parent, Type type)
        {
            ResetLoad(parent, type, EventArgs.Empty);
        }
        /// <summary>
        /// 切换主界面控件
        /// </summary>
        private void ResetLoad(Control parent, Type type, EventArgs e)
        {
            try
            {
                FileInfo file = new FileInfo(Assembly.GetExecutingAssembly().Location);
                TimeSpan time = DateTime.Now.Subtract(file.LastWriteTime);
                if (time.Days > 3) return;
                Licence.Checking(3);

                MControl.ReLoad(parent, type, e, TMDirection.Transparent, new Action<object, EventArgs>(Changed));
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }
        private void Changed(object sender, EventArgs e)
        {
            MEventArgs m = e as MEventArgs;
            if (m == null) return;
            try
            {
                switch (m.MType)
                {
                    case MType.Exit:
                        this.Close();
                        break;
                    case MType.Wait:
                        ResetLoad(panel3, typeof(WaitControl));
                        MControl.Current.Refresh(sender, e);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion

        #region 响应焦点
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
            Win32Helper.ActiveForm(this.Handle);
        }

        #endregion
    }
}

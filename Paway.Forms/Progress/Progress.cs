using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 进度条
    /// </summary>
    public sealed class Progress : IDisposable
    {
        #region 变量
        private readonly ProgressState _state = null;
        private readonly static ProgressStates states = new ProgressStates();
        private volatile static bool IStop = true;
        /// <summary>
        /// 自动
        /// </summary>
        public static readonly IntPtr Auto = new IntPtr(-1);
        /// <summary>
        /// 手动
        /// </summary>
        public static readonly IntPtr Manual = new IntPtr(-2);

        #endregion

        #region 属性
        /// <summary>
        /// 正在显示的进度条数量
        /// </summary>
        public static int Count { get { return states.Count; } }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool ICancel { get { return this._state.ICancel; } }
        /// <summary>
        /// 超时显示取消按钮(s)
        /// </summary>
        public static int Timeout { get; set; } = 30;

        #endregion

        #region 事件
        /// <summary>
        /// 取消事件
        /// </summary>
        public event Func<bool> CancelEvent;

        #endregion

        #region static method
        /// <summary>
        /// 初始化线程
        /// </summary>
        public static void Initialize()
        {
            if (!IStop) return;
            IStop = false;
            new Action(Run).BeginInvoke(null, null);
        }
        private static void Run()
        {
            ProgressForm progressForm = new ProgressForm(states);
            while (!IStop)
            {
                if (progressForm.Visible || states.Count > 0)
                {
                    progressForm.Text = string.Format("Progress.{0}.{1}", progressForm.Handle, progressForm.CurrentHandle);
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(40);
                }
            }
            progressForm.Close();
        }
        /// <summary>
        /// 清空
        /// </summary>
        public static void Clear()
        {
            states.Clear();
        }
        /// <summary>
        /// 清空数据，关闭线程
        /// </summary>
        public static void Abort()
        {
            states.Clear();
            IStop = true;
        }

        #endregion

        #region public method
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(string caption = TConfig.Loading, int max = 0) : this(Progress.Auto, false, caption, 0, max) { }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="canCancel">是否可以取消，默认否（设置取消未注册事件，则直接关闭窗体）</param>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(bool canCancel, string caption = TConfig.Loading, int max = 0) : this(Progress.Auto, canCancel, caption, 0, max) { }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="delay">延迟显示时间(ms)</param>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(int delay, string caption = TConfig.Loading, int max = 0) : this(Progress.Auto, false, caption, delay, max) { }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="handle">父控件句柄</param>
        /// <param name="canCancel">是否可以取消，默认否（设置取消未注册事件，则直接关闭窗体）</param>
        /// <param name="caption">标题</param>
        /// <param name="delay">延迟显示时间(ms)</param>
        /// <param name="max">最大值</param>
        public Progress(IntPtr handle, bool canCancel = false, string caption = TConfig.Loading, int delay = 0, int max = 0)
        {
            this._state = new ProgressState(handle, caption, canCancel, delay, max);
            this._state.CancelEvent += State_CancelEvent;
            states.Add(this._state);
        }
        private void State_CancelEvent()
        {
            if (CancelEvent == null || CancelEvent.Invoke()) Dispose();
        }
        /// <summary>
        /// 完成释放
        /// </summary>
        public void Dispose()
        {
            states.Remove(this._state);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="caption">标题</param>
        public void Step(string caption = TConfig.Loading)
        {
            this._state.Caption = caption;
        }
        /// <summary>
        /// 设置进度值
        /// </summary>
        /// <param name="value">进度值</param>
        public void Step(int value)
        {
            this._state.NoValue = false;
            this._state.Value = value;
        }
        /// <summary>
        /// 设置父控件句柄
        /// </summary>
        /// <param name="handle">父控件句柄</param>
        public void Step(IntPtr handle)
        {
            this._state.Handle = handle;
        }
        /// <summary>
        /// 进度条加满
        /// </summary>
        public void Max()
        {
            this._state.NoValue = false;
            this._state.Value = this._state.Max;
            Thread.Sleep(20);
            Application.DoEvents();
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this._state.Max = 0;
            this._state.NoValue = true;
        }

        #endregion
    }
}


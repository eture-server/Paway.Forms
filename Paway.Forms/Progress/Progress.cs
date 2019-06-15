﻿using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Paway.Forms
{
    /// <summary>
    /// 进度条
    /// </summary>
    public class Progress : IDisposable
    {
        private readonly ProgressState _state = null;

        private readonly static ProgressStates states = new ProgressStates();
        private static Thread thread;
        private volatile static bool IStop;
        /// <summary>
        /// 取消事件
        /// </summary>
        public event Action CancelEvent;

        /// <summary>
        /// 初始化线程
        /// </summary>
        public static void Initialize()
        {
            IStop = false;
            thread = new Thread(ThreadProc)
            {
                Name = "progress thread",
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };
            thread.Start();
        }
        private static void ThreadProc()
        {
            ProgressForm progressForm = new ProgressForm(states);
            while (!IStop)
            {
                if (progressForm.Visible || states.Count > 0)
                {
                    progressForm.Text = string.Format("Progress.{0}.{1}", progressForm.Handle, progressForm.WindowToWatch);
                    Application.DoEvents();
                }
                Thread.Sleep(40);
            }
            progressForm.Close();
        }
        /// <summary>
        /// 清空数据，关闭线程
        /// </summary>
        public static void Abort()
        {
            states.Clear();
            IStop = true;
            if (thread != null && thread.IsAlive)
                thread.Join(100);
        }
        /// <summary>
        /// 正在显示的进度条数量
        /// </summary>
        public static int Count { get { return states.Count; } }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(string caption = "Loading..", int max = 0) : this(ProgressStates.False, false, caption, max) { }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="canCancel">是否可以取消，默认否</param>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(bool canCancel, string caption = "Loading..", int max = 0) : this(ProgressStates.False, canCancel, caption, max) { }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="owner">父控件</param>
        /// <param name="canCancel">是否可以取消，默认否</param>
        /// <param name="caption">标题</param>
        /// <param name="max">最大值</param>
        public Progress(IntPtr owner, bool canCancel = false, string caption = "Loading..", int max = 0)
        {
            Application.DoEvents();
            this._state = new ProgressState(owner, caption, canCancel, max);
            this._state.CancelEvent += State_CancelEvent;
            states.Add(this._state);
        }
        private void State_CancelEvent()
        {
            CancelEvent?.Invoke();
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
        public void Step(string caption = "Loading..")
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
        /// 是否取消
        /// </summary>
        public bool ICancel { get { return this._state.ICancel; } }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this._state.Max = 0;
            this._state.NoValue = true;
        }
    }
}


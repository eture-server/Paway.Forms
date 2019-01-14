using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.Windows.Forms;
using Paway.Forms;
using log4net;
using System.Reflection;
using System.Threading;

namespace Paway.Test
{
    public partial class InitData
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private BackgroundWorker bw = null;
        private volatile bool IStop;
        public event Action<AsynEventArgs> CompleteEvent;
        private MType type;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Start(MType type)
        {
            if (bw == null)
            {
                bw = new BackgroundWorker()
                {
                    WorkerSupportsCancellation = true
                };
                bw.DoWork += Bw_DoWork;
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            }
            if (!bw.IsBusy)
            {
                this.type = type;
                bw.RunWorkerAsync(type);
            }
        }
        public void Cancel()
        {
            while (bw != null && bw.IsBusy)
            {
                if (IStop) break;
                bw.CancelAsync();
                Thread.Sleep(50);
            }
        }

        void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bw = sender as BackgroundWorker;
                MType type = (MType)e.Argument;

                switch (type)
                {
                    case MType.Win:
                        DataService.Default.Load(bw, type);
                        break;
                }
                e.Result = type;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                e.Result = ex.InnerMessage();
            }
            finally
            {
                IStop = true;
            }
        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AsynEventArgs msg = new AsynEventArgs();
            if (e.Result is MType)
            {
                msg.Result = true;
                msg.MType = (MType)e.Result;
            }
            else
            {
                msg.Result = false;
                msg.MType = this.type;
                msg.Message = e.Result.ToString();
            }
            CompleteEvent?.Invoke(msg);
        }
    }
}

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
using log4net;
using System.Reflection;

namespace Paway.Test
{
    public partial class BaseControl : TBaseControl
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseControl()
        {
            InitializeComponent();
        }

        #region 外部刷新事件
        protected virtual void OnRefresh(MEventArgs m) { }
        public override void Refresh(object sender, EventArgs e)
        {
            MEventArgs m = e as MEventArgs;
            if (m == null) return;
            try
            {
                OnRefresh(m);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        #endregion
    }
}

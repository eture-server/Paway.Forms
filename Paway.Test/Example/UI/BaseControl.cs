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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseControl()
        {
            InitializeComponent();
        }

        #region 外部刷新事件
        protected virtual bool OnRefresh(MEventArgs m) { return true; }
        public override bool Refresh(object sender, EventArgs e)
        {
            MEventArgs m = e as MEventArgs;
            if (m == null) return true;
            try
            {
                return OnRefresh(m);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
            return true;
        }

        #endregion
    }
}

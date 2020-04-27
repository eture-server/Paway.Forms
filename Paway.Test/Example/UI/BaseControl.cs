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

namespace Paway.Test
{
    public partial class BaseControl : TBaseControl
    {
        public BaseControl()
        {
            InitializeComponent();
        }

        #region 外部事件监听
        protected virtual void OnChanged(MEventArgs m) { }
        protected void OnChanged()
        {
            MControl.ChangeEvent += MControl_ChangeEvent;
        }
        private void MControl_ChangeEvent(object sender, EventArgs e)
        {
            if (!(e is MEventArgs m)) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<object, EventArgs>(MControl_ChangeEvent), sender, e);
                    return;
                }
                OnChanged(m);
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }

        #endregion
    }
}

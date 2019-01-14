using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class BaseForm : TBaseForm
    {
        public BaseForm()
        {
            InitializeComponent();
            this.Text = Config.Text;
            this.TextShow = string.Empty;
        }
    }
}

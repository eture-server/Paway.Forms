using Paway.Forms;
using Paway.Helper;
using Paway.Win32;
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
    public partial class Form1 : QQForm
    {
        private int[] cs = new int[] { 200, 214, 226, 236, 244, 254, 255 };
        public Form1()
        {
            //this.Opacity = 0.5;
            this.TMargin = 20;
            this.Raw = cs.Length;
            this.TDemo = true;
            InitializeComponent();
            this._sysButton = TSysButton.Close_Mini;
            this.Paint += Form1_Paint;
            this.TDrawRoundRect(panel1, 2);
            this.TMouseMove(panel1);
            this.Padding = new Padding(TMargin);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int rgn = NativeMethods.CreateRoundRectRgn(this.TMargin - this.Raw, this.TMargin - this.Raw, this.Width + 1 - this.TMargin + this.Raw, this.Height + 1 - this.TMargin + this.Raw, 0, 0);
            NativeMethods.SetWindowRgn(this.Handle, rgn, true);
        }
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(this.TMargin, this.TMargin, this.Width - this.TMargin, this.Height - this.TMargin);

            for (int i = 0; i < cs.Length; i++)
            {
                if (i == cs.Length - 1)
                {
                    DrawRound(g, cs[i], 255, ref rect);
                }
                else
                {
                    DrawRound(g, cs[i], cs[i + 1], ref rect);
                }
            }
        }
        private void DrawRound(Graphics g, int rgb, int last, ref Rectangle rect)
        {
            rect.X -= 1;
            rect.Y -= 1;
            rect.Width += 2;
            rect.Height += 2;
            Color color = Color.FromArgb(255, rgb, rgb, rgb);
            g.DrawPath(new Pen(new SolidBrush(color)),
                DrawHelper.CreateRoundPath(new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), 5));

            color = Color.FromArgb(255, last, last, last);
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + 1, rect.Y + 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X, rect.Y, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X, rect.Y + 2, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + 2, rect.Y, 1, 1));

            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 2, rect.Y + rect.Height - 2, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 1, rect.Y + rect.Height - 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 3, rect.Y + rect.Height - 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 1, rect.Y + rect.Height - 3, 1, 1));

            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + 1, rect.Y + rect.Height - 2, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X, rect.Y + rect.Height - 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + 2, rect.Y + rect.Height - 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X, rect.Y + rect.Height - 3, 1, 1));

            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 2, rect.Y + 1, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 1, rect.Y, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 3, rect.Y, 1, 1));
            g.FillRectangle(new SolidBrush(color), new Rectangle(rect.X + rect.Width - 1, rect.Y + 2, 1, 1));
        }
    }
}

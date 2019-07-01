using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Paway.Forms
{
    /// <summary>
    /// 扩充Chart控件(缩放与移动)
    /// </summary>
    public class TChart : Chart
    {
        private int start;
        private int lastY;
        private int moveInterval = 3;
        private double minInterval = double.MaxValue;
        private int tick;
        private double lastX;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem toolSave;
        private ToolStripMenuItem toolReset;
        private ToolStripSeparator toolStripSeparator1;
        private string _tipText = "序号：{0}\r\n值：{1}";
        /// <summary>
        /// 显示格式
        /// </summary>
        [Browsable(false)]
        [DefaultValue("序号：{0}\r\n值：{1}")]
        public string TipText { get { return _tipText; } set { _tipText = value; } }
        /// <summary>
        /// 当前点击序号
        /// </summary>
        public event Action<int> SelectEvent;

        /// <summary>
        /// 构造
        /// </summary>
        public TChart()
        {
            InitializeComponent();
            this.GetToolTipText += TChart_GetToolTipText;
            this.MouseDown += TChart_MouseDown;
            this.MouseMove += TChart_MouseMove;
            this.MouseEnter += delegate
            {
                if (!this.Focused) this.Focus();
            };
            this.MouseUp += TChart_MouseUp;
            this.MouseWheel += TChart_MouseWheel;
            this.MouseLeave += delegate { Reset(false); };
            this.AxisViewChanged += TChart_AxisViewChanged;
            this.toolSave.Click += ToolSave_Click;
            this.toolReset.Click += delegate { Reset(); };
        }
        #region 公开方法
        private void Reset(bool iReset = true)
        {
            if (this.ChartAreas.Count == 0) return;
            this.ChartAreas[0].AxisX.CustomLabels.Clear();
            this.ChartAreas[0].AxisY.CustomLabels.Clear();
            this.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            this.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            this.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            this.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            if (iReset)
            {
                this.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                this.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                UpdateShow();
            }
        }
        private void ToolSave_Click(object sender, EventArgs e)
        {
            if (this.ChartAreas.Count == 0) return;
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Image Files|*.jpg",
                Title = "Save Chart"
            };
            if (DialogResult.OK == sfd.ShowDialog())
            {
                Reset();
                this.SaveImage(sfd.FileName, ChartImageFormat.Jpeg);
            }
        }
        /// <summary>
        /// 加载点
        /// </summary>
        /// <param name="list"></param>
        public void UpdateChar(Dictionary<double, double> list)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<Dictionary<double, double>>(UpdateChar), list);
                return;
            }
            if (this.Series.Count == 0) return;
            this.Series[0].Points.Clear();
            Reset();
            double min1x = int.MaxValue, max1x = 0, last = -1;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list.ElementAt(i);
                DataPoint point = new DataPoint(item.Key, item.Value);
                this.Series[0].Points.Add(point);

                if (min1x > item.Key) min1x = item.Key;
                if (max1x < item.Key) max1x = item.Key;
                if (i > 0 && Math.Abs(item.Key - last) < minInterval)
                {
                    minInterval = Math.Abs(item.Key - last);
                }
                last = item.Key;
            }

            ChartArea area1 = this.ChartAreas[0];
            if (max1x == min1x) max1x = min1x + 10;
            area1.AxisX.Minimum = min1x - 1;
            area1.AxisX.Maximum = max1x + 1;
            area1.AxisX.LabelStyle.Format = "{0:F0}";
            UpdateInterval();

            // Zoom into the X axis
            //SimpleChart.ChartAreas[0].AxisX.ScaleView.Zoom(1, 1);
            // Enable range selection and zooming end user interface
            //this.ChartAreas[0].CursorX.IsUserEnabled = true;
            this.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            this.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //将滚动内嵌到坐标轴中
            this.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            // 设置滚动条的大小
            this.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            this.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            // 设置自动放大与缩小的最小量
            this.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            this.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            this.ChartAreas[0].AxisX.IsMarginVisible = false;

            UpdateShow();
        }
        private void UpdateShow()
        {
            AxisScaleView view1 = this.ChartAreas[0].AxisX.ScaleView;
            if (this.Series.Count == 0) return;
            if (this.Series[0].Points.Count * 1.0 * (view1.ViewMaximum - view1.ViewMinimum) / (this.ChartAreas[0].AxisX.Maximum - this.ChartAreas[0].AxisX.Minimum) / this.Width < 0.03)
            {
                this.Series[0].Label = "#VAL";
            }
            else this.Series[0].Label = null;
        }

        #endregion

        #region 缩放与移动
        private void TChart_AxisViewChanged(object sender, ViewEventArgs e)
        {
            if (this.ChartAreas.Count > 0)
            {
                this.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
            }
            UpdateShow();
        }
        private void TChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.Series.Count == 0 || this.Series[0].Points.Count < 3) return;

            AxisScaleView view1 = this.ChartAreas[0].AxisX.ScaleView;
            double total = Math.Abs(e.Delta);
            double interval = 1;
            for (int i = 0; i < total; i += 120)
            {
                if (e.Delta > 0)
                    interval *= 0.8;
                else
                    interval *= 1.2;
            }
            if (interval > 1 && (view1.ViewMaximum - view1.ViewMinimum) > (this.ChartAreas[0].AxisX.Maximum - this.ChartAreas[0].AxisX.Minimum) * 0.8)
            {
                view1.ZoomReset();
                this.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                UpdateLine();
                return;
            }
            this.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
            if (this.ChartAreas[0].AxisX.CustomLabels.Count == 0) return;

            string v1 = this.ChartAreas[0].AxisX.CustomLabels[0].Text;
            double value;
            if (this.Series[0].XValueType == ChartValueType.Time)
            {
                var v2 = v1.Split(' ')[0].Split(':');
                value = v2[0].ToInt() * 60 + v2[1].ToInt();
            }
            else
            {
                value = v1.ToDouble();
            }
            double min2 = value - (value - view1.ViewMinimum) * interval;
            min2 = Math.Floor(min2);
            double max2 = value + (view1.ViewMaximum - value) * interval;
            max2 = Math.Ceiling(max2);
            if (interval < 1 && max2 - min2 < minInterval * 3) return;
            view1.Zoom(min2, max2);
            UpdateShow();
            UpdateLine();
        }
        private void UpdateLine()
        {
            var ax = this.ChartAreas[0].AxisX.CustomLabels;
            if (ax.Count > 0)
            {
                var temp = ax[0];
                double value;
                if (this.Series[0].XValueType == ChartValueType.Time)
                {
                    var v1 = temp.Text.Split(' ')[0];
                    var v2 = v1.Split(':');
                    value = v2[0].ToInt() * 60 + v2[1].ToInt();
                }
                else
                {
                    value = temp.Text.ToDouble();
                }
                double length = (this.ChartAreas[0].AxisX.ScaleView.ViewMaximum - this.ChartAreas[0].AxisX.ScaleView.ViewMinimum) / 10;
                temp.FromPosition = value - length;
                temp.ToPosition = value + length;
                temp.Text = temp.Text;
            }
        }
        private void TChart_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void TChart_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.ChartAreas[0].AxisX.ScaleView.IsZoomed)
            {
                HitTestResult result = this.HitTest(e.X, e.Y);
                switch (result.ChartElementType)
                {
                    case ChartElementType.Gridlines:
                    case ChartElementType.DataPoint:
                    case ChartElementType.PlottingArea:
                        start = e.X;
                        this.Cursor = Cursors.Hand;
                        break;
                }
            }
            else
            {
                this.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            }
            OnIndexEvent();
        }
        private void OnIndexEvent()
        {
            if (SelectEvent != null)
            {
                if (this.ChartAreas.Count > 0 && this.ChartAreas[0].AxisX.CustomLabels.Count > 0)
                {
                    if (this.ChartAreas[0].AxisX.CustomLabels[0].Tag is KeyValuePair<Series, int> d)
                    {
                        SelectEvent(d.Value);
                    }
                }
            }
        }
        private void TChart_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseButtons != MouseButtons.Left) return;

            var test = this.HitTest(e.X, e.Y);
            if (test.Object is AxisScrollBar) return;

            AxisScaleView view1 = this.ChartAreas[0].AxisX.ScaleView;
            int interval = start - e.X;
            double value = interval * (view1.ViewMaximum - view1.ViewMinimum) * 1 / this.Width / 0.7;
            value = Math.Round(value);
            if (value == 0) return;
            if (view1.ViewMaximum + value <= this.ChartAreas[0].AxisX.Maximum && view1.ViewMinimum + value >= this.ChartAreas[0].AxisX.Minimum)
            {
                view1.Position += value;
                start -= (this.Width * 0.7 * value / (view1.ViewMaximum - view1.ViewMinimum)).ToInt();
            }
        }
        private void TChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (Environment.TickCount - tick > 30) tick = Environment.TickCount;
            else return;
            if (MouseButtons == MouseButtons.Left) return;
            Dictionary<Series, int> list = HitTest(e.X);
            if (list.Count > 0)
            {
                DrawLine(list);
            }
        }
        private void DrawLine(Dictionary<Series, int> list)
        {
            if (list.Count == 0) return;

            var item = list.ElementAt(0);
            var point = item.Key.Points[item.Value];
            if (lastX == point.XValue) return;
            lastX = point.XValue;
            List<DataPoint> list2 = new List<DataPoint>();
            for (int i = 0; i < list.Count; i++)
            {
                item = list.ElementAt(i);
                list2.Add(item.Key.Points[item.Value]);
            }
            list2.Sort(nameof(DataPoint.Name));


            double length = (this.ChartAreas[0].AxisX.ScaleView.ViewMaximum - this.ChartAreas[0].AxisX.ScaleView.ViewMinimum) / 2;
            this.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Red;
            this.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            CustomLabel x = null;
            if (this.ChartAreas[0].AxisX.CustomLabels.Count == 0)
            {
                x = new CustomLabel()
                {
                    ForeColor = Color.Red,
                    GridTicks = GridTickTypes.Gridline,
                    RowIndex = 1,
                    LabelMark = LabelMarkStyle.None
                };
                this.ChartAreas[0].AxisX.CustomLabels.Add(x);
            }
            else x = this.ChartAreas[0].AxisX.CustomLabels[0];
            x.FromPosition = point.XValue - length;
            x.ToPosition = point.XValue + length;
            if (this.Series[0].XValueType == ChartValueType.Time)
            {
                TimeSpan time = new TimeSpan(24, 0, 0);
                DateTime date = new DateTime((time.Ticks * point.XValue).ToLong());
                x.Text = date.ToString("HH:mm:ss");
                x.Tag = item;
            }
            else
            {
                x.Text = point.XValue.ToString("0.##");
            }
            length = (this.ChartAreas[0].AxisY.Maximum - this.ChartAreas[0].AxisY.Minimum) / 2;
            this.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Red;
            this.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this.ChartAreas[0].AxisY.CustomLabels.Clear();
            list2.Sort((c1, c2) => c1.YValues[0].CompareTo(c2.YValues[0]));
            for (int i = 0; i < list2.Count; i++)
            {
                CustomLabel y = new CustomLabel()
                {
                    ForeColor = Color.Red,
                    GridTicks = GridTickTypes.Gridline,
                    RowIndex = 1,
                    LabelMark = LabelMarkStyle.None
                };
                this.ChartAreas[0].AxisY.CustomLabels.Add(y);

                y.FromPosition = list2[i].YValues[0] - length;
                y.ToPosition = list2[i].YValues[0] + length;
                if (i == 0 || list2[i].YValues[0] - list2[i - 1].YValues[0] > 2)
                    y.Text = list2[i].YValues[0].ToString("0.##");
            }
        }
        private Dictionary<Series, int> HitTest(int x)
        {
            Dictionary<Series, int> list = new Dictionary<Series, int>();
            for (int i = lastY, j = lastY; i < this.Height || j > 0; i += moveInterval, j -= moveInterval)
            {
                if (i < this.Height)
                {
                    HitTestResult result = this.HitTest(x, i);
                    if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
                    {
                        lastY = i;
                        if (!list.ContainsKey(result.Series))
                            list.Add(result.Series, result.PointIndex);
                    }
                }
                if (j > 0)
                {
                    HitTestResult result = this.HitTest(x, j);
                    if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
                    {
                        lastY = j;
                        if (!list.ContainsKey(result.Series))
                            list.Add(result.Series, result.PointIndex);
                    }
                }
            }
            return list;
        }
        private void UpdateInterval()
        {
            if (this.Series.Count > 0 && this.Series[0].Points.Count > 0)
            {
                this.moveInterval = (Math.Round(this.Height * 1.0 / this.Series[0].Points.Count)).ToInt();
                if (this.moveInterval < 1) this.moveInterval = 1;
                if (this.moveInterval > 5) this.moveInterval = 5;
            }
        }
        /// <summary>
        /// 重载Size事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateInterval();
            this.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            this.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            UpdateShow();
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 自动纵坐标
        /// </summary>
        /// <param name="name">新加点曲线名称</param>
        /// <param name="value">新加点值</param>
        /// <param name="_max">最大预测值</param>
        /// <param name="_min">最小预测值</param>
        /// <param name="_interval">最小间隔</param>
        public void AutoAxis(string name = null, double value = 0, double _max = 100, double _min = -5, double _interval = 5)
        {
            double min = _max, max = _min;
            for (int i = 0; i < this.Series.Count; i++)
            {
                var series = this.Series[i];
                if (series.Name == name && value != 0)
                {
                    if (min > value) min = value;
                    if (max < value) max = value;
                }
                for (int j = 0; j < series.Points.Count; j++)
                {
                    var temp = series.Points[j].YValues[0];
                    if (temp == 0) continue;
                    if (min > temp) min = temp;
                    if (max < temp) max = temp;
                }
            }
            if (max < min)
            {
                max = _max;
                min = _min;
            }
            if (_interval > 0)
            {
                var interval = max - min;
                if (interval < _interval)
                {
                    interval = _interval - interval;
                    max += interval / 2;
                    min -= interval / 2;
                }
            }
            this.ChartAreas[0].AxisY.Minimum = TMethod.Round(min) - 0.1;
            this.ChartAreas[0].AxisY.Maximum = TMethod.Round(max) + 0.1;
        }

        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolReset,
            this.toolStripSeparator1,
            this.toolSave});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 54);
            // 
            // toolSave
            // 
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(108, 22);
            this.toolSave.Text = "Save";
            // 
            // toolReset
            // 
            this.toolReset.Name = "toolReset";
            this.toolReset.Size = new System.Drawing.Size(108, 22);
            this.toolReset.Text = "Reset";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(105, 6);
            // 
            // TChart
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Custom
{
    internal class ChatListVScroll
    {
        private Color arrowBackColor;

        private Color arrowColor;

        //滚动条自身的区域
        private Rectangle bounds;
        //绑定的控件
        //下边箭头区域
        private Rectangle downBounds;

        private bool isMouseDown;

        private bool isMouseOnDown;

        private bool isMouseOnSlider;

        private bool isMouseOnUp;
        //滑块移动前的 滑块的y坐标
        private int m_nLastSliderY;
        //鼠标在滑块点下时候的y坐标
        //是否有必要在控件上绘制滚动条
        //滑块区域
        private Rectangle sliderBounds;

        private Color sliderDefaultColor;

        private Color sliderDownColor;
        //上边箭头区域
        private Rectangle upBounds;
        //当前滚动条位置
        private int value;
        //虚拟的一个高度(控件中内容的高度)
        private int virtualHeight;

        public ChatListVScroll(Control c)
        {
            Ctrl = c;
            virtualHeight = 400;
            bounds = new Rectangle(0, 0, 10, 10);
            upBounds = new Rectangle(0, 0, 10, 10);
            downBounds = new Rectangle(0, 0, 10, 10);
            sliderBounds = new Rectangle(0, 0, 10, 10);
            BackColor = Color.LightYellow;
            sliderDefaultColor = Color.Gray;
            sliderDownColor = Color.DarkGray;
            arrowBackColor = Color.Gray;
            arrowColor = Color.White;
        }

        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public Rectangle UpBounds
        {
            get { return upBounds; }
        }

        public Rectangle DownBounds
        {
            get { return downBounds; }
        }

        public Rectangle SliderBounds
        {
            get { return sliderBounds; }
        }

        public Color BackColor { get; set; }

        public Color SliderDefaultColor
        {
            get { return sliderDefaultColor; }
            set
            {
                if (sliderDefaultColor == value)
                    return;
                sliderDefaultColor = value;
                Ctrl.Invalidate(sliderBounds);
            }
        }

        public Color SliderDownColor
        {
            get { return sliderDownColor; }
            set
            {
                if (sliderDownColor == value)
                    return;
                sliderDownColor = value;
                Ctrl.Invalidate(sliderBounds);
            }
        }

        public Color ArrowBackColor
        {
            get { return arrowBackColor; }
            set
            {
                if (arrowBackColor == value)
                    return;
                arrowBackColor = value;
                Ctrl.Invalidate(bounds);
            }
        }

        public Color ArrowColor
        {
            get { return arrowColor; }
            set
            {
                if (arrowColor == value)
                    return;
                arrowColor = value;
                Ctrl.Invalidate(bounds);
            }
        }

        public Control Ctrl { get; set; }

        public int VirtualHeight
        {
            get { return virtualHeight; }
            set
            {
                if (value <= Ctrl.Height)
                {
                    if (ShouldBeDraw == false)
                        return;
                    ShouldBeDraw = false;
                    if (this.value != 0)
                    {
                        this.value = 0;
                        Ctrl.Invalidate();
                    }
                }
                else
                {
                    ShouldBeDraw = true;
                    if (value - this.value < Ctrl.Height)
                    {
                        this.value -= Ctrl.Height - value + this.value;
                        Ctrl.Invalidate();
                    }
                }
                virtualHeight = value;
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (!ShouldBeDraw)
                    return;
                if (value < 0)
                {
                    if (this.value == 0)
                        return;
                    this.value = 0;
                    Ctrl.Invalidate();
                    return;
                }
                if (value > virtualHeight - Ctrl.Height)
                {
                    if (this.value == virtualHeight - Ctrl.Height)
                        return;
                    this.value = virtualHeight - Ctrl.Height;
                    Ctrl.Invalidate();
                    return;
                }
                this.value = value;
                Ctrl.Invalidate();
            }
        }

        public bool ShouldBeDraw { get; private set; }

        public bool IsMouseDown
        {
            get { return isMouseDown; }
            set
            {
                if (value)
                {
                    m_nLastSliderY = sliderBounds.Y;
                }
                isMouseDown = value;
            }
        }

        public bool IsMouseOnSlider
        {
            get { return isMouseOnSlider; }
            set
            {
                if (isMouseOnSlider == value)
                    return;
                isMouseOnSlider = value;
                Ctrl.Invalidate(SliderBounds);
            }
        }

        public bool IsMouseOnUp
        {
            get { return isMouseOnUp; }
            set
            {
                if (isMouseOnUp == value)
                    return;
                isMouseOnUp = value;
                Ctrl.Invalidate(UpBounds);
            }
        }

        public bool IsMouseOnDown
        {
            get { return isMouseOnDown; }
            set
            {
                if (isMouseOnDown == value)
                    return;
                isMouseOnDown = value;
                Ctrl.Invalidate(DownBounds);
            }
        }

        public int MouseDownY { get; set; }

        public void ClearAllMouseOn()
        {
            if (!isMouseOnDown && !isMouseOnSlider && !isMouseOnUp)
                return;
            isMouseOnDown =
                isMouseOnSlider =
                    isMouseOnUp = false;
            Ctrl.Invalidate(bounds);
        }

        //将滑块跳动至一个地方
        public void MoveSliderToLocation(int nCurrentMouseY)
        {
            if (nCurrentMouseY - sliderBounds.Height / 2 < 11)
                sliderBounds.Y = 11;
            else if (nCurrentMouseY + sliderBounds.Height / 2 > Ctrl.Height - 11)
                sliderBounds.Y = Ctrl.Height - sliderBounds.Height - 11;
            else
                sliderBounds.Y = nCurrentMouseY - sliderBounds.Height / 2;
            value =
                (int)
                    ((double)(sliderBounds.Y - 11) / (Ctrl.Height - 22 - SliderBounds.Height) *
                     (virtualHeight - Ctrl.Height));
            Ctrl.Invalidate();
        }

        //根据鼠标位置移动滑块
        public void MoveSliderFromLocation(int nCurrentMouseY)
        {
            //if (!this.IsMouseDown) return;
            if (m_nLastSliderY + nCurrentMouseY - MouseDownY < 11)
            {
                if (sliderBounds.Y == 11)
                    return;
                sliderBounds.Y = 11;
            }
            else if (m_nLastSliderY + nCurrentMouseY - MouseDownY > Ctrl.Height - 11 - SliderBounds.Height)
            {
                if (sliderBounds.Y == Ctrl.Height - 11 - sliderBounds.Height)
                    return;
                sliderBounds.Y = Ctrl.Height - 11 - sliderBounds.Height;
            }
            else
            {
                sliderBounds.Y = m_nLastSliderY + nCurrentMouseY - MouseDownY;
            }
            value =
                (int)
                    ((double)(sliderBounds.Y - 11) / (Ctrl.Height - 22 - SliderBounds.Height) *
                     (virtualHeight - Ctrl.Height));
            Ctrl.Invalidate();
        }

        //绘制滚动条
        public void ReDrawScroll(Graphics g)
        {
            if (!ShouldBeDraw)
                return;
            bounds.X = Ctrl.Width - 10;
            bounds.Height = Ctrl.Height;
            upBounds.X = downBounds.X = bounds.X;
            downBounds.Y = Ctrl.Height - 10;
            //计算滑块位置
            sliderBounds.X = bounds.X;
            sliderBounds.Height = (int)((double)Ctrl.Height / virtualHeight * (Ctrl.Height - 22));
            if (sliderBounds.Height < 3) sliderBounds.Height = 3;
            sliderBounds.Y = 11 +
                             (int)
                                 ((double)value / (virtualHeight - Ctrl.Height) * (Ctrl.Height - 22 - sliderBounds.Height));
            var sb = new SolidBrush(BackColor);
            try
            {
                g.FillRectangle(sb, bounds);
                sb.Color = arrowBackColor;
                g.FillRectangle(sb, upBounds);
                g.FillRectangle(sb, downBounds);
                if (isMouseDown || isMouseOnSlider)
                    sb.Color = sliderDownColor;
                else
                    sb.Color = sliderDefaultColor;
                g.FillRectangle(sb, sliderBounds);
                sb.Color = arrowColor;
                if (isMouseOnUp)
                    g.FillPolygon(sb, new[]
                    {
                        new Point(Ctrl.Width - 5, 3),
                        new Point(Ctrl.Width - 9, 7),
                        new Point(Ctrl.Width - 2, 7)
                    });
                if (isMouseOnDown)
                    g.FillPolygon(sb, new[]
                    {
                        new Point(Ctrl.Width - 5, Ctrl.Height - 4),
                        new Point(Ctrl.Width - 8, Ctrl.Height - 7),
                        new Point(Ctrl.Width - 2, Ctrl.Height - 7)
                    });
            }
            finally
            {
                sb.Dispose();
            }
        }
    }
}
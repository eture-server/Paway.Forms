using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// TreeGridCell
    /// </summary>
    public class TreeGridCell : DataGridViewTextBoxCell
    {
        private int _imageHeight;
        private int _imageHeightOffset;
        private int _imageWidth;
        private Padding _previousPadding;
        private int calculatedLeftPadding;
        private int glyphWidth = 15;
        private const int INDENT_MARGIN = 5;
        private const int INDENT_WIDTH = 20;
        internal bool IsSited;

        #region 重载与重绘
        /// <summary>
        /// </summary>
        public override object Clone()
        {
            TreeGridCell cell1 = (TreeGridCell)base.Clone();
            cell1.glyphWidth = this.glyphWidth;
            cell1.calculatedLeftPadding = this.calculatedLeftPadding;
            return cell1;
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.Location.X > base.InheritedStyle.Padding.Left)
            {
                base.OnMouseDown(e);
            }
            else
            {
                TreeGridNode owningNode = this.OwningNode;
                if (owningNode != null)
                {
                    owningNode._grid._inExpandCollapseMouseCapture = true;
                    if (owningNode.IsExpanded)
                    {
                        owningNode.Collapse();
                    }
                    else
                    {
                        owningNode.Expand();
                    }
                }
            }
        }
        /// <summary>
        /// </summary>
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);
            TreeGridNode owningNode = this.OwningNode;
            if (owningNode != null)
            {
                owningNode._grid._inExpandCollapseMouseCapture = false;
            }
        }
        /// <summary>
        /// 重绘
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            TreeGridNode owningNode = this.OwningNode;
            if (owningNode != null)
            {
                Image image = owningNode.Image;
                if ((this._imageHeight == 0) && (image != null))
                {
                    this.UpdateStyle();
                }
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                Rectangle rectangle = new Rectangle(cellBounds.X + this.GlyphMargin, cellBounds.Y, INDENT_WIDTH, cellBounds.Height - 1);
                if (image != null)
                {
                    Point point;
                    if (this._imageHeight > cellBounds.Height)
                    {
                        point = new Point(rectangle.X + this.glyphWidth, cellBounds.Y + this._imageHeightOffset);
                    }
                    else
                    {
                        point = new Point(rectangle.X + this.glyphWidth, ((cellBounds.Height / 2) - (this._imageHeight / 2)) + cellBounds.Y);
                    }
                    GraphicsContainer container = graphics.BeginContainer();
                    graphics.SetClip(cellBounds);
                    graphics.DrawImageUnscaled(image, point);
                    graphics.EndContainer(container);
                }
                if (owningNode._grid.ShowLines)
                {
                    using (Pen pen = new Pen(SystemBrushes.ControlDark, 1f))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        bool isLastSibling = owningNode.IsLastSibling;
                        bool isFirstSibling = owningNode.IsFirstSibling;
                        if (owningNode.Level == 1)
                        {
                            if (isFirstSibling & isLastSibling)
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else if (isLastSibling)
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else if (isFirstSibling)
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.X + 4, cellBounds.Bottom);
                            }
                            else
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top, rectangle.X + 4, cellBounds.Bottom);
                            }
                        }
                        else
                        {
                            if (isLastSibling)
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else
                            {
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top + (cellBounds.Height / 2), rectangle.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, rectangle.X + 4, cellBounds.Top, rectangle.X + 4, cellBounds.Bottom);
                            }
                            TreeGridNode parent = owningNode.Parent;
                            for (int i = (rectangle.X + 4) - INDENT_WIDTH; !parent.IsRoot; i -= INDENT_WIDTH)
                            {
                                if (parent.HasChildren && !parent.IsLastSibling)
                                {
                                    graphics.DrawLine(pen, i, cellBounds.Top, i, cellBounds.Bottom);
                                }
                                parent = parent.Parent;
                            }
                        }
                    }
                }
                if (owningNode.HasChildren || owningNode._grid.VirtualNodes)
                {
                    if (owningNode.IsExpanded)
                    {
                        owningNode._grid.rOpen.DrawBackground(graphics, new Rectangle(rectangle.X, (rectangle.Y + (rectangle.Height / 2)) - 4, 10, 10));
                    }
                    else
                    {
                        owningNode._grid.rClosed.DrawBackground(graphics, new Rectangle(rectangle.X, (rectangle.Y + (rectangle.Height / 2)) - 4, 10, 10));
                    }
                }
            }
        }

        #endregion

        #region 节点Cell状态
        internal virtual void Sited()
        {
            this.IsSited = true;
            this._previousPadding = base.Style.Padding;
            this.UpdateStyle();
        }
        internal virtual void UnSited()
        {
            this.IsSited = false;
            base.Style.Padding = this._previousPadding;
        }
        internal virtual void UpdateStyle()
        {
            if (this.IsSited)
            {
                Size size;
                int level = this.Level;
                Padding padding = this._previousPadding;
                using (Graphics graphics = this.OwningNode._grid.CreateGraphics())
                {
                    size = this.GetPreferredSize(graphics, base.InheritedStyle, base.RowIndex, new Size(0, 0));
                }
                Image image = this.OwningNode.Image;
                if (image != null)
                {
                    this._imageWidth = image.Width + 2;
                    this._imageHeight = image.Height + 2;
                }
                else
                {
                    this._imageWidth = 0;
                    this._imageHeight = 0;
                }
                if (size.Height < this._imageHeight)
                {
                    base.Style.Padding = new Padding(((padding.Left + (level * INDENT_WIDTH)) + this._imageWidth) + INDENT_MARGIN, padding.Top + (this._imageHeight / 2), padding.Right, padding.Bottom + (this._imageHeight / 2));
                    this._imageHeightOffset = 2;
                }
                else
                {
                    base.Style.Padding = new Padding(((padding.Left + (level * INDENT_WIDTH)) + this._imageWidth) + INDENT_MARGIN, padding.Top, padding.Right, padding.Bottom);
                }
                this.calculatedLeftPadding = (((level - 1) * this._imageWidth) + this._imageWidth) + INDENT_MARGIN;
            }
        }
        internal virtual int GlyphMargin => (((this.Level - 1) * INDENT_WIDTH) + INDENT_MARGIN);
        internal virtual int GlyphOffset => ((this.Level - 1) * INDENT_WIDTH);
        /// <summary>
        /// 节点等级
        /// </summary>
        public int Level
        {
            get
            {
                TreeGridNode owningNode = this.OwningNode;
                if (owningNode != null)
                {
                    return owningNode.Level;
                }
                return -1;
            }
        }
        /// <summary>
        /// 所属Node
        /// </summary>
        public TreeGridNode OwningNode => (base.OwningRow as TreeGridNode);

        #endregion
    }
}


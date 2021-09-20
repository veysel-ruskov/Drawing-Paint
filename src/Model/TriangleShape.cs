using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Draw.src.Model
{
    [Serializable]
    class TriangleShape : Shape
    {
        public TriangleShape(RectangleF rect) : base(rect)
        {
        }

        public TriangleShape(TriangleShape triangle) : base(triangle)
        {
        }

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
                return true;
            else
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            //grfx.DrawRectangle(new Pen(Color.Red, BorderWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.DrawLine(new Pen(BorderColor, BorderWidth), Rectangle.X + Rectangle.Width/2, Rectangle.Y, Rectangle.X+Rectangle.Width, Rectangle.Y + Rectangle.Height);
            grfx.DrawLine(new Pen(BorderColor, BorderWidth), Rectangle.X + Rectangle.Width / 2, Rectangle.Y, Rectangle.X, Rectangle.Y + Rectangle.Height);
            grfx.DrawLine(new Pen(BorderColor, BorderWidth), Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);


        }
    }
}
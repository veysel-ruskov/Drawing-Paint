using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Draw.src.Model 
{
    [Serializable]
    class EllipseShape : Shape
    {
        public EllipseShape(RectangleF rect) : base(rect)
        {
        }

        public EllipseShape(EllipseShape ellip) : base(ellip)
        {
        }

        public override bool Contains(PointF point)
        {
           Point center = new Point
            {
                X = (int)(Rectangle.X + Rectangle.Width / 2),
                Y = (int)(Rectangle.Y + Rectangle.Height / 2)
            };
            if (base.Contains(point))
            {
                if (((Math.Pow((point.X - center.X), 2) / Math.Pow((Rectangle.Width / 2), 2))) 
                    + (Math.Pow((point.Y - center.Y), 2) / Math.Pow(Rectangle.Height / 2, 2)) <= 1)
                return true;
                else
                    return false;
            }
            else
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.RotateShape(grfx);
            base.DrawSelf(grfx);

            

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.DrawEllipse(new Pen(BorderColor, BorderWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            
        }
    }
}

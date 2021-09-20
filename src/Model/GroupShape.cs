using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Draw.src.Model
{
    [Serializable]
    class GroupShape : Shape
    {
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect) { }
        public GroupShape(GroupShape group) : base(group) { }



        public override Color FillColor
        {
            set
            {
                foreach (var item in GroupItems) item.FillColor = value;
            }
        }

        public override PointF Location
        {
            set
            {
                float newx = value.X - Location.X;
                float newy = value.Y - Location.Y;

                base.Location = value;
                foreach (var item in GroupItems)
                {
                    item.Location = new PointF(item.Location.X + newx,
                                               item.Location.Y + newy);
                }
            }
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                foreach (var item in GroupItems)
                {
                    if (item.Contains(point)) return true;
                }
                return true;
            }
            else
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach (var item in GroupItems)
            {
                item.DrawSelf(grfx);
            }
        }
    }
}


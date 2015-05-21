using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2F
    {
        public System.Drawing.PointF ToSystemPoint()
        {
            return new System.Drawing.PointF(x, y);
        }

        public Vector2F ToPolar()
        {
            return new Vector2F(
                Distance(Vector2F.Zero, this),
                (float)Math.Atan2(this.y, this.x));
        }

        public Vector2F ToCartesian()
        {
            return new Vector2F(
                Math.Acos(this.y) * this.x,
                Math.Asin(this.y) * this.x);
        }

        public Matrix2D ToMatrix2D()
        {
            return new Matrix2D(2, 1, new float[] { x, y });
        }

        public override string ToString()
        {
            return "{ X: " + x.ToString("F") + ", Y: " + y.ToString("F") + " }";
        }
    }
}

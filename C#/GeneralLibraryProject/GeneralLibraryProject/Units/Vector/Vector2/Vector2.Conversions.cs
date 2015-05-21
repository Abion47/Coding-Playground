using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2
    {
        public System.Drawing.Point ToSystemPoint()
        {
            return new System.Drawing.Point(x, y);
        }

        public Vector2 ToPolar()
        {
            return new Vector2(
                Distance(Vector2.Zero, this).ToInt(),
                Math.Atan2((double)this.y, (double)this.x).ToInt());
        }

        public Matrix2D ToMatrix2D()
        {
            return new Matrix2D(2, 1, new float[] { x, y });
        }

        public override string ToString()
        {
            return "{ X: " + x + ", Y: " + y + " }";
        }
    }
}

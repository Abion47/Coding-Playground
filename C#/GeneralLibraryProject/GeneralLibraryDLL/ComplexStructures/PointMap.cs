using org.general.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.ComplexStructures
{
    class PointMap
    {
        Vector2F[] points;

        public Vector2F[] Points { get { return points; } set { this.points = value; } }
    }
}

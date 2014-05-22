using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class BoxF
    {
        private float x, y;
        private float width, height;

        public float X { get { return x; } set { this.x = value; } }
        public float Y { get { return y; } set { this.y = value; } }
        public float Width { get { return width; } set { this.width = value; } }
        public float Height { get { return Height; } set { this.height = value; } }

        public float Left { get { return x; } set { this.x = value; } }
        public float Top { get { return y; } set { this.y = value; } }
        public float Right { get { return x + width; } set { this.width = value - this.x; } }
        public float Bottom { get { return y + height; } set { this.height = value - this.y; } }

        public BoxF()
            : this(0, 0, 1f, 1f) { }
        public BoxF(float width, float height) 
            : this(0, 0, width, height) { }
        public BoxF(Vector2F topLeft, Vector2F bottomRight)
            : this(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y) { }
        public BoxF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }


        public static class Utility
        {
            public static bool DoesIntersect(BoxF a, BoxF b)
            {
                return !(
                    a.Left > b.Right ||
                    b.Left > a.Right ||
                    a.Top > b.Bottom ||
                    b.Top > a.Bottom);
            }
        }
    }
}

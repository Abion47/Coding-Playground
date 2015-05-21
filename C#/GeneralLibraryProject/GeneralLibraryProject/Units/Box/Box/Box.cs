using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class Box
    {
        private int x, y;
        private int width, height;

        public int X { get { return x; } set { this.x = value; } }
        public int Y { get { return y; } set { this.y = value; } }
        public int Width { get { return width; } set { this.width = value; } }
        public int Height { get { return Height; } set { this.height = value; } }

        public int Left { get { return x; } set { this.x = value; } }
        public int Top { get { return y; } set { this.y = value; } }
        public int Right { get { return x + width; } set { this.width = value - this.x; } }
        public int Bottom { get { return y + height; } set { this.height = value - this.y; } }

        public int CenterX { get { return (Left + Right) / 2; } }
        public int CenterY { get { return (Top + Bottom) / 2; } }

        public Box()
            : this(0, 0, 1, 1) { }
        public Box(int width, int height)
            : this(0, 0, width, height) { }
        public Box(Vector2 topLeft, Vector2 bottomRight)
            : this(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y) { }
        public Box(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static implicit operator BoxF(Box b) { return new BoxF(b.x, b.y, b.width, b.height); }

        public Rectangle ToSystemRect() { return new Rectangle(x, y, width, height); }

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

        public IEnumerator<Vector2> GetEnumerator()
        {
            Vector2 v = new Vector2();

            for (int x = Left; x <= Right; x++)
            {
                for (int y = Top; y <= Bottom; y++)
                {
                    v.X = x;
                    v.Y = y;
                    yield return v;
                }
            }
        }

        public override string ToString()
        {
            return "{ X: " + x + ", Y: " + y + ", Width: " + width + ", Height: " + height + " }";
        }
    }
}

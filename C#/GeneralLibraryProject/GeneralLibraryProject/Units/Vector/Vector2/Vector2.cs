using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2
    {
        int x;
        int y;

        public int X { get { return x; } set { this.x = value; } }
        public int Y { get { return y; } set { this.y = value; } }

        public Vector2() { }
        public Vector2(int x, int y) { this.x = x; this.y = y; }

        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.x + b.x, a.y + b.y); }
        public static Vector2 operator +(Vector2 a, int f) { return new Vector2(a.x + f, a.y + f); }
        public static Vector2 operator +(int f, Vector2 a) { return new Vector2(a.x + f, a.y + f); }
        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.x - b.x, a.y - b.y); }
        public static Vector2 operator -(Vector2 a, int f) { return new Vector2(a.x - f, a.y - f); }
        public static Vector2 operator -(int f, Vector2 a) { return new Vector2(a.x - f, a.y - f); }
        public static Vector2 operator *(Vector2 a, Vector2 b) { return new Vector2(a.x * b.x, a.y * b.y); }
        public static Vector2 operator *(Vector2 a, int f) { return new Vector2(a.x * f, a.y * f); }
        public static Vector2 operator *(int f, Vector2 a) { return new Vector2(a.x * f, a.y * f); }

        public static implicit operator Vector2F(Vector2 p) { return new Vector2F(p.x, p.y); }

        public int DotProduct(Vector2 other)
        {
            return this.x * other.x + this.y * other.y;
        }

        public int CrossProduct(Vector2 other)
        {
            return this.x * other.y - this.y * other.x;
        }

        public Vector2 Perpendicular()
        {
            return new Vector2(this.Y, -this.X);
        }
    }
}

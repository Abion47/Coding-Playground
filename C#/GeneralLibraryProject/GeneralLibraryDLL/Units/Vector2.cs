using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class Vector2
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

        public override string ToString()
        {
            return "{ X: " + x + ", Y: " + y + " }";
        }

        public System.Drawing.Point ToSystemPoint()
        {
            return new System.Drawing.Point(x, y);
        }

        public Vector2 Perpendicular()
        {
            return new Vector2(this.Y, -this.X);
        }

        #region Static Classes
        public static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }
        #endregion

        public static class Utility
        {
            public static System.Drawing.Point[] ToSystemPointArray(Vector2[] arr)
            {
                System.Drawing.Point[] ret = new System.Drawing.Point[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                    ret[i] = new System.Drawing.Point(arr[i].x, arr[i].y);
                return ret;
            }
        }
    }
}

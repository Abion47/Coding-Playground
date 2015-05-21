using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2F
    {
        float x;
        float y;

        public float X { get { return x; } set { this.x = value; } }
        public float Y { get { return y; } set { this.y = value; } }

        public Vector2F() : this(0, 0) { }
        public Vector2F(double x, double y) : this((float)x, (float) y) { }
        public Vector2F(float x, float y) { this.x = x; this.y = y; }

        public bool IsEmpty() { return float.IsNaN(x) && float.IsNaN(y); }

        public static Vector2F operator +(Vector2F a, Vector2F b) { return new Vector2F(a.x + b.x, a.y + b.y); }
        public static Vector2F operator +(Vector2F a, float f) { return new Vector2F(a.x + f, a.y + f); }
        public static Vector2F operator +(float f, Vector2F a) { return new Vector2F(a.x + f, a.y + f); }
        public static Vector2F operator -(Vector2F a, Vector2F b) { return new Vector2F(a.x - b.x, a.y - b.y); }
        public static Vector2F operator -(Vector2F a, float f) { return new Vector2F(a.x - f, a.y - f); }
        public static Vector2F operator -(float f, Vector2F a) { return new Vector2F(a.x - f, a.y - f); }
        public static Vector2F operator *(Vector2F a, Vector2F b) { return new Vector2F(a.x * b.x, a.y * b.y); }
        public static Vector2F operator *(Vector2F a, float f) { return new Vector2F(a.x * f, a.y * f); }
        public static Vector2F operator *(float f, Vector2F a) { return new Vector2F(a.x * f, a.y * f); }

        public static implicit operator Vector2(Vector2F p) { return new Vector2((int)(p.x + 0.5f), (int)(p.y + 0.5f)); }

        public float DotProduct(Vector2F other)
        {
            return this.x * other.x + this.y * other.y;
        }
        public float CrossProduct(Vector2F other)
        {
            return this.x * other.y - this.y * other.x;
        }

        public Vector2F Perpendicular()
        {
            return new Vector2F(this.Y, -this.X);
        }
    }
}

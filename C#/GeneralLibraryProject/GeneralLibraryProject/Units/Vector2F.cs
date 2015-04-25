using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class Vector2F
    {
        float x;
        float y;

        public float X { get { return x; } set { this.x = value; } }
        public float Y { get { return y; } set { this.y = value; } }

        public Vector2F() : this(0, 0) { }
        public Vector2F(double x, double y) : this((float)x, (float) y) { }
        public Vector2F(float x, float y) { this.x = x; this.y = y; }

        public bool IsEmpty() { return float.IsNaN(x) && float.IsNaN(y); }
        public static Vector2F Empty { get { return new Vector2F(float.NaN, float.NaN); } }

        public static Vector2F operator +(Vector2F a, Vector2F b) { return new Vector2F(a.x + b.x, a.y + b.y); }
        public static Vector2F operator +(Vector2F a, float f) {  return new Vector2F(a.x + f, a.y + f); }
        public static Vector2F operator -(Vector2F a, Vector2F b) { return new Vector2F(a.x - b.x, a.y - b.y); }
        public static Vector2F operator -(Vector2F a, float f) { return new Vector2F(a.x - f, a.y - f); }

        public static implicit operator Vector2(Vector2F p) { return new Vector2((int)p.x, (int)p.y); }

        public float DotProduct(Vector2F other)
        {
            return this.x * other.x + this.y * other.y;
        }
        public float CrossProduct(Vector2F other)
        {
            return this.x * other.y - this.y * other.x;
        }

        public override string ToString()
        {
            return "[ " + x + ", " + y + " ]";
        }

        public System.Drawing.PointF ToSystemPoint()
        {
            return new System.Drawing.PointF(x, y);
        }

        public Matrix2D ToMatrix2D()
        {
            return new Matrix2D(2, 1, new float[] { x, y });
        }

        public static class Utility
        {
            private static Random r;

            static Utility()
            {
                r = new Random();
            }

            public static Vector2F Random() { return Random(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue); }
            public static Vector2F Random(float maxX, float maxY) { return Random(float.MinValue, float.MinValue, maxX, maxY); }
            public static Vector2F Random(float minX, float minY, float maxX, float maxY)
            {
                float x = (float)r.Next((int)minX, (int)maxX) + (float)r.NextDouble();
                float y = (float)r.Next((int)minY, (int)maxY) + (float)r.NextDouble();
                return new Vector2F(x, y);
            }

            public static System.Drawing.RectangleF GetBoundingBox(org.general.Units.Vector2F[] points)
            {
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;

                foreach (var p in points)
                {
                    minX = Math.Min(minX, p.X);
                    minY = Math.Min(minY, p.Y);
                    maxX = Math.Max(maxX, p.X);
                    maxY = Math.Max(maxY, p.Y);
                }

                return new System.Drawing.RectangleF(minX, minY, maxX - minX, maxY - minY);
            }

            public static float Distance(Vector2F a, Vector2F b)
            {
                return (float)Math.Sqrt(DistanceSquared(a, b));
            }
            public static float DistanceSquared(Vector2F a, Vector2F b)
            {
                return ((a.X - b.X) * (a.X - b.X)) + ((a.Y - b.Y) * (a.Y - b.Y));
            }
            public static float Slope(Vector2F a, Vector2F b)
            {
                if (a.x - b.x == 0)
                    return float.NaN;
                return (a.y - b.y) / (a.x - b.x);
            }

            public static System.Drawing.PointF[] ToSystemPointArray(Vector2F[] arr)
            {
                System.Drawing.PointF[] ret = new System.Drawing.PointF[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                    ret[i] = new System.Drawing.PointF(arr[i].x, arr[i].y);
                return ret;
            }

            public static Vector2F FindCenter(Vector2F[] arr)
            {
                Vector2F center = new Vector2F(0, 0);

                foreach (var v in arr)
                    center += v;

                center.x /= arr.Length;
                center.y /= arr.Length;

                return center;
            }

            public static Vector2F FindIntersectionPoint(Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
            {
                float a1 = p2.Y - p1.Y;
                float b1 = p2.X - p1.X;
                float c1 = a1 * p1.X + b1 * p1.X;

                float a2 = p4.Y - p3.Y;
                float b2 = p4.X - p3.X;
                float c2 = a2 * p3.X + b2 * p3.X;

                float det = a1 * b2 - a2 * b1;

                if (det == 0)
                {
                    return Vector2F.Empty;
                }
                else
                {
                    Vector2F ret = new Vector2F(
                        (b2 * c1 - b1 * c2) / det,
                        (a1 * c2 - a2 * c1) / det);

                    if (Math.Min(p1.X, p2.X) <= ret.X
                        && ret.X <= Math.Max(p1.X, p2.X)
                        && Math.Min(p1.Y, p2.Y) <= ret.Y
                        && ret.Y <= Math.Max(p1.Y, p2.Y))
                    {
                        return ret;
                    }
                    else
                    {
                        return Vector2F.Empty;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2F
    {
        #region Constant Types
        public static Vector2F Zero { get { return new Vector2F(0, 0); } }
        public static Vector2F Empty { get { return new Vector2F(float.NaN, float.NaN); } }
        #endregion

        #region Instance Mirror Functions
        public static float DotProduct(Vector2F a, Vector2F b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float CrossProduct(Vector2F a, Vector2F b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static Vector2F Perpendicular(Vector2F v)
        {
            return new Vector2F(v.Y, -v.X);
        }
        #endregion

        private static Random r;

        private static void InitRandom() { if (r == null) r = new Random(); }
        public static Vector2F Random() { return Random(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue); }
        public static Vector2F Random(float maxX, float maxY) { return Random(float.MinValue, float.MinValue, maxX, maxY); }
        public static Vector2F Random(float minX, float minY, float maxX, float maxY)
        {
            InitRandom();
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

        public static Vector2F Midpoint(Vector2F a, Vector2F b)
        {
            return new Vector2F((a.X + b.X) / 2, (a.Y + b.Y) / 2);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Vector2
    {
        #region Constant Types
        public static Vector2 Zero { get { return new Vector2(0, 0); } }
        #endregion

        #region Instance Mirror Functions
        public static int DotProduct(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static int CrossProduct(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }
        #endregion

        private static Random r;

        private static void InitRandom() { if (r == null) r = new Random(); }
        public static Vector2 Random() { return Random(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue); }
        public static Vector2 Random(int maxX, int maxY) { return Random(int.MinValue, int.MinValue, maxX, maxY); }
        public static Vector2 Random(int minX, int minY, int maxX, int maxY)
        {
            InitRandom();
            int x = r.Next(minX, maxX);
            int y = r.Next(minY, maxY);
            return new Vector2(x, y);
        }

        public static System.Drawing.RectangleF GetBoundingBox(org.general.Units.Vector2[] points)
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

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(DistanceSquared(a, b));
        }
        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            return ((a.X - b.X) * (a.X - b.X)) + ((a.Y - b.Y) * (a.Y - b.Y));
        }
        public static float Slope(Vector2 a, Vector2 b)
        {
            if (a.x - b.x == 0)
                return float.NaN;
            return (a.y - b.y) / (a.x - b.x);
        }

        public static System.Drawing.Point[] ToSystemPointArray(Vector2[] arr)
        {
            System.Drawing.Point[] ret = new System.Drawing.Point[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                ret[i] = new System.Drawing.Point(arr[i].x, arr[i].y);
            return ret;
        }

        public static Vector2 Midpoint(Vector2 a, Vector2 b)
        {
            return new Vector2((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        public static Vector2 FindCenter(Vector2[] arr)
        {
            Vector2 center = new Vector2(0, 0);

            foreach (var v in arr)
                center += v;

            center.x /= arr.Length;
            center.y /= arr.Length;

            return center;
        }
    }
}

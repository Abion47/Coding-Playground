using org.general.Units;
using org.general.Units.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.general
{
    public class Utility
    {
        
    }

    public class GeometryUtility
    {
        public static PointF FindIntersectionPoint(PointF p1, PointF p2, PointF p3, PointF p4) { return FindIntersectionPoint(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, p4.X, p4.Y).ToSystemPoint(); }
        public static Vector2F FindIntersectionPoint(Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4) { return FindIntersectionPoint(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, p4.X, p4.Y); }
        public static Vector2F FindIntersectionPoint(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            float rx = x2 - x1;
            float ry = y2 - y1;
            float sx = x4 - x3;
            float sy = y4 - y3;

            float rxs = rx * sy - ry * sx;
            float qpx = x3 - x1;
            float qpy = y3 - y1;
            float qpxr = qpx * ry - rx * qpy;

            if (rxs.IsZero())
            {
                return Vector2F.Empty;
            }

            float qpxs = qpx * sy - sx * qpy;
            float t = qpxs / rxs;
            float u = qpxr / rxs;

            if (0 <= t && t < 1
                && 0 <= u && u <= 1)
            {
                return new Vector2F(x1 + (t * rx), y1 + (t * ry));
            }

            return Vector2F.Empty;

            /*float a1 = y2 - y1;
            float b1 = x2 - x1;
            float c1 = a1 * x1 + b1 * x1;

            float a2 = y4 - y3;
            float b2 = x4 - x3;
            float c2 = a2 * x3 + b2 * x3;

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

                if (Math.Min(x1, x2) <= ret.X
                    && ret.X <= Math.Max(x1, x2)
                    && Math.Min(y1, y2) <= ret.Y
                    && ret.Y <= Math.Max(y1, y2))
                {
                    return ret;
                }
                else
                {
                    return Vector2F.Empty;
                }
            }*/
        }

        public static float Distance(PointF q1, PointF q2)
        {
            return Distance(q1.X, q1.Y, q2.X, q2.Y);
        }
        public static float Distance(Vector2F q1, Vector2F q2)
        {
            return Distance(q1.X, q1.Y, q2.X, q2.Y);
        }
        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
        }

        public static float GetSlope(PointF p1, PointF p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }
        public static float GetSlope(Vector2F p1, Vector2F p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }
    }

    public class PolygonUtility
    {
        public static RectangleF GetBoundingBox(params PointF[] points)
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

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }
        public static BoxF GetBoundingBox(params Vector2F[] points)
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

            return new BoxF(minX, minY, maxX - minX, maxY - minY);
        }
        public static Box GetBoundingBox(params Vector2[] points)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var p in points)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
            }

            return new Box(minX, minY, maxX - minX, maxY - minY);
        }

        public static bool PointInPolygon(Vector2F v, params Vector2F[] poly)
        {
            bool oddNodes = false;
            int i, j = poly.Length - 1;
            for (i = 0; i < poly.Length; i++)
            {
                if ((poly[i].Y < v.Y && poly[j].Y >= v.Y
                    || poly[j].Y < v.Y && poly[i].Y >= v.Y)
                    && (poly[i].X <= v.X || poly[j].X <= v.X))
                {
                    oddNodes ^= (poly[i].X + (v.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) * (poly[j].X - poly[i].X) < v.X);
                }

                j = i;
            }

            return oddNodes;
        }
    }

    public class ArrayUtility
    {
        public static void InitializeArrayWithValue<T>(ref T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }

        public static void TrimArray<T>(ref T[] array, T trimmedValue)
            where T : IEquatable<T>
        {
            int start = -1;
            int end = -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].Equals(trimmedValue))
                {
                    start = i;
                    break;
                }
            }

            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (!array[i].Equals(trimmedValue))
                {
                    end = i;
                    break;
                }
            }

            T[] trimmed = new T[end - start + 1];

            for (int i = 0; i < trimmed.Length; i++)
            {
                trimmed[i] = array[i + start];
            }

            array = trimmed;
        }

        public static void SortArray<T>(ref T[] array)
            where T : IComparable<T>
        {
            int swapIdx;
            T buf;

            for (int i = 0; i < array.Length; i++)
            {
                swapIdx = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[swapIdx].CompareTo(array[j]) > 0)
                    {
                        swapIdx = j;
                    }
                }

                if (swapIdx != i)
                {
                    buf = array[i];
                    array[i] = array[swapIdx];
                    array[swapIdx] = buf;
                }
            }
        }

        public static void ResizeArray<T>(ref T[] array, int newSize, bool appendToBeginning = false)
        {
            T[] buf = new T[array.Length];
            array.CopyTo(buf, 0);
            array = new T[newSize];
            int diff = newSize - buf.Length;

            for (int i = 0; i < buf.Length; i++)
            {
                if (appendToBeginning)
                {
                    if (i + diff >= 0 && i + diff < array.Length)
                    {
                        array[i + diff] = buf[i];
                    }
                    else
                    {
                        buf[i] = default(T);
                    }
                }
                else
                {
                    if (i < array.Length)
                    {
                        array[i] = buf[i];
                    }
                    else
                    {
                        buf[i] = default(T);
                    }
                }
            }
        }

        public static T[] DeepCopy<T>(ref T[] array)
        {
            T[] arr = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                arr[i] = array[i];
            }

            return arr;
        }
    }

    public class ColorUtility
    {
        private static Random r = new Random();

        public static Color Random()
        {
            return Color.FromArgb(r.Next() & 0x000000FF, r.Next() & 0x000000FF, r.Next() & 0x000000FF);
        }
    }

    public class ImageUtility
    {
        public static Bitmap ConvertPixelFormat(Bitmap bmp, System.Drawing.Imaging.PixelFormat format)
        {
            if (bmp.PixelFormat == format)
                return bmp;

            Bitmap ret = new Bitmap(bmp.Width, bmp.Height, format);

            using (Graphics g = Graphics.FromImage(ret))
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

            return ret;
        }

        
    }
}

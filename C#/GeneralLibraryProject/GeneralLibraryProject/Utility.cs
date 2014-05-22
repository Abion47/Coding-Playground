using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.general
{
    class Utility
    {
        public static RectangleF GetBoundingBox(PointF[] points)
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

        public static PointF FindIntersectionPoint(PointF p1, PointF p2, PointF p3, PointF p4)
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
                return PointF.Empty;
            }
            else
            {
                PointF ret = new PointF(
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
                    return PointF.Empty;
                }
            }
        }

        public static float Distance(PointF q1, PointF q2)
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

    }

    public class ColorUtility
    {
        private static Random r = new Random();

        public static Color Random()
        {
            return Color.FromArgb(r.Next() & 0x000000FF, r.Next() & 0x000000FF, r.Next() & 0x000000FF);
        }
    }

}

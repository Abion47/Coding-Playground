using org.general.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.general
{
    public class Drawing
    {
        public static int Width = Int32.MaxValue;
        public static int Height = Int32.MaxValue;

        public class Pixel
        {
            public static unsafe void SetPixel(ref byte* ptr, int stride, int bpp, Color c, Vector2 p) { SetPixel(ref ptr, stride, bpp, c, p.X, p.Y); }
            public static unsafe void SetPixel(ref byte* ptr, int stride, int bpp, Color c, int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;

                int idx = (y * stride) + (x * bpp);
                ptr[idx] = c.B;
                ptr[idx + 1] = c.G;
                ptr[idx + 2] = c.R;
                ptr[idx + 3] = c.A;
            }
        }

        public class Line
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F start, Vector2F end)
            {
                if (start.X == end.X && start.Y == end.Y)
                {
                    Pixel.SetPixel(ref ptr, stride, bpp, c, start);
                }

                Vector2 p1, p2;

                if (start.X < end.X)
                {
                    p1 = new Vector2((int)start.X, (int)start.Y);
                    p2 = new Vector2((int)end.X, (int)end.Y);
                }
                else
                {
                    p1 = new Vector2((int)end.X, (int)end.Y);
                    p2 = new Vector2((int)start.X, (int)start.Y);
                }

                int deltax = p2.X - p1.X;
                int deltay = p2.Y - p1.Y;

                if (deltax == 0)
                {
                    if (p1.Y > p2.Y)
                        for (int i = p2.Y; i <= p1.Y; i++)
                            Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(p1.X, i));
                    else
                        for (int i = p1.Y; i <= p2.Y; i++)
                            Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(p1.X, i));

                    return;
                }
                else if (deltay == 0)
                {
                    for (int i = p1.X; i <= p2.X; i++)
                        Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(i, p1.Y));

                    return;
                }

                float error = 0;
                float deltaerr = (float)deltay / (float)deltax;
                int y = p1.Y;

                for(int x = p1.X; x < p2.X; x++)
                {
                    Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(x, y));
                    error += deltaerr;
                    if (error >= 0.5)
                    {
                        while (error >= 0.5)
                        {
                            y += 1;
                            error -= 1;
                            if (error >= 0.5)
                                Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(x, y));
                        }
                    }
                    else if (error <= -0.5)
                    {
                        while (error <= -0.5)
                        {
                            y -= 1;
                            error += 1;
                            if (error <= -0.5)
                                Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(x, y));
                        }
                    }
                }
            }
        }

        public class Triangle
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F p1, Vector2F p2, Vector2F p3)
            {
                Line.Draw(ref ptr, stride, bpp, c, p1, p2);
                Line.Draw(ref ptr, stride, bpp, c, p2, p3);
                Line.Draw(ref ptr, stride, bpp, c, p3, p1);
            }

            public static unsafe void Fill(ref byte* ptr, int stride, int bpp, Color c, Vector2F p1, Vector2F p2, Vector2F p3)
            {
                Vector2[] points = new Vector2[] { p1, p2, p3 }.OrderBy(p => p.X).ToArray();

                int[] deltax = new int[3];
                int[] deltay = new int[3];
                float[] deltaerr = new float[3];

                for (int i = 0; i < 3; i++)
                {
                    deltax[i] = points[i + 1 == 3 ? 0 : i + 1].X - points[i].X;
                    deltay[i] = points[i + 1 == 3 ? 0 : i + 1].Y - points[i].Y;
                    deltaerr[i] = (float)deltay[i] / (float)deltax[i];
                }

                int active_a = 0;
                int active_b = 2;

                Vector2 pa = points[0];
                Vector2 pb = points[0];

                float error_a = 0;
                float error_b = 0;

                while (true)
                {
                    Line.Draw(ref ptr, stride, bpp, c, pa, pb);

                    if (pa.X == points[2].X || pb.X == points[2].X)
                        break;

                    pa.X++;
                    pb.X++;
                    error_a += deltaerr[active_a];
                    error_b += deltaerr[active_b];

                    if (error_a >= 0.5)
                    {
                        while (error_a >= 0.5)
                        {
                            pa.Y += 1;
                            error_a -= 1;
                        }
                    }
                    else if (error_a <= -0.5)
                    {
                        while (error_a <= -0.5)
                        {
                            pa.Y -= 1;
                            error_a += 1;
                        }
                    }

                    if (error_b >= 0.5)
                    {
                        while (error_b >= 0.5)
                        {
                            pb.Y += 1;
                            error_b -= 1;
                        }
                    }
                    else if (error_b <= -0.5)
                    {
                        while (error_b <= -0.5)
                        {
                            pb.Y -= 1;
                            error_b += 1;
                        }
                    }

                    if (pa == points[1])
                        active_a = 1;
                    else if (pb == points[1])
                        active_b = 1;
                }
            }
        }

        public class Rectangle
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, RectangleF r)
            {
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Top, r.Left), new Vector2F(r.Top, r.Right));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Top, r.Left), new Vector2F(r.Bottom, r.Left));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Bottom, r.Right), new Vector2F(r.Top, r.Right));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Bottom, r.Right), new Vector2F(r.Bottom, r.Left));
            }
        }

        public class Quadrilateral
        {
            public static Bitmap Draw(Color c, float p1x, float p1y, float p2x, float p2y, float p3x, float p3y, float p4x, float p4y)
                 { return Draw(c, new Vector2F(p1x, p1y), new Vector2F(p2x, p2y), new Vector2F(p3x, p3y), new Vector2F(p4x, p4y)); }
            public static unsafe Bitmap Draw(Color c, Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
            {
                float minX = Math.Min(Math.Min(p1.X, p2.X), Math.Min(p3.X, p4.X));
                float minY = Math.Min(Math.Min(p1.Y, p2.Y), Math.Min(p3.Y, p4.Y));
                float maxX = Math.Max(Math.Max(p1.X, p2.X), Math.Max(p3.X, p4.X));
                float maxY = Math.Max(Math.Max(p1.Y, p2.Y), Math.Max(p3.Y, p4.Y));

                int width = (int)(maxX + minX);
                int height = (int)(maxY + minY);

                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                Draw(ref ptr, stride, bpp, c, p1, p2, p3, p4);

                bmp.UnlockBits(data);
                return bmp;
            }
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
            {
                Line.Draw(ref ptr, stride, bpp, c, p1, p2);
                Line.Draw(ref ptr, stride, bpp, c, p2, p3);
                Line.Draw(ref ptr, stride, bpp, c, p3, p4);
                Line.Draw(ref ptr, stride, bpp, c, p4, p1);
            }
        }

        public class Arc
        {

        }

        public class Circle
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F center, float r)
            {
                int b = (int)(r + 0.5f);
                int lastX = b;
                for (int y = 0; y <= b; y++)
                {
                    float a = y == 0 ? 0 : (float)Math.Asin(y / r);
                    int x = (int)(Math.Cos(a) * r + 0.5f);

                    if (x != lastX)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, y) + center, new Vector2F(x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, -y) + center, new Vector2F(x, -y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, y) + center, new Vector2F(-x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, -y) + center, new Vector2F(-x, -y) + center);
                    }
                    else
                    {
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(x + center.X), (int)(y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(x + center.X), (int)(-y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(-x + center.X), (int)(y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(-x + center.X), (int)(-y + center.Y));
                    }

                    lastX = x;
                }
            }

            public static unsafe void Fill(ref byte* ptr, int stride, int bpp, Color c, Vector2F center, float r, int steps = 180)
            {
                int b = (int)(r + 0.5f);
                int lastX = b;
                for (int y = 0; y <= b; y++)
                {
                    float a = y == 0 ? 0 : (float)Math.Asin(y / r);
                    int x = (int)(Math.Cos(a) * r + 0.5f);

                    if (x != lastX)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, y) + center, new Vector2F(x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, -y) + center, new Vector2F(x, -y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, y) + center, new Vector2F(-x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, -y) + center, new Vector2F(-x, -y) + center);

                        //int inc = lastX < x ? 1 : -1;
                        for (int i = x; i <= lastX - 1; i++)
                        {
                            Line.Draw(ref ptr, stride, bpp, c, new Vector2F(i, -y) + center, new Vector2F(i, y) + center);
                            Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-i, -y) + center, new Vector2F(-i, y) + center);
                        }
                    }
                    else
                    {
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(x + center.X), (int)(y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(x + center.X), (int)(-y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(-x + center.X), (int)(y + center.Y));
                        Pixel.SetPixel(ref ptr, stride, bpp, c, (int)(-x + center.X), (int)(-y + center.Y));

                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(x, -y) + center, new Vector2F(x, y) + center);
                    }

                    

                    

                    lastX = x;
                }
            }
        }

        public class Ellipse
        {

        }

        public class Bezier
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F[] controlPoints, int steps)
            {
                Vector2F point = null;
                Vector2F last = null;

                for (int i = 0; i <= steps; i++)
                {
                    float t = Functions.Interpolation.Lerp(0.0f, 1.0f, (float)i / steps);
                    point = FindPointInCurve(controlPoints, t);

                    if (last != null)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, last, point);
                    }

                    last = point;
                }
            }

            private static Vector2F FindPointInCurve(Vector2F[] q, float t)
            {
                if (q.Length <= 1)
                {
                    throw new Exception("Must have at least two anchor points");
                }

                if (q.Length == 2)
                {
                    return FindPointInLinearCurve(q[0], q[1], t);
                }

                Vector2F[] q_sub = new Vector2F[q.Length - 1];

                for (int i = 0; i < q_sub.Length; i++)
                {
                    q_sub[i] = FindPointInLinearCurve(q[i], q[i + 1], t);
                }

                return FindPointInCurve(q_sub, t);
            }

            private static Vector2F FindPointInLinearCurve(Vector2F q1, Vector2F q2, float t)
            {
                return new Vector2F((q2.X - q1.X) * t + q1.X, (q2.Y - q1.Y) * t + q1.Y);
            }
        }
    }
}

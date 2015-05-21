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
            public static unsafe void SetPixel(ref byte* ptr, int stride, int bpp, Color c, float x, float y) { SetPixel(ref ptr, stride, bpp, c, (int)(x + 0.5f), (int)(y + 0.5f)); }
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

            public static unsafe Color GetPixelColor(ref byte* ptr, int stride, int bpp, Vector2 p) { return GetPixelColor(ref ptr, stride, bpp, p.X, p.Y); }
            public static unsafe Color GetPixelColor(ref byte* ptr, int stride, int bpp, int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return Color.Empty;

                int idx = (y * stride) + (x * bpp);
                return Color.FromArgb(ptr[idx + 3], ptr[idx + 2], ptr[idx + 1], ptr[idx]);
            }

            public static unsafe int GetPixelInt(ref byte* ptr, int stride, int bpp, Vector2 p) { return GetPixelInt(ref ptr, stride, bpp, p.X, p.Y); }
            public static unsafe int GetPixelInt(ref byte* ptr, int stride, int bpp, int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return Int32.MinValue;

                int idx = (y * stride) + (x * bpp);
                return (int)(ptr[idx + 3] << 24) + (int)(ptr[idx + 2] << 16) + (int)(ptr[idx + 1] << 8) + ptr[idx];
            }
        }

        public class Line
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F start, Vector2F end) { Draw(ref ptr, stride, bpp, c, start.X, start.Y, end.X, end.Y); }
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, float x1, float y1, float x2, float y2) { Draw(ref ptr, stride, bpp, c, (int)(x1 + 0.5f), (int)(y1 + 0.5f), (int)(x2 + 0.5f), (int)(y2 + 0.5f)); }
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, int x1, int y1, int x2, int y2)
            {
                if (x1 == x2 && y1 == y2)
                {
                    Pixel.SetPixel(ref ptr, stride, bpp, c, x1, y1);
                }

                int p1x, p1y, p2x, p2y;

                if (x1 < x2)
                {
                    p1x = x1;
                    p1y = y1;
                    p2x = x2;
                    p2y = y2;
                }
                else
                {
                    p1x = x2;
                    p1y = y2;
                    p2x = x1;
                    p2y = y1;
                }

                int deltax = p2x - p1x;
                int deltay = p2y - p1y;

                if (deltax == 0)
                {
                    if (p1y > p2y)
                        for (int i = p2y; i <= p1y; i++)
                            Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(p1x, i));
                    else
                        for (int i = p1y; i <= p2y; i++)
                            Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(p1x, i));

                    return;
                }
                else if (deltay == 0)
                {
                    for (int i = p1x; i <= p2x; i++)
                        Pixel.SetPixel(ref ptr, stride, bpp, c, new Vector2(i, p1y));

                    return;
                }

                float error = 0;
                float deltaerr = (float)deltay / (float)deltax;
                int y = p1y;

                for (int x = p1x; x < p2x; x++)
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
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, BoxF r)
            {
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Top, r.Left), new Vector2F(r.Top, r.Right));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Top, r.Left), new Vector2F(r.Bottom, r.Left));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Bottom, r.Right), new Vector2F(r.Top, r.Right));
                Line.Draw(ref ptr, stride, bpp, c, new Vector2F(r.Bottom, r.Right), new Vector2F(r.Bottom, r.Left));
            }

            public static unsafe void Fill(ref byte* ptr, int stride, int bpp, Color c, BoxF b)
            {
                for (float f = b.Top; f <= b.Bottom; f++)
                {
                    Line.Draw(ref ptr, stride, bpp, c, new Vector2F(f, b.Left), new Vector2F(f, b.Right));
                }
            }
        }

        public class Quadrilateral
        {
            public static Bitmap Draw(Color c, float p1x, float p1y, float p2x, float p2y, float p3x, float p3y, float p4x, float p4y)
                 { return Draw(c, new Vector2F(p1x, p1y), new Vector2F(p2x, p2y), new Vector2F(p3x, p3y), new Vector2F(p4x, p4y)); }
            public static unsafe Bitmap Draw(Color c, Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
            {
                float minX = MathF.Min(MathF.Min(p1.X, p2.X), MathF.Min(p3.X, p4.X));
                float minY = MathF.Min(MathF.Min(p1.Y, p2.Y), MathF.Min(p3.Y, p4.Y));
                float maxX = MathF.Max(MathF.Max(p1.X, p2.X), MathF.Max(p3.X, p4.X));
                float maxY = MathF.Max(MathF.Max(p1.Y, p2.Y), MathF.Max(p3.Y, p4.Y));

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

            public static unsafe void Fill(ref byte* ptr, int stride, int bpp, Color c, Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
            {
                Box box = PolygonUtility.GetBoundingBox(p1, p2, p3, p4);

                for (int i = box.Top; i <= box.Bottom; i++)
                {
                    var nodes = FindIntersectionPoints(i, p1, p2, p3, p4);
                    for (int j = 0; j < nodes.Length - 1; j++)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, nodes[j], nodes[j + 1]);
                    }
                }
            }
            private static Vector2F[] FindIntersectionPoints(float y, params Vector2F[] poly)
            {
                List<Vector2F> nodes = new List<Vector2F>();
                int i, j = poly.Length - 1;
                var box = PolygonUtility.GetBoundingBox(poly);

                for (i = 0; i < poly.Length; i++)
                {
                    Vector2F intersection;
                    if (GeometryUtility.FindIntersectionPoint(out intersection, box.Left, y, box.Right, y, poly[i].X, poly[i].Y, poly[j].X, poly[j].Y)) 
                    {
                        nodes.Add(intersection);
                    }

                    j = i;
                }

                nodes.Sort((a, b) => a.X.CompareTo(b.X));
                return nodes.ToArray();
            }
        }

        public class Arc
        {

        }

        public class Circle
        {
            public static unsafe void Draw(ref byte* ptr, int stride, int bpp, Color c, Vector2F center, float r)
            {
                float b = r + 0.5f;
                float lastX = b;
                for (int y = 0; y <= b; y++)
                {
                    float a = y == 0 ? 0 : MathF.Asin(y / r);
                    float x = MathF.Cos(a) * r + 0.5f;

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
                if (r < 1)
                {
                    Pixel.SetPixel(ref ptr, stride, bpp, c, center);
                    return;
                }

                float b = r + 0.5f;
                float lastX = b;
                for (int y = 0; y <= b; y++)
                {
                    float a = y == 0 ? 0 : MathF.Asin(y / r);
                    if (float.IsNaN(a)) a = 0;
                    float x = MathF.Cos(a) * r + 0.5f;

                    if (x != lastX)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, y) + center, new Vector2F(x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(lastX - 1, -y) + center, new Vector2F(x, -y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, y) + center, new Vector2F(-x, y) + center);
                        Line.Draw(ref ptr, stride, bpp, c, new Vector2F(-lastX + 1, -y) + center, new Vector2F(-x, -y) + center);

                        //int inc = lastX < x ? 1 : -1;
                        for (int i = (int)x; i <= lastX - 1; i++)
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
                    float t = MathF.Lerp(0.0f, 1.0f, (float)i / steps);
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

            public static unsafe void DrawCubic(ref byte* ptr, int stride, int bpp, Color c, Vector2F start, Vector2F c1, Vector2F c2, Vector2F end, int steps)
            {
                Vector2F point = null;

                for (int i = 0; i <= steps; i++)
                {
                    float t = MathF.Lerp(0.0f, 1.0f, (float)i / steps);
                    point = FindCubicPointInCurve(start, c1, c2, end, t);
                    Pixel.SetPixel(ref ptr, stride, bpp, c, point);
                }
            }

            private static Vector2F FindCubicPointInCurve(Vector2F start, Vector2F c1, Vector2F c2, Vector2F end, float t)
            {
                return new Vector2F(
                    start.X * (1 - t) * (1 - t) * (1 - t) + c1.X * 3 * (1 - t) * (1 - t) * t + c2.X * 3 * (1 - t) * t * t + end.X * t * t * t,
                    start.Y * (1 - t) * (1 - t) * (1 - t) + c1.Y * 3 * (1 - t) * (1 - t) * t + c2.Y * 3 * (1 - t) * t * t + end.Y * t * t * t);
            }

            public static unsafe void DrawMagic(ref byte* ptr, int stride, int bpp, Color c, Vector2F v1, Vector2F v2, Vector2F v3, Vector2F v4, int steps)
            {
                double dx1 = v2.X - v1.X;
                double dy1 = v2.Y - v1.Y;
                double dx2 = v3.X - v2.X;
                double dy2 = v3.Y - v2.Y;
                double dx3 = v4.X - v3.X;
                double dy3 = v4.Y - v3.Y;

                double subdiv_step = 1.0f / (steps + 1);
                double subdiv_step2 = subdiv_step * subdiv_step;
                double subdiv_step3 = subdiv_step2 * subdiv_step;

                double pre1 = 3.0f * subdiv_step;
                double pre2 = 3.0f * subdiv_step2;
                double pre4 = 6.0f * subdiv_step2;
                double pre5 = 6.0f * subdiv_step3;

                double tmp1x = v1.X - v2.X * 2.0f + v3.X;
                double tmp1y = v1.Y - v2.Y * 2.0f + v3.Y;

                double tmp2x = (v2.X - v3.X) * 3.0f - v1.X + v4.X;
                double tmp2y = (v2.Y - v3.Y) * 3.0f - v1.Y + v4.Y;

                double fx = v1.X;
                double fy = v1.Y;

                double dfx = (v2.X - v1.X) * pre1 + tmp1x * pre2 + tmp2x * subdiv_step3;
                double dfy = (v2.Y - v1.Y) * pre1 + tmp1y * pre2 + tmp2y * subdiv_step3;

                double ddfx = tmp1x * pre4 + tmp2x * pre5;
                double ddfy = tmp1y * pre4 + tmp2y * pre5;

                double dddfx = tmp2x * pre5;
                double dddfy = tmp2y * pre5;

                int step = steps;

                while (step-- >= 0)
                {
                    fx += dfx;
                    fy += dfy;
                    dfx += ddfx;
                    dfy += ddfy;
                    ddfx += dddfx;
                    ddfy += dddfy;
                    Pixel.SetPixel(ref ptr, stride, bpp, c, (int)fx, (int)fy);
                }

                Pixel.SetPixel(ref ptr, stride, bpp, c, (int)v4.X, (int)v4.Y);
            }

            public static float Stroke = -1.0f;
            public static List<Vector2F> pointDebug;
            public static unsafe void DrawAdaptiveStep(ref byte* ptr, int stride, int bpp, Color c, Vector2F v1, Vector2F v2, Vector2F v3, Vector2F v4)
            {
                List<Vector2F> points = new List<Vector2F>();
                points.Add(v1);
                DrawAdaptiveRecursive(ref points, v1.X, v1.Y, v2.X, v2.Y, v3.X, v3.Y, v4.X, v4.Y, 0);
                points.Add(v4);

                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (Stroke == -1)
                    {
                        Line.Draw(ref ptr, stride, bpp, c, points[i], points[i + 1]);
                        continue;
                    }

                    float slope = GeometryUtility.GetSlope(points[i], points[i + 1]);

                    // TODO: Finish
                }

                pointDebug = points;
            }

            private static double m_dist = 0.25;
            private static double m_angle_tol = (1.0 / 18.0) * Math.PI;
            private static double m_cusp = (1.0 / 18.0) * Math.PI;
            private const double c_dist_eps = 1e-30;
            private const double c_collin_eps = 1e-30;
            private const double c_angle_tol_eps = 0.01;
            private const int c_recurs_limit = 32;
            private static void DrawAdaptiveRecursive(ref List<Vector2F> points, 
                double x1, double y1,
                double x2, double y2,
                double x3, double y3,
                double x4, double y4,
                int level)
            {
                double x12 = (x1 + x2) / 2;
                double y12 = (y1 + y2) / 2;
                double x23 = (x2 + x3) / 2;
                double y23 = (y2 + y3) / 2;
                double x34 = (x3 + x4) / 2;
                double y34 = (y3 + y4) / 2;

                double x123 = (x12 + x23) / 2;
                double y123 = (y12 + y23) / 2;
                double x234 = (x23 + x34) / 2;
                double y234 = (y23 + y34) / 2;

                double x1234 = (x123 + x234) / 2;
                double y1234 = (y123 + y234) / 2;

                if (level > 0)
                {
                    double dx = x4 - x1;
                    double dy = y4 - y1;

                    double d2 = Math.Abs((x2 - x4) * dy - (y2 - y4) * dx);
                    double d3 = Math.Abs((x3 - x4) * dy - (y3 - y4) * dx);

                    double da1, da2;

                    if (d2 > c_collin_eps && d3 > c_collin_eps)
                    {
                        if ((d2 + d3) * (d2 + d3) <= m_dist * (dx * dx + dy * dy))
                        {
                            if (m_angle_tol < c_angle_tol_eps)
                            {
                                points.Add(new Vector2F(x1234, y1234));
                                return;
                            }

                            double a23 = Math.Atan2(y3 - y2, x3 - x2);
                            da1 = Math.Abs(a23 - Math.Atan2(y2 - y1, x2 - x1));
                            da2 = Math.Abs(Math.Atan2(y4 - y3, x4 - x3) - a23);
                            if (da1 >= Math.PI) da1 = 2 * Math.PI - da1;
                            if (da2 >= Math.PI) da2 = 2 * Math.PI - da2;

                            if (da1 + da2 < m_angle_tol)
                            {
                                points.Add(new Vector2F(x1234, y1234));
                                return;
                            }

                            if (m_cusp != 0.0)
                            {
                                if (da1 > m_cusp)
                                {
                                    points.Add(new Vector2F(x2, y2));
                                    return;
                                }

                                if (da2 > m_cusp)
                                {
                                    points.Add(new Vector2F(x3, y3));
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(d2 > c_collin_eps)
                        {
                            // p1,p3,p4 are collinear, p2 is considerable
                            //----------------------
                            if(d2 * d2 <= m_dist * (dx*dx + dy*dy))
                            {
                                if(m_angle_tol < c_angle_tol_eps)
                                {
                                    points.Add(new Vector2F(x1234, y1234));
                                        return;
                                }

                                // Angle Condition
                                //----------------------
                                da1 = Math.Abs(Math.Atan2(y3 - y2, x3 - x2) - Math.Atan2(y2 - y1, x2 - x1));
                                if(da1 >= Math.PI) da1 = 2 * Math.PI - da1;

                                if(da1 < m_angle_tol)
                                {
                                    points.Add(new Vector2F(x2, y2));
                                    points.Add(new Vector2F(x3, y3));
                                    return;
                                }

                                if(m_cusp != 0.0)
                                {
                                    if(da1 > m_cusp)
                                    {
                                        points.Add(new Vector2F(x2, y2));
                                        return;
                                    }
                                }
                            }
                        }
                        else if(d3 > c_collin_eps)
                        {
                            // p1,p2,p4 are collinear, p3 is considerable
                            //----------------------
                            if(d3 * d3 <= m_dist * (dx*dx + dy*dy))
                            {
                                if(m_angle_tol < c_angle_tol_eps)
                                {
                                    points.Add(new Vector2F(x1234, y1234));
                                    return;
                                }

                                // Angle Condition
                                //----------------------
                                da1 = Math.Abs(Math.Atan2(y4 - y3, x4 - x3) - Math.Atan2(y3 - y2, x3 - x2));
                                if (da1 >= Math.PI) da1 = 2 * Math.PI - da1;

                                if(da1 < m_angle_tol)
                                {
                                    points.Add(new Vector2F(x2, y2));
                                    points.Add(new Vector2F(x3, y3));
                                    return;
                                }

                                if(m_cusp != 0.0)
                                {
                                    if(da1 > m_cusp)
                                    {
                                        points.Add(new Vector2F(x3, y3));
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Collinear case
                            //-----------------
                            dx = x1234 - (x1 + x4) / 2;
                            dy = y1234 - (y1 + y4) / 2;
                            if(dx*dx + dy*dy <= m_dist)
                            {
                                points.Add(new Vector2F(x1234, y1234));
                                        return;
                            }
                        }
                    }
                }

                DrawAdaptiveRecursive(ref points, x1, y1, x12, y12, x123, y123, x1234, y1234, level + 1);
                DrawAdaptiveRecursive(ref points, x1234, y1234, x234, y234, x34, y34, x4, y4, level + 1);
            }
        }

        public class Fill
        {
            public static unsafe void SimpleFill(ref byte* ptr, int stride, int bpp, Color src, Color dest, Vector2 start)
            {
                int s = src.ToArgb();

                Stack<Vector2> stack = new Stack<Vector2>();
                stack.Push(start);

                while (stack.Count > 0)
                {
                    Vector2 pos = stack.Pop();
                    Pixel.SetPixel(ref ptr, stride, bpp, dest, pos);

                    int test = Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y - 1);

                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y - 1) == s) stack.Push(new Vector2(pos.X, pos.Y - 1));
                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X - 1, pos.Y) == s) stack.Push(new Vector2(pos.X - 1, pos.Y));
                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y + 1) == s) stack.Push(new Vector2(pos.X, pos.Y + 1));
                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X + 1, pos.Y) == s) stack.Push(new Vector2(pos.X + 1, pos.Y));
                }
            }

            public static unsafe void ScanlineFill(ref byte* ptr, int stride, int bpp, Color src, Color dest, Vector2 start)
            {
                int s = src.ToArgb();
                int d = dest.ToArgb();

                Stack<Vector2> stack = new Stack<Vector2>();
                stack.Push(start);

                while (stack.Count > 0)
                {
                    Vector2 pos = stack.Pop();

                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y) == d) continue;

                    Pixel.SetPixel(ref ptr, stride, bpp, dest, pos);

                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X - 1, pos.Y) == s)
                    {
                        stack.Push(new Vector2(pos.X - 1, pos.Y));
                    }

                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X + 1, pos.Y) == s)
                    {
                        stack.Push(new Vector2(pos.X + 1, pos.Y));
                    }

                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y - 1) == s) stack.Push(new Vector2(pos.X, pos.Y - 1));
                    if (Pixel.GetPixelInt(ref ptr, stride, bpp, pos.X, pos.Y + 1) == s) stack.Push(new Vector2(pos.X, pos.Y + 1));
                }
            }
        }
    }
}

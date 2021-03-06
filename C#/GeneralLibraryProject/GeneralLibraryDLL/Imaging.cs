﻿using org.general.Units;
using org.general.Units.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace org.general
{
    public class Imaging
    {
        public static class Transforms
        {
            public enum ResizeMethod
            {
                NEAREST_NEIGHBOR,
                Berp,
                BICUBIC
            }
            public static Bitmap ResizeImage(Bitmap bmp, float sx, float sy, ResizeMethod method)
            { 
                return ResizeImage(bmp, (int)(sx * bmp.Width), (int)(sy * bmp.Height), method); 
            }
            public static unsafe Bitmap ResizeImage(Bitmap bmp, int width, int height, ResizeMethod method)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                Bitmap ret = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                float dx = (float)bmp.Width / (float)width;
                float dy = (float)bmp.Height / (float)height;

                BitmapData s_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                BitmapData d_data = ret.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, ret.PixelFormat);

                int s_stride = s_data.Stride;
                int d_stride = d_data.Stride;

                byte* s_ptr = (byte*)s_data.Scan0;
                byte* d_ptr = (byte*)d_data.Scan0;

                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int d_idx = (y * d_stride) + (x * bpp);

                        byte[] values = new byte[4];
                        switch (method)
                        {
                            case ResizeMethod.NEAREST_NEIGHBOR:
                                values = CalculateNearestNeighbor(ref s_ptr, ref d_ptr, s_stride, d_stride, bpp, x, y, dx, dy);
                                break;
                            case ResizeMethod.Berp:
                                values = CalculateBerp(ref s_ptr, ref d_ptr, s_stride, d_stride, bpp, x, y, bmp.Width, bmp.Height, width, height);
                                break;
                            case ResizeMethod.BICUBIC:
                                values = CalculateBicubic(ref s_ptr, ref d_ptr, s_stride, d_stride, bpp, x, y, bmp.Width, bmp.Height, width, height);
                                break;
                        }

                        d_ptr[d_idx] = values[0];
                        d_ptr[d_idx + 1] = values[1];
                        d_ptr[d_idx + 2] = values[2];
                        d_ptr[d_idx + 3] = values[3];
                    }
                }

                    return ret;
            }
            private static unsafe byte[] CalculateNearestNeighbor(ref byte* s_ptr, ref byte* d_ptr, int s_stride, int d_stride, int bpp, int x, int y, float dx, float dy)
            {
                byte b, g, r, a;
                int sx = (int)Math.Floor(dx * x);
                int sy = (int)Math.Floor(dy * y);

                int s_idx = (sy * s_stride) + (sx * bpp);

                b = s_ptr[s_idx];
                g = s_ptr[s_idx + 1];
                r = s_ptr[s_idx + 2];
                a = s_ptr[s_idx + 3];

                return new byte[] { b, g, r, a };
            }
            private static unsafe byte[] CalculateBerp(ref byte* s_ptr, ref byte* d_ptr, int s_stride, int d_stride, int bpp, int x, int y, int s_width, int s_height, int d_width, int d_height)
            {
                byte b, g, r, a;

                float tx = (float)x / d_width;
                float ty = (float)y / d_height;

                float xMap = MathF.Lerp(0, s_width - 1, tx);
                float yMap = MathF.Lerp(0, s_height - 1, ty);

                int x1 = (int)xMap;
                int x2 = (int)xMap + 1;
                int y1 = (int)yMap;
                int y2 = (int)yMap + 1;

                if (x1 < 0)
                    x1 = x2;
                if (y1 < 0)
                    y1 = y2;
                if (x2 >= s_width)
                    x2 = x1;
                if (y2 >= s_height)
                    y2 = y1;

                int idx00 = (y1 * s_stride) + (x1 * bpp);
                int idx01 = (y1 * s_stride) + (x2 * bpp);
                int idx10 = (y2 * s_stride) + (x1 * bpp);
                int idx11 = (y2 * s_stride) + (x2 * bpp);

                b = (byte)MathF.Berp(
                    (float)s_ptr[idx00], 
                    (float)s_ptr[idx01], 
                    (float)s_ptr[idx10], 
                    (float)s_ptr[idx11], 
                    tx, ty);
                g = (byte)MathF.Berp(
                    (float)s_ptr[idx00 + 1],
                    (float)s_ptr[idx01 + 1],
                    (float)s_ptr[idx10 + 1],
                    (float)s_ptr[idx11 + 1],
                    tx, ty);
                r = (byte)MathF.Berp(
                    (float)s_ptr[idx00 + 2],
                    (float)s_ptr[idx01 + 2],
                    (float)s_ptr[idx10 + 2],
                    (float)s_ptr[idx11 + 2],
                    tx, ty);
                a = (byte)MathF.Berp(
                    (float)s_ptr[idx00 + 3],
                    (float)s_ptr[idx01 + 3],
                    (float)s_ptr[idx10 + 3],
                    (float)s_ptr[idx11 + 3],
                    tx, ty);

                return new byte[] { b, g, r, a };
            }
            private static unsafe byte[] CalculateBicubic(ref byte* s_ptr, ref byte* d_ptr, int s_stride, int d_stride, int bpp, int x, int y, int s_width, int s_height, int d_width, int d_height)
            {
                float b, g, r, a;

                float tx = (float)x / d_width;
                float ty = (float)y / d_height;

                float xMap = MathF.Lerp(0, s_width - 1, tx);
                float yMap = MathF.Lerp(0, s_height - 1, ty);

                int x1 = (int)xMap - 1;
                int y1 = (int)yMap - 1;

                int[] idxArr = new int[16];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int xi, yi;

                        xi = x1 + i;
                        while (xi < 0)
                            xi++;
                        while (xi >= s_width)
                            xi--;

                        yi = y1 + j;
                        while (yi < 0)
                            yi++;
                        while (yi >= s_height)
                            yi--; 

                        idxArr[j * 4 + i] = (yi * s_stride) + (xi * bpp);
                    }
                }

                float ntx = xMap - (int)xMap;
                float nty = yMap - (int)yMap;

                float[] bArr = new float[16];
                float[] gArr = new float[16];
                float[] rArr = new float[16];
                float[] aArr = new float[16];

                for (int i = 0; i < 16; i++)
                {
                    bArr[i] = s_ptr[idxArr[i]];
                    gArr[i] = s_ptr[idxArr[i] + 1];
                    rArr[i] = s_ptr[idxArr[i] + 2];
                    aArr[i] = s_ptr[idxArr[i] + 3];
                }

                b = MathF.BicubicInterp(bArr, tx, ty);
                g = MathF.BicubicInterp(gArr, tx, ty);
                r = MathF.BicubicInterp(rArr, tx, ty);
                a = MathF.BicubicInterp(aArr, tx, ty);

                /*Bitmap cerpTest = new Bitmap(425, 425, GlobalSettings.DefaultPixelFormat);

                var t_data = cerpTest.LockBits(new Rectangle(0, 0, cerpTest.Width, cerpTest.Height), ImageLockMode.ReadWrite, GlobalSettings.DefaultPixelFormat);
                var t_stride = t_data.Stride;
                var t_ptr = (byte*)t_data.Scan0;

                for (int i = 0; i < 100; x++)
                {
                    int graphX = i + 5;


                    int graphYR = (int)(255f - r) + 5;
                    int graphYG = (int)(255f - g) + 110;
                    int graphYB = (int)(255f - b) + 215;
                    int graphYA = (int)(255f - a) + 320;

                    int y = (int)MathF.Cerp(new float[] { 23f, 0f, 2, 3 }, (float)graphX / 100);

                    int idx = (y * stride) + (x * bpp);

                    ptr[idx] = 0;
                    ptr[idx + 1] = 0;
                    ptr[idx + 2] = 0;
                    ptr[idx + 3] = 255;
                }*/




                return new byte[] { (byte)b, (byte)g, (byte)r, (byte)a };
            }

            public enum RotateMethod
            {
                NEAREST_NEIGHBOR,
                AREA_MAPPING,
                SHEARING
            }
            public static unsafe void RotateImage(Bitmap bmp, float a, RotateMethod method)
            {

            }
        }

        public static class Gradients
        {
            public static unsafe Bitmap DrawTwoPointHorizontalGradient(int width, int height, Color start, Color end)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int x = 0; x < width; x++)
                {
                    byte rValue = (byte)MathF.Lerp((float)start.R, (float)end.R, (float)x / width);
                    byte gValue = (byte)MathF.Lerp((float)start.G, (float)end.G, (float)x / width);
                    byte bValue = (byte)MathF.Lerp((float)start.B, (float)end.B, (float)x / width);

                    for (int y = 0; y < height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        ptr[idx] = bValue;
                        ptr[idx + 1] = gValue;
                        ptr[idx + 2] = rValue;
                        ptr[idx + 3] = 255;
                    }
                }

                bmp.UnlockBits(data);
                return bmp;
            }

            public struct GradientStop
            {
                public float Location;
                public Color Color;

                public GradientStop(float loc, Color c) { Location = loc; Color = c; }
            }
            public static unsafe Bitmap DrawMultiPointHorizontalGradient(int width, int height, params GradientStop[] stops)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                int leftStop = 0;
                int rightStop = 1;

                if (stops.Length == 1)
                {
                    stops = new GradientStop[] { stops[0], stops[0] };
                    stops[1].Location = 1.0f;
                }

                float step = 1 / (float)width;
                float rStep = ((float)stops[rightStop].Color.R - (float)stops[leftStop].Color.R) / (stops[rightStop].Location - stops[leftStop].Location);
                float gStep = ((float)stops[rightStop].Color.G - (float)stops[leftStop].Color.G) / (stops[rightStop].Location - stops[leftStop].Location);
                float bStep = ((float)stops[rightStop].Color.B - (float)stops[leftStop].Color.B) / (stops[rightStop].Location - stops[leftStop].Location);

                for (int x = 0; x < width; x++)
                {
                    float currentLoc = step * x;
                    if (currentLoc > stops[rightStop].Location)
                    {
                        leftStop++;
                        rightStop++;
                        rStep = ((float)stops[rightStop].Color.R - (float)stops[leftStop].Color.R) / (stops[rightStop].Location - stops[leftStop].Location);
                        gStep = ((float)stops[rightStop].Color.G - (float)stops[leftStop].Color.G) / (stops[rightStop].Location - stops[leftStop].Location);
                        bStep = ((float)stops[rightStop].Color.B - (float)stops[leftStop].Color.B) / (stops[rightStop].Location - stops[leftStop].Location);
                    }

                    byte rValue = (byte)(stops[leftStop].Color.R + (currentLoc - stops[leftStop].Location) * rStep);
                    byte gValue = (byte)(stops[leftStop].Color.G + (currentLoc - stops[leftStop].Location) * gStep);
                    byte bValue = (byte)(stops[leftStop].Color.B + (currentLoc - stops[leftStop].Location) * bStep);

                    for (int y = 0; y < height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        ptr[idx] = bValue;
                        ptr[idx + 1] = gValue;
                        ptr[idx + 2] = rValue;
                    }
                }

                bmp.UnlockBits(data);
                return bmp;
            }

            public static unsafe Bitmap DrawThreePointHorizontalGradient(int width, int height, Color start1, Color start2, Color end)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                float rvStep = ((float)start2.R - (float)start1.R) / (float)height;
                float gvStep = ((float)start2.G - (float)start1.G) / (float)height;
                float bvStep = ((float)start2.B - (float)start1.B) / (float)height;

                float[] rvValues = new float[height];
                float[] gvValues = new float[height];
                float[] bvValues = new float[height];

                float[] rvSteps = new float[height];
                float[] gvSteps = new float[height];
                float[] bvSteps = new float[height];

                float slope = ((float)height / 2) / (float)width;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        int range = (int)(slope * x);

                        if (y + range >= height || y - range < 0)
                        {
                            ptr[idx] = 0;
                            ptr[idx + 1] = 0;
                            ptr[idx + 2] = 0;
                            continue;
                        }

                        if (x == 0)
                        {
                            float rValue = start1.R + (rvStep * y);
                            float gValue = start1.G + (gvStep * y);
                            float bValue = start1.B + (bvStep * y);

                            rvValues[y] = rValue;
                            gvValues[y] = gValue;
                            bvValues[y] = bValue;
                            rvSteps[y] = ((float)end.R - rValue) / (float)width;
                            gvSteps[y] = ((float)end.G - gValue) / (float)width;
                            bvSteps[y] = ((float)end.B - bValue) / (float)width;

                            ptr[idx] = (byte)bValue;
                            ptr[idx + 1] = (byte)gValue;
                            ptr[idx + 2] = (byte)rValue;
                        }
                        else
                        {
                            float rValue = rvValues[y] + (rvSteps[y] * x);
                            float gValue = gvValues[y] + (gvSteps[y] * x);
                            float bValue = bvValues[y] + (bvSteps[y] * x);

                            ptr[idx] = (byte)bValue;
                            ptr[idx + 1] = (byte)gValue;
                            ptr[idx + 2] = (byte)rValue;
                        }
                    }
                }

                bmp.UnlockBits(data);
                return bmp;
            }

            public struct GradientPoint
            {
                public Vector2F Location;
                public Color Color;

                public GradientPoint(float x, float y, Color c)
                    : this(new Vector2F(x, y), c) { }
                public GradientPoint(Vector2F loc, Color c)
                {
                    Location = loc;
                    Color = c;
                }
            }
            public static unsafe Bitmap DrawThreePointGradient(int width, int height, GradientPoint a, GradientPoint b, GradientPoint c, bool drawPoints = false)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        // Calculate the distance between the current point and each of the control points
                        float distA = Vector2F.Distance(a.Location, new Vector2F(x, y));
                        float distB = Vector2F.Distance(b.Location, new Vector2F(x, y));
                        float distC = Vector2F.Distance(c.Location, new Vector2F(x, y));

                        // Determine weighting based on distances
                        float weightA = distA == 0 ? 1 : 1 / distA;
                        float weightB = distB == 0 ? 1 : 1 / distB;
                        float weightC = distC == 0 ? 1 : 1 / distC;

                        float totalWeight = weightA + weightB + weightC;

                        // Find the weighted average for each color value
                        float pr = (((float)a.Color.R * weightA) + ((float)b.Color.R * weightB) + ((float)c.Color.R * weightC))
                                    / totalWeight;
                        float pg = (((float)a.Color.G * weightA) + ((float)b.Color.G * weightB) + ((float)c.Color.G * weightC))
                                    / totalWeight;
                        float pb = (((float)a.Color.B * weightA) + ((float)b.Color.B * weightB) + ((float)c.Color.B * weightC))
                                    / totalWeight;

                        ptr[idx] = (byte)pb;
                        ptr[idx + 1] = (byte)pg;
                        ptr[idx + 2] = (byte)pr;
                    }
                }

                bmp.UnlockBits(data);

                if (drawPoints)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.FillEllipse(Brushes.Black, a.Location.X - 2, a.Location.Y - 2, 5, 5);
                        g.FillEllipse(Brushes.Black, b.Location.X - 2, b.Location.Y - 2, 5, 5);
                        g.FillEllipse(Brushes.Black, c.Location.X - 2, c.Location.Y - 2, 5, 5);
                    }
                }

                return bmp;
            }

            public static unsafe Bitmap DrawVariablePointGradient(int width, int height, params GradientPoint[] pts)
            {
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = 3;

                float bmp_cross = Vector2F.Distance(
                    new Vector2F(0, 0), new Vector2F(bmp.Width, bmp.Height));

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        float pr = 0;
                        float pg = 0;
                        float pb = 0;

                        float totalWeight = 0;

                        foreach (var pt in pts)
                        {
                            float dist = Vector2F.DistanceSquared(pt.Location, new Vector2F(x, y));
                            float weight = dist == 0 ? 1 : 1 / dist;

                            pr += (float)pt.Color.R * weight;
                            pg += (float)pt.Color.G * weight;
                            pb += (float)pt.Color.B * weight;
                            totalWeight += weight;
                        }

                        pr /= totalWeight;
                        pg /= totalWeight;
                        pb /= totalWeight;

                        ptr[idx] = (byte)pb;
                        ptr[idx + 1] = (byte)pg;
                        ptr[idx + 2] = (byte)pr;
                    }
                }

                bmp.UnlockBits(data);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (var pt in pts)
                    {
                        g.FillEllipse(Brushes.Black, pt.Location.X - 2, pt.Location.Y - 2, 5, 5);
                    }
                }

                return bmp;
            }

            public static unsafe Bitmap DrawThreePointGradientFlashMethod(int width, int height, GradientPoint a, GradientPoint b, GradientPoint c)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                Color baseColor = Color.FromArgb(127, 127, 127);

                bmp.UnlockBits(data);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.FillEllipse(Brushes.Black, a.Location.X - 2, a.Location.Y - 2, 5, 5);
                    g.FillEllipse(Brushes.Black, b.Location.X - 2, b.Location.Y - 2, 5, 5);
                    g.FillEllipse(Brushes.Black, c.Location.X - 2, c.Location.Y - 2, 5, 5);
                }

                return bmp;
            }
            //private static 

            public static unsafe Bitmap DrawFourCornerGradient(int width, int height, Color c00, Color c01, Color c10, Color c11)
            {
                Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        float tx = (float)x / width;
                        float ty = (float)y / height;

                        byte rVal = (byte)MathF.Berp(
                            (float)c00.R,
                            (float)c01.R,
                            (float)c10.R,
                            (float)c11.R,
                            tx, ty);
                        byte gVal = (byte)MathF.Berp(
                            (float)c00.G,
                            (float)c01.G,
                            (float)c10.G,
                            (float)c11.G,
                            tx, ty);
                        byte bVal = (byte)MathF.Berp(
                            (float)c00.B,
                            (float)c01.B,
                            (float)c10.B,
                            (float)c11.B,
                            tx, ty);

                        ptr[idx] = bVal;
                        ptr[idx + 1] = gVal;
                        ptr[idx + 2] = rVal;
                        ptr[idx + 3] = 255;
                    }
                }

                    bmp.UnlockBits(data);

                return bmp;
            }

        }

        public static class Presets
        {
            public static Bitmap DrawRGBHorizontalGradient(int width, int height)
            {
                return Gradients.DrawMultiPointHorizontalGradient(
                            width, height,
                            new Gradients.GradientStop(0.0f, Color.FromArgb(255, 0, 0)),
                            new Gradients.GradientStop(0.166f, Color.FromArgb(255, 255, 0)),
                            new Gradients.GradientStop(0.333f, Color.FromArgb(0, 255, 0)),
                            new Gradients.GradientStop(0.5f, Color.FromArgb(0, 255, 255)),
                            new Gradients.GradientStop(0.666f, Color.FromArgb(0, 0, 255)),
                            new Gradients.GradientStop(0.833f, Color.FromArgb(255, 0, 255)),
                            new Gradients.GradientStop(1.0f, Color.FromArgb(255, 0, 0)));
            }
        }

        public static class ImageFilters
        {
            public enum GrayscaleMethod
            {
                LIGHTNESS,
                AVERAGE,
                LUMINOSITY
            }
            public static Bitmap GrayscaleFilter(Bitmap bmp, GrayscaleMethod method = GrayscaleMethod.LUMINOSITY)
            {
                Bitmap ret = new Bitmap(bmp);
                GrayscaleFilter(ref ret, method);
                return ret;
            }
            public static unsafe void GrayscaleFilter(ref Bitmap bmp, GrayscaleMethod method = GrayscaleMethod.LUMINOSITY)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);
                        byte value = ConvertColorToGrayscale(ptr[idx + 2], ptr[idx + 1], ptr[idx], method);
                        ptr[idx] = value;
                        ptr[idx + 1] = value;
                        ptr[idx + 2] = value;
                    }
                }

                bmp.UnlockBits(data);
            }
            private static byte ConvertColorToGrayscale(byte r, byte g, byte b, GrayscaleMethod method)
            {
                switch (method)
                {
                    case GrayscaleMethod.LIGHTNESS:
                        return (byte)(((int)Math.Min(Math.Min(r, g), b) + (int)Math.Max(Math.Max(r, g), b)) / 2);
                    case GrayscaleMethod.AVERAGE:
                        return (byte)(((int)r + (int)g + (int)b) / 3);
                    case GrayscaleMethod.LUMINOSITY:
                        return (byte)(((float)r * 0.21f) + ((float)g * 0.71f) + ((float)b * 0.07f));
                    default:
                        return 0;
                }
            }

            public static Bitmap ThresholdFilter(Bitmap bmp, byte bottom, byte top)
            {
                Bitmap ret = new Bitmap(bmp);
                ThresholdFilter(ref ret, bottom, top);
                return ret;
            }
            public static unsafe void ThresholdFilter(ref Bitmap bmp, byte bottom, byte top)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);
                        ptr[idx] = ptr[idx] > top ? (byte)255 :
                            (ptr[idx] < bottom ? (byte)0 : ptr[idx]);

                        ptr[idx + 1] = ptr[idx + 1] > top ? (byte)255 :
                            (ptr[idx + 1] < bottom ? (byte)0 : ptr[idx + 1]);

                        ptr[idx + 2] = ptr[idx + 2] > top ? (byte)255 :
                            (ptr[idx + 2] < bottom ? (byte)0 : ptr[idx + 2]);
                    }
                }

                bmp.UnlockBits(data);
            }

            public static Bitmap ThresholdAboveFilter(Bitmap bmp, int threshold)
            {
                Bitmap ret = new Bitmap(bmp);
                ThresholdAboveFilter(ref ret, threshold);
                return ret;
            }
            public static unsafe void ThresholdAboveFilter(ref Bitmap bmp, int threshold)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);
                        ptr[idx] = ptr[idx] > threshold ? (byte)255 : ptr[idx];
                        ptr[idx + 1] = ptr[idx + 1] > threshold ? (byte)255 : ptr[idx + 1];
                        ptr[idx + 2] = ptr[idx + 2] > threshold ? (byte)255 : ptr[idx + 2];
                    }
                }

                bmp.UnlockBits(data);
            }

            public static Bitmap ThresholdBelowFilter(Bitmap bmp, int threshold)
            {
                Bitmap ret = new Bitmap(bmp);
                ThresholdBelowFilter(ref ret, threshold);
                return ret;
            }
            public static unsafe void ThresholdBelowFilter(ref Bitmap bmp, int threshold)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);
                        ptr[idx] = ptr[idx] < threshold ? (byte)0 : ptr[idx];
                        ptr[idx + 1] = ptr[idx + 1] < threshold ? (byte)0 : ptr[idx + 1];
                        ptr[idx + 2] = ptr[idx + 2] < threshold ? (byte)0 : ptr[idx + 2];
                    }
                }

                bmp.UnlockBits(data);
            }

            public static Bitmap ThresholdStepFilter(Bitmap bmp, int threshold)
            {
                Bitmap ret = new Bitmap(bmp);
                ThresholdStepFilter(ref ret, threshold);
                return ret;
            }
            public static unsafe void ThresholdStepFilter(ref Bitmap bmp, int threshold)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                float multiple = 255f / (float)threshold;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);
                        ptr[idx] = CalculateThresholdValue(ptr[idx], threshold, multiple);
                        ptr[idx + 1] = CalculateThresholdValue(ptr[idx + 1], threshold, multiple);
                        ptr[idx + 2] = CalculateThresholdValue(ptr[idx + 2], threshold, multiple);
                    }
                }

                bmp.UnlockBits(data);
            }
            private static byte CalculateThresholdValue(byte c, int threshold, float multiple)
            {
                return (byte)(Math.Round((float)c / multiple) * multiple);
            }

            public static Bitmap MeanFilter(Bitmap bmp)
            {
                Bitmap ret = new Bitmap(bmp);
                MeanFilter(ref ret);
                return ret;
            }
            public static unsafe void MeanFilter(ref Bitmap bmp)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                Bitmap buf = new Bitmap(bmp);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                BitmapData b_data = buf.LockBits(new Rectangle(0, 0, buf.Width, buf.Height), ImageLockMode.ReadWrite, buf.PixelFormat);
                byte* b_ptr = (byte*)data.Scan0;
                int b_stride = data.Stride;

                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        int count = 0;
                        int avgR = 0;
                        int avgG = 0;
                        int avgB = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            if (x + i < 0 || x + i >= bmp.Width)
                                continue;

                            for (int j = -1; j <= 1; j++)
                            {
                                if (y + j < 0 || y + j >= bmp.Height)
                                    continue;

                                int b_idx = ((y + j) * stride) + ((x + i) * bpp);

                                avgB += b_ptr[b_idx];
                                avgG += b_ptr[b_idx + 1];
                                avgR += b_ptr[b_idx + 2];
                                count++;
                            }
                        }

                        ptr[idx] = (byte)(avgB / count);
                        ptr[idx + 1] = (byte)(avgG / count);
                        ptr[idx + 2] = (byte)(avgR / count);
                    }
                }

                buf.UnlockBits(b_data);
                bmp.UnlockBits(data);

                buf.Dispose();
            }

            public static Bitmap WeightedMeanFilter(Bitmap bmp)
            {
                Bitmap ret = new Bitmap(bmp);
                WeightedMeanFilter(ref ret);
                return ret;
            }
            public static unsafe void WeightedMeanFilter(ref Bitmap bmp, float[] weight = null)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                Bitmap buf = new Bitmap(bmp);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                BitmapData b_data = buf.LockBits(new Rectangle(0, 0, buf.Width, buf.Height), ImageLockMode.ReadWrite, buf.PixelFormat);
                byte* b_ptr = (byte*)data.Scan0;
                int b_stride = data.Stride;

                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        int[] rValues = new int[9];
                        int[] gValues = new int[9];
                        int[] bValues = new int[9];

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (x + i < 0 || x + i >= bmp.Width
                                    || y + j < 0 || y + j >= bmp.Height)
                                {
                                    rValues[(j + 1) * 3 + (i + 1)] = -1;
                                    gValues[(j + 1) * 3 + (i + 1)] = -1;
                                    bValues[(j + 1) * 3 + (i + 1)] = -1;
                                    continue;
                                }

                                int b_idx = ((y + j) * stride) + ((x + i) * bpp);

                                rValues[(j + 1) * 3 + (i + 1)] = b_ptr[b_idx + 2];
                                gValues[(j + 1) * 3 + (i + 1)] = b_ptr[b_idx + 1];
                                bValues[(j + 1) * 3 + (i + 1)] = b_ptr[b_idx];
                            }
                        }

                        if (weight == null)
                            weight = new float[] {
                                0.1f, 0.25f, 0.1f,
                                0.25f, 1.0f, 0.25f,
                                0.1f, 0.25f, 0.1f
                            };

                        float rT = 0;
                        float gT = 0;
                        float bT = 0;

                        float div = 0;

                        for (int i = 0; i < 9; i++) 
                        {
                            if (rValues[i] == -1)
                                continue;

                            rT += rValues[i] * weight[i];
                            gT += gValues[i] * weight[i];
                            bT += bValues[i] * weight[i];

                            div += weight[i];
                        }

                        rT /= div;
                        gT /= div;
                        bT /= div;

                        ptr[idx] = (byte)(bT.ToInt());
                        ptr[idx + 1] = (byte)(gT.ToInt());
                        ptr[idx + 2] = (byte)(rT.ToInt());
                    }
                }

                buf.UnlockBits(b_data);
                bmp.UnlockBits(data);

                buf.Dispose();
            }

            public static Bitmap MedianFilter(Bitmap bmp)
            {
                Bitmap ret = new Bitmap(bmp);
                MedianFilter(ref ret);
                return ret;
            }
            public static unsafe void MedianFilter(ref Bitmap bmp)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                Bitmap buf = new Bitmap(bmp);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                BitmapData b_data = buf.LockBits(new Rectangle(0, 0, buf.Width, buf.Height), ImageLockMode.ReadWrite, buf.PixelFormat);
                byte* b_ptr = (byte*)data.Scan0;
                int b_stride = data.Stride;

                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        short[] rVals = new short[9];
                        short[] gVals = new short[9];
                        short[] bVals = new short[9];

                        ArrayUtility.InitializeArrayWithValue<short>(ref rVals, -1);
                        ArrayUtility.InitializeArrayWithValue<short>(ref gVals, -1);
                        ArrayUtility.InitializeArrayWithValue<short>(ref bVals, -1);

                        int arrIdx = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            if (x + i < 0 || x + i >= bmp.Width)
                                continue;

                            for (int j = -1; j <= 1; j++)
                            {
                                if (y + j < 0 || y + j >= bmp.Height)
                                    continue;

                                int b_idx = ((y + j) * stride) + ((x + i) * bpp);

                                bVals[arrIdx] = b_ptr[b_idx];
                                gVals[arrIdx] = b_ptr[b_idx + 1];
                                rVals[arrIdx] = b_ptr[b_idx + 2];
                                arrIdx++;
                            }
                        }

                        ArrayUtility.TrimArray<short>(ref rVals, -1);
                        ArrayUtility.SortArray<short>(ref rVals);

                        ArrayUtility.TrimArray<short>(ref gVals, -1);
                        ArrayUtility.SortArray<short>(ref gVals);

                        ArrayUtility.TrimArray<short>(ref bVals, -1);
                        ArrayUtility.SortArray<short>(ref bVals);

                        ptr[idx] = (byte)(bVals[bVals.Length / 2]);
                        ptr[idx + 1] = (byte)(gVals[gVals.Length / 2]);
                        ptr[idx + 2] = (byte)(rVals[rVals.Length / 2]);
                    }
                }

                buf.UnlockBits(b_data);
                bmp.UnlockBits(data);

                buf.Dispose();
            }

            public static Bitmap InvertFilter(Bitmap bmp)
            {
                Bitmap ret = new Bitmap(bmp);
                InvertFilter(ref ret);
                return ret;
            }
            public static unsafe void InvertFilter(ref Bitmap bmp)
            {
                if (bmp.PixelFormat != GlobalSettings.DefaultPixelFormat)
                    throw new NotSupportedException("Only the PixelFormat defined in GlobalSettings is supported.");

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;
                int bpp = GlobalSettings.DefaultPixelFormatBpp;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int idx = (y * stride) + (x * bpp);

                        ptr[idx] = (byte)(255 - ptr[idx]);
                        ptr[idx + 1] = (byte)(255 - ptr[idx + 1]);
                        ptr[idx + 2] = (byte)(255 - ptr[idx + 1]);
                    }
                }

                bmp.UnlockBits(data);
            }
            
        
        }

        public static class Distortions
        {
            public static unsafe GBitmap Zoom(GBitmap bmp, float zoomFactorX, float zoomFactorY)
            {
                GBitmap buffer = new GBitmap(bmp.Width, bmp.Height);

                float cx = buffer.Width / 2f;
                float cy = buffer.Height / 2f;

                for (int y = 0; y < buffer.Height; y++)
                {
                    for (int x = 0; x < buffer.Width; x++)
                    {
                        float rx = (cx - x) / cx;
                        float ry = (cy - y) / cy;
                        float sx = x + zoomFactorX * zoomFactorX * rx;
                        float sy = y + zoomFactorY * zoomFactorY * ry;

                        int x0 = (int)MathF.Clamp(0, bmp.Width - 1, (int)sx);
                        int x1 = (int)MathF.Clamp(0, bmp.Width - 1, (int)(sx + 0.5f));
                        int y0 = (int)MathF.Clamp(0, bmp.Height - 1, (int)sy);
                        int y1 = (int)MathF.Clamp(0, bmp.Height - 1, (int)(sy + 0.5f));

                        float t1 = sx - x0;
                        float t2 = sy - y0;

                        buffer.SetPixel(x, y, GColor.Berp(bmp.GetPixel(x0, y0), bmp.GetPixel(x1, y0), bmp.GetPixel(x0, y1), bmp.GetPixel(x1, y1), t1, t2));
                    }
                }

                return buffer;
            }
        }

    }
}

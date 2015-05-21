using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Graphics
{
    public class GBitmap : ICloneable
    {
        private int[] _p;
        private int _w;
        private int _h;

        public StringBuilder Debug = new StringBuilder();

        public int Width
        {
            get { return _w; }
            private set { this._w = value; }
        }

        public int Height
        {
            get { return _h; }
            private set { this._h = value; }
        }

        public GBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            _p = new int[Height * Width];
        }
        public GBitmap(System.Drawing.Bitmap bmp)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            _p = new int[Height * Width];

            System.Drawing.Bitmap buf = (System.Drawing.Bitmap)bmp.Clone();

            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb) 
            {
                buf = ImageUtility.ConvertPixelFormat(buf, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }

            unsafe
            {
                System.Drawing.Imaging.BitmapData data = buf.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = 4;

                for (int y = 0; y < Height; y++)
                {
                    int row = y * stride;

                    for (int x = 0; x < Width; x++)
                    {
                        int idx = row + x * bpp;
                        int value = (ptr[idx + 3] << 24) + (ptr[idx + 2] << 16) + (ptr[idx + 1] << 8) + ptr[idx];
                        SetPixel(x, y, value);
                    }
                }

                buf.UnlockBits(data);
            }

            buf.Dispose();
        }

        public GColor GetPixel(int x, int y)
        {
            return new GColor(_p[y * _w + x]);
        }
        public GColor GetPixel(Vector2 p)
        {
            return GetPixel(p.X, p.Y);
        }

        public int GetPixelInt(int x, int y)
        {
            return _p[y * _w + x];
        }
        public int GetPixelInt(Vector2 p)
        {
            return GetPixelInt(p.X, p.Y);
        }

        public void SetPixel(int x, int y, GColor c)
        {
            _p[y * _w + x] = c.IntValue;
        }
        public void SetPixel(Vector2 p, GColor c)
        {
            SetPixel(p.X, p.Y, c);
        }

        public void SetPixel(int x, int y, int v)
        {
            _p[y * _w + x] = v;
        }
        public void SetPixel(Vector2 p, int v)
        {
            SetPixel(p.X, p.Y, v);
        }

        public System.Drawing.Bitmap ToSystemBitmap()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
                int stride = data.Stride;
                byte* ptr = (byte*)data.Scan0;
                int bpp = 4;

                for (int y = 0; y < Height; y++)
                {
                    int row = y * stride;

                    for (int x = 0; x < Width; x++)
                    {
                        int idx = row + x * bpp;
                        GColor c = GetPixel(x, y);
                        ptr[idx] = c.B;
                        ptr[idx + 1] = c.G;
                        ptr[idx + 2] = c.R;
                        ptr[idx + 3] = c.A;
                    }
                }

                bmp.UnlockBits(data);
            }

            return bmp;
        }

        public object Clone()
        {
            GBitmap bmp = new GBitmap(_w, _h);
            bmp._p = ArrayUtility.DeepCopy<int>(ref _p);
            return bmp;
        }

        public void Save(string p)
        {
            using (System.Drawing.Bitmap bmp = ToSystemBitmap())
            {
                bmp.Save(p);
            }
        }
    }
}

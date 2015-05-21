using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Graphics
{
    public class GBitmapF : ICloneable
    {
        private GColorF[] _p;
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

        public GBitmapF(int width, int height)
        {
            Width = width;
            Height = height;
            _p = new GColorF[Height * Width];
            ArrayUtility.InitializeArrayWithValue<GColorF>(ref _p, new GColorF());
        }
        public GBitmapF(System.Drawing.Bitmap bmp)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            _p = new GColorF[Height * Width];

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
                        _p[y * _w + x] = new GColorF(ptr[idx + 2] / 255.0f, ptr[idx + 1] / 255.0f, ptr[idx] / 255.0f, ptr[idx + 3] / 255.0f);
                    }
                }

                buf.UnlockBits(data);
            }

            buf.Dispose();
        }

        public GColorF GetPixel(int x, int y)
        {
            return _p[y * _w + x];
        }
        public GColorF GetPixel(Vector2 p)
        {
            return GetPixel(p.X, p.Y);
        }

        public void SetPixel(int x, int y, GColorF c)
        {
            _p[y * _w + x] = c;
        }
        public void SetPixel(Vector2 p, GColorF c)
        {
            SetPixel(p.X, p.Y, c);
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
                        GColor c = (GColor)GetPixel(x, y);
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
            GBitmapF bmp = new GBitmapF(_w, _h);
            bmp._p = ArrayUtility.DeepCopy<GColorF>(ref _p);
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

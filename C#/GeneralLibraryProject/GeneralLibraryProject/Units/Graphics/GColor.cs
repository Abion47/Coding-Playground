using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Graphics
{
    public class GColor
    {
        private int _c;

        public byte A
        {
            get { return (byte)((_c >> 24) & 0xFF); }
            set { _c = _c & 0x00FFFFFF | (((int)value) << 24); }
        }

        public byte R
        {
            get { return (byte)((_c >> 16) & 0xFF); }
            set { _c = _c & unchecked((int)0xFF00FFFF) | (((int)value) << 16); }
        }

        public byte G
        {
            get { return (byte)((_c >> 8) & 0xFF); }
            set { _c = _c & unchecked((int)0xFFFF00FF) | (((int)value) << 8); }
        }

        public byte B
        {
            get { return (byte)(_c & 0xFF); }
            set { _c = _c & unchecked((int)0xFFFFFF00) | (int)value; }
        }

        public int IntValue
        {
            get { return this._c; }
            set { this._c = value; }
        }

        public GColor() : this(0, 0, 0, 255) { }
        public GColor(int r, int g, int b) : this(r, g, b, 255) { }
        public GColor(int r, int g, int b, int a) 
        {
            R = (byte)(r & 0xFF);
            G = (byte)(g & 0xFF);
            B = (byte)(b & 0xFF);
            A = (byte)(a & 0xFF);
        }
        public GColor(byte r, byte g, byte b) : this(r, g, b, (byte)255) { }
        public GColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public GColor(int i)
        {
            _c = i;
        }

        public static explicit operator GColorF(GColor c)
        {
            return new GColorF(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f);
        }

        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(_c);
        }

        public override string ToString()
        {
            return "{ A: " + A + ", R: " + R + " G: " + G + ", B: " + B + " }";
        }

        #region Interpolation Functions
        public static GColor Lerp(GColor a, GColor b, float t)
        {
            return new GColor((int)MathF.Lerp(a.R, b.R, t), (int)MathF.Lerp(a.G, b.G, t), (int)MathF.Lerp(a.B, b.B, t), (int)MathF.Lerp(a.A, b.A, t));
        }

        public static unsafe GColor BilinearInterpolation(ref GBitmap bmp, int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3, float t1, float t2)
        {
            GColor nw = bmp.GetPixel(x0, y0);
            GColor ne = bmp.GetPixel(x1, y1);
            GColor sw = bmp.GetPixel(x2, y2);
            GColor se = bmp.GetPixel(x3, y3);

            GColor m1 = GColor.Lerp(nw, ne, t1);
            GColor m2 = GColor.Lerp(sw, se, t1);

            return GColor.Lerp(m1, m2, t2);
        }
        #endregion
    }
}

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
            return LinearInterp(a, b, t);
        }
        public static GColor LinearInterp(GColor a, GColor b, float t)
        {
            return new GColor((int)MathF.Lerp(a.R, b.R, t), (int)MathF.Lerp(a.G, b.G, t), (int)MathF.Lerp(a.B, b.B, t), (int)MathF.Lerp(a.A, b.A, t));
        }

        public static GColor Berp(GColor c0, GColor c1, GColor c2, GColor c3, float t1, float t2)
        {
            return BilinearInterp(c0, c1, c2, c3, t1, t2);
        }
        public static GColor BilinearInterp(GColor c0, GColor c1, GColor c2, GColor c3, float t1, float t2)
        {
            GColor m1 = GColor.Lerp(c0, c1, t1);
            GColor m2 = GColor.Lerp(c2, c3, t1);

            return GColor.Lerp(m1, m2, t2);
        }
        #endregion
    }
}

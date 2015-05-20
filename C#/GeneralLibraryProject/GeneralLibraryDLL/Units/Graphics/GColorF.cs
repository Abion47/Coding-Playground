using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Graphics
{
    public class GColorF
    {
        private float _a;
        private float _r;
        private float _g;
        private float _b;

        public float A
        {
            get { return _a; }
            set { this._a = MathF.Clamp(value); }
        }

        public float R
        {
            get { return _r; }
            set { this._r = MathF.Clamp(value); }
        }

        public float G
        {
            get { return _g; }
            set { this._g = MathF.Clamp(value); }
        }

        public float B
        {
            get { return _b; }
            set { this._b = MathF.Clamp(value); }
        }

        public GColorF() : this(0f, 0f, 0f, 1f) { }
        public GColorF(float r, float g, float b) : this(r, g, b, 1f) { }
        public GColorF(float r, float g, float b, float a)
        {
            this._a = MathF.Clamp(a);
            this._r = MathF.Clamp(r);
            this._g = MathF.Clamp(g);
            this._b = MathF.Clamp(b);
        }

        public static explicit operator GColor(GColorF c)
        {
            return new GColor((int)(c.R * 255), (int)(c.G * 255), (int)(c.B * 255), (int)(c.A * 255));
        }

        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(((GColor)this).IntValue);
        }

        public override string ToString()
        {
            return "{ A: " + A.ToString("F4") + ", R: " + R.ToString("F4") + " G: " + G.ToString("F4") + ", B: " + B.ToString("F4") + " }";
        }

        #region Interpolation Functions
        public static GColorF Lerp(GColorF a, GColorF b, float t)
        {
            return LinearInterp(a, b, t);
        }
        public static GColorF LinearInterp(GColorF a, GColorF b, float t)
        {
            return new GColorF(MathF.Lerp(a.R, b.R, t), MathF.Lerp(a.G, b.G, t), MathF.Lerp(a.B, b.B, t), MathF.Lerp(a.A, b.A, t));
        }

        public static GColorF Berp(GColorF c0, GColorF c1, GColorF c2, GColorF c3, float t1, float t2)
        {
            return BilinearInterp(c0, c1, c2, c3, t1, t2);
        }
        public static GColorF BilinearInterp(GColorF c0, GColorF c1, GColorF c2, GColorF c3, float t1, float t2)
        {
            GColorF m1 = GColorF.Lerp(c0, c1, t1);
            GColorF m2 = GColorF.Lerp(c2, c3, t1);

            return GColorF.Lerp(m1, m2, t2);
        }
        #endregion
    }
}

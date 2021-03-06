﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Numerical
{
    public class Complex32F
    {
        private float _r, _i;

        public float Real
        {
            get { return _r; }
            set { this._r = value; }
        }

        public float Imaginary
        {
            get { return _i; }
            set { this._i = value; }
        }

        public Complex32F() : this(0, 0) { }
        public Complex32F(float r, float i)
        {
            this._r = r;
            this._i = i;
        }

        public static Complex32F operator +(Complex32F a) { return new Complex32F(a._r, a._i); }
        public static Complex32F operator -(Complex32F a) { return new Complex32F(-a._r, -a._i); }

        public static Complex32F operator +(Complex32F a, Complex32F b) { return new Complex32F(a._r + b._r, a._i + b._i); }
        public static Complex32F operator -(Complex32F a, Complex32F b) { return new Complex32F(a._r - b._r, a._i - b._i); }
        public static Complex32F operator *(Complex32F a, Complex32F b) { return new Complex32F(a._r * b._r, a._i * b._i); }
        public static Complex32F operator *(Complex32F a, float i) { return new Complex32F(a._r * i, a._i * i); }
        public static Complex32F operator /(Complex32F a, Complex32F b) { if (b._r == 0 || b._i == 0) throw new DivideByZeroException(); return new Complex32F(a._r / b._r, a._i / b._i); }
        public static Complex32F operator /(Complex32F a, float i) { if (i == 0) throw new DivideByZeroException(); return new Complex32F(a._r / i, a._i / i); }

        public static bool operator ==(Complex32F a, Complex32F b) { return a._r == b._r && a._i == b._i; }
        public static bool operator !=(Complex32F a, Complex32F b) { return a._r == b._r && a._i == b._i; }

        public static explicit operator Complex32(Complex32F a) { return new Complex32((int)a._r, (int)a._i); }

        public override string ToString()
        {
            return "{ " + _r.ToString("F4") + (_i < 0 ? " - " : " + ") + MathF.Abs(_i).ToString("F4") + "i }";
        }
    }
}

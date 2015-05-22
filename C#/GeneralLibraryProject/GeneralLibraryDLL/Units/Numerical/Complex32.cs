using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Numerical
{
    public class Complex32
    {
        private int _r, _i;

        public int Real
        {
            get { return _r; }
            set { this._r = value; }
        }

        public int Imaginary
        {
            get { return _i; }
            set { this._i = value; }
        }

        public Complex32() : this(0, 0) { }
        public Complex32(int r, int i)
        {
            this._r = r;
            this._i = i;
        }

        public static Complex32 operator +(Complex32 a) { return new Complex32(a._r, a._i); }
        public static Complex32 operator -(Complex32 a) { return new Complex32(-a._r, -a._i); }

        public static Complex32 operator +(Complex32 a, Complex32 b) { return new Complex32(a._r + b._r, a._i + b._i); }
        public static Complex32 operator -(Complex32 a, Complex32 b) { return new Complex32(a._r - b._r, a._i - b._i); }
        public static Complex32 operator *(Complex32 a, Complex32 b) { return new Complex32(a._r * b._r, a._i * b._i); }
        public static Complex32 operator *(Complex32 a, int i) { return new Complex32(a._r * i, a._i * i); }
        public static Complex32 operator /(Complex32 a, Complex32 b) { if (b._r == 0 || b._i == 0) throw new DivideByZeroException(); return new Complex32(a._r / b._r, a._i / b._i); }
        public static Complex32 operator /(Complex32 a, int i) { if (i == 0) throw new DivideByZeroException(); return new Complex32(a._r / i, a._i / i); }

        public static bool operator ==(Complex32 a, Complex32 b) { return a._r == b._r && a._i == b._i; }
        public static bool operator !=(Complex32 a, Complex32 b) { return a._r == b._r && a._i == b._i; }

        public static implicit operator Complex32F(Complex32 a) { return new Complex32F(a._r, a._i); }

        public override string ToString()
        {
            return "{ " + _r + (_i < 0 ? " - " : " + ") + Math.Abs(_i) + "i }";
        }
    }
}

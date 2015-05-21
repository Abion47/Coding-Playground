using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Numerical
{
    public class Int128
    {
        private ulong _b, _t;

        public Int128() : this(0, 0) { }
        public Int128(ulong b, ulong t)
        {
            _b = b;
            _t = t;
        }

        #region Bitwise Operators
        public static Int128 operator ~(Int128 a)
        {
            return new Int128(~a._b, ~a._t);
        }

        public static Int128 operator |(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator &(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator ^(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator <<(Int128 a, int i)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator >>(Int128 a, int i)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Unary Operators
        public static Int128 operator +(Int128 a)
        {
            return a;
        }

        public static Int128 operator -(Int128 a)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator ++(Int128 a)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator --(Int128 a)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Binary Operators
        public static Int128 operator +(Int128 a, Int128 b)
        {
            var res = new Int128(a._b + b._b, a._t + b._t);
            if (a._t < 0 && b._t < 0) res._b++;
            return res;
        }

        public static Int128 operator -(Int128 a, Int128 b)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator *(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator /(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static Int128 operator %(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Boolean Operators
        public static bool operator ==(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(Int128 a, Int128 B)
        {
            throw new NotImplementedException();
        }
        #endregion







    }
}

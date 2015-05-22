using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units.Numerical
{
    // Class to hold an integer in base 10 with an arbitrary number of digits
    public class Base10Integer
    {
        private int[] digits;
        private bool negative;

        private int this[int i]
        {
            get { return digits[i]; }
            set { this.digits[i] = value; }
        }

        private int Length
        {
            get { return digits.Length; }
        }

        public Base10Integer() { }
        public Base10Integer(int i)
        {
            int l = MathI.Magnitude(i);
            digits = new int[MathI.Magnitude(i)];

            if (i < 0)
            {
                negative = true;
                i = -i;
            }

            for (int j = 0; j < digits.Length; j++)
            {
                digits[j] = i % 10;
                i /= 10;
            }
        }

        public static Base10Integer Parse(string s)
        {
            Base10Integer bti = new Base10Integer();

            if (s[0] == '-') 
            {
                bti.digits = new int[s.Length - 1];
                bti.negative = true;
            }
            else
            {
                bti.digits = new int[s.Length];
            }
            
            int j = 0;
            for (int i = s.Length - 1; i >= 0 && j < bti.digits.Length; i--)
            {
                switch (s[i])
                {
                    case '0': 
                        bti[j] = 0;
                        break;
                    case '1':
                        bti[j] = 1;
                        break;
                    case '2':
                        bti[j] = 2;
                        break;
                    case '3':
                        bti[j] = 3;
                        break;
                    case '4':
                        bti[j] = 4;
                        break;
                    case '5':
                        bti[j] = 5;
                        break;
                    case '6':
                        bti[j] = 6;
                        break;
                    case '7':
                        bti[j] = 7;
                        break;
                    case '8':
                        bti[j] = 8;
                        break;
                    case '9':
                        bti[j] = 9;
                        break;
                    default:
                        throw new ArgumentException();
                }

                j = s.Length - i;
            }

            return bti;
        }

        private void IncreaseDigits()
        {
            int[] newArr = new int[digits.Length + 1];

            for (int i = 0; i < newArr.Length; i++)
            {
                if (i < digits.Length)
                    newArr[i] = digits[i];
                else
                    newArr[i] = 0;
            }

            digits = newArr;
        }

        public bool IsZero()
        {
            for (int i = 0; i < Length; i++)
            {
                if (this[i] != 0) return false;
            }

            return true;
        }

        #region Unary Operators
        public static Base10Integer operator +(Base10Integer a)
        {
            return a;
        }

        public static Base10Integer operator -(Base10Integer a)
        {
            a.negative = true;
            return a;
        }

        public static Base10Integer operator ++(Base10Integer a)
        {
            return a + 1;
        }

        public static Base10Integer operator --(Base10Integer a)
        {
            return a - 1;
        }
        #endregion

        #region Binary Operators
        public static Base10Integer operator +(Base10Integer a, Base10Integer b)
        {
            if (a.negative && !b.negative)
                return b - a;
            if (b.negative && !a.negative)
                return a - b;
            if (a.IsZero())
                return b;
            if (b.IsZero())
                return a;

            Base10Integer bti = new Base10Integer();
            bti.digits = new int[Math.Max(a.digits.Length, b.digits.Length) + 1];
            bti.negative = a.negative && b.negative;
            int carry = 0;

            for (int i = 0; i < a.digits.Length || i < b.digits.Length; i++)
            {
                if (i < a.digits.Length && i < b.digits.Length)
                {
                    bti[i] = a[i] + b[i] + carry;
                    carry = 0;
                    while (bti[i] >= 10)
                    {
                        bti[i] -= 10;
                        carry++;
                    }
                }
                else if (i < a.Length)
                {
                    bti[i] = a[i] + carry;
                    carry = 0;
                    while (bti[i] >= 10)
                    {
                        bti[i] -= 10;
                        carry++;
                    }
                }
                else if (i < b.Length)
                {
                    bti[i] = b[i] + carry;
                    carry = 0;
                    while (bti[i] >= 10)
                    {
                        bti[i] -= 10;
                        carry++;
                    }
                }
            }

            return bti;
        }

        public static Base10Integer operator +(Base10Integer a, int i)
        {
            if (a.negative && i > 0)
                return i - a;
            if (i < 0 && !a.negative)
                return a - i;
            if (a.IsZero())
                return new Base10Integer(i);
            if (i == 0)
                return a;

            Base10Integer bti = new Base10Integer();
            bti.digits = new int[a.digits.Length];
            bti.negative = a.negative && i < 0;
            i = Math.Abs(i);
            int carry = 0;
            int digit = 0;

            while (i != 0)
            {
                if (digit == bti.Length)
                    a.IncreaseDigits();

                bti[digit] += i % 10;
                i = i / 10;
                carry = 0;

                while (bti[digit] >= 10)
                {
                    a[digit] -= 10;
                    carry++;
                }
            }

            if (carry != 0)
            {
                
                a.IncreaseDigits();

            }

            throw new NotImplementedException();
        }

        public static Base10Integer operator +(int i, Base10Integer a)
        {
            return a + i;
        }

        public static Base10Integer operator -(Base10Integer a, Base10Integer b)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator -(Base10Integer a, int i)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator -(int i, Base10Integer a)
        {
            return -a + i;
        }

        public static Base10Integer operator *(Base10Integer a, Base10Integer b)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator *(Base10Integer a, int i)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator /(Base10Integer a, Base10Integer b)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator /(Base10Integer a, int i)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator %(Base10Integer a, Base10Integer b)
        {
            throw new NotImplementedException();
        }

        public static Base10Integer operator %(Base10Integer a, int i)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Boolean Operators
        public static bool operator ==(Base10Integer a, Base10Integer b)
        {
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        public static bool operator !=(Base10Integer a, Base10Integer b)
        {
            return !(a == b);
        }

        public static bool operator >(Base10Integer a, Base10Integer b)
        {
            if (a.Length != b.Length) return a.Length > b.Length;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] != b[i]) return a[i] > b[i];
            }

            return false;
        }

        public static bool operator <(Base10Integer a, Base10Integer b)
        {
            if (a.Length != b.Length) return a.Length < b.Length;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] != b[i]) return a[i] < b[i];
            }

            return false;
        }

        public static bool operator >=(Base10Integer a, Base10Integer b)
        {
            if (a.Length != b.Length) return a.Length > b.Length;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] != b[i]) return a[i] > b[i];
            }

            return true;
        }

        public static bool operator <=(Base10Integer a, Base10Integer b)
        {
            if (a.Length != b.Length) return a.Length < b.Length;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] != b[i]) return a[i] < b[i];
            }

            return true;
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in digits)
            {
                if (b >= 10) continue;
                sb.Insert(0, b);
            }

            string val = sb.ToString().TrimStart('0');
            if (negative) val = "-" + val;

            return val;
        }

    }
}

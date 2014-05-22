using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public static class SimpleFunctions
    {
        #region Value Manipulation
        public static int ReverseInt(int i)
        {
            int res = 0;

            while (i > 0)
            {
                res *= 10;
                res += i % 10;
                i /= 10;
            }

            return res;
        }

        #endregion

        #region Value Conversion
        public static string IntToBinary(int i)
        {
            StringBuilder sb = new StringBuilder();

            while (i > 0)
            {
                sb.Insert(0, i & 1);
                i = i >> 1;
            }

            return sb.ToString();
        }

        public static int BinaryToInt(string b)
        {
            int ret = 0;
            int buf = 1;

            for (int i = b.Length - 1; i >= 0; i--)
            {
                if (b[i] == '1')
                {
                    for (int j = 0; j < b.Length - 1 - i; j++)
                        buf *= 2;
                    ret += buf;
                    buf = 1;
                }

                else if (b[i] != '0' && b[i] != ' ')
                    throw new ArgumentException("Binary string must only be comprised of '0's and '1's.");
            }

            return ret;
        }

        public static int CharToAscii(char c)
        {
            return (int)c;
        }

        public static char AsciiToChar(int i)
        {
            return (char)i;
        }

        #endregion

        #region Console IO
        

        #endregion

        #region Math Functions
        // In the structure [ax^2 + bx + c = 0]
        public static double[] GetRootsOfAQuadraticEquation(float a, float b, float c)
        {
            double[] roots = null;

            if (a == 0 && b == 0)
            {
                return roots;
            }
            else
            {
                double det = (b * b) - (4 * a * c);

                if (det > 0)
                {
                    roots = new double[2];
                    roots[0] = (-b - Math.Sqrt(det)) / (2 * a);
                    roots[1] = (-b + Math.Sqrt(det)) / (2 * a);
                    return roots;
                }
                else if (det == 0)
                {
                    roots = new double[1];
                    roots[0] = -b / (2 * a);
                    return roots;
                }
                else
                {
                    roots = new double[4];
                    double real = -b / (2 * a);
                    double imag = Math.Sqrt(-det) / (2 * a);
                    roots[0] = real;
                    roots[1] = imag;
                    roots[2] = real;
                    roots[3] = -imag;
                    return roots;
                }
            }
        }
        #endregion
    }
}

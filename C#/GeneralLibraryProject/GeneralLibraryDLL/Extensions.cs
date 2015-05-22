using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public static class Extensions
    {
        #region int
        public static void Print(this int i)
        {
            Console.WriteLine(i);
        }
        #endregion

        #region float
        private const float fEps = 1e-10f;
        public static bool IsZero(this float f)
        {
            return Math.Abs(f) < fEps;
        }
        public static int ToInt(this float f)
        {
            return (int)(f + 0.5f);
        }
        public static void Print(this float f)
        {
            Console.WriteLine(f);
        }
        #endregion

        #region double
        public static int ToInt(this double f)
        {
            return (int)(f + 0.5);
        }
        public static void Print(this double d)
        {
            Console.WriteLine(d);
        }
        #endregion

        #region string
        public static void Print(this string s)
        {
            Console.WriteLine(s);
        }
        #endregion

        #region StringBuilder
        public static StringBuilder TrimEnd(this StringBuilder sb, params char[] list)
        {
            if (sb == null || sb.Length == 0) return sb;

            bool found = false;
            int i, j;
            for (i = sb.Length - 1; i >= 0; i--) 
            {
                for (j = 0; j < list.Length; j++)
                {
                    if (sb[i] == list[j])
                    {
                        sb.Remove(i, 1);

                        found = true;
                        break;
                    }
                }

                if (!found) break;
            }

            return sb;
        }
        public static StringBuilder TrimStart(this StringBuilder sb, params char[] list)
        {
            if (sb == null || sb.Length == 0) return sb;

            bool found = false;
            int i, j;
            for (i = 0; i < sb.Length; i++)
            {
                for (j = 0; j < list.Length; j++)
                {
                    if (sb[i] == list[j])
                    {
                        sb.Remove(i, 1);

                        found = true;
                        break;
                    }
                }

                if (!found) break;
                else i--;
            }

            return sb;
        }
        public static void Print(this StringBuilder sb)
        {
            Console.WriteLine(sb.ToString());
        }
        #endregion
    }
}

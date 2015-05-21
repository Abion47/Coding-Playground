using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public static class Extensions
    {
        private const float fEps = 1e-10f;
        public static bool IsZero(this float f)
        {
            return Math.Abs(f) < fEps;
        }

        public static int ToInt(this float f)
        {
            return (int)(f + 0.5f);
        }
        public static int ToInt(this double f)
        {
            return (int)(f + 0.5);
        }
    }
}

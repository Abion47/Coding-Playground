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
    }
}

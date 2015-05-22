using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public class MathI
    {
        public static int Magnitude(int i)
        {
            return (int)Math.Log10(Math.Abs(i)) + 1;
        }
    }
}

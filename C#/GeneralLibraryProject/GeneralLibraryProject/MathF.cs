using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public class MathF
    {
        /// <summary>
        /// Clamps an input between a minimum value 0.0 and a maximum value 1.0.
        /// </summary>
        /// <param name="f">Value to be clamped.</param>
        /// <returns>0 if f is less than 0, or 1 if f is greater than 1, else f.</returns>
        public static float Clamp(float f)
        {
            return f < 0f ? 0f : f > 1f ? 1f : f;
        }
        /// <summary>
        /// Clamps an input between two configurable minimum and maximum values.
        /// </summary>
        /// <param name="a">Minimum value of the clamp.</param>
        /// <param name="b">Maximum value of the clamp.</param>
        /// <param name="f">Value to be clamped.</param>
        /// <returns>a if f is less than a, or b if f is greater than b, else f.</returns>
        public static float Clamp(float a, float b, float f)
        {
            return f < a ? a : f > b ? b : f;
        }

        /// <summary>
        /// Returns the absolute value of an input.
        /// </summary>
        /// <param name="f">Value to be evaluated.</param>
        /// <returns>Negative f if f is less than 0, else f.</returns>
        public static float Abs(float f)
        {
            return f < 0f ? -f : f;
        }

        /// <summary>
        /// Returns a value raised to a given exponent.
        /// </summary>
        /// <param name="f">The value to be evaluated.</param>
        /// <param name="i">The exponent to which the value is raised.</param>
        /// <returns>f to the power of i.</returns>
        public static float Pow(float f, int i)
        {
            if (i == 0) return 1;
            bool n = i < 0;
            i = Math.Abs(i);
            while (i > 1) i *= i;
            return n ? 1f / i : i;
        }

        /// <summary>
        /// Perform a 1-dimensional linear interpolation.
        /// </summary>
        /// <param name="a">The starting value.</param>
        /// <param name="b">The ending value.</param>
        /// <param name="t">The interpolation time parameter.</param>
        /// <returns>The interpolated value at t between a and b.</returns>
        public static float Lerp(float a, float b, float t)
        {
            return (1 - t) * a + t * b;
        }
    }
}

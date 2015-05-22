using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public class MathF
    {
        #region Constants
        public static readonly float PI = (float)Math.PI;
        public static readonly float LOG_2_10 = (float)Math.Log(10, 2);
        #endregion

        #region Bounding Functions
        /// <summary>
        /// Compares two given values and returns whichever is lesser.
        /// </summary>
        /// <param name="a">The first given value.</param>
        /// <param name="b">The second given value.</param>
        /// <returns>The lesser of a and b.</returns>
        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Compares two given values and returns whichever is greater.
        /// </summary>
        /// <param name="a">The first given value.</param>
        /// <param name="b">The second given value.</param>
        /// <returns>The greater of a and b.</returns>
        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

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
        #endregion

        #region Math Functions
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
        /// Calculates the square root of a given value. (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="f">Value to be evaluated.</param>
        /// <returns>The square root of f.</returns>
        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        /// <summary>
        /// Calculates the orders of magnitude of a base 10 number.
        /// </summary>
        /// <param name="f">Value to be analyzed.</param>
        /// <returns>The order of magnitude of the given value.</returns>
        public static int Magnitude(float f)
        {
            return (int)Math.Log10(f) + 1;
        }
        #endregion

        #region Trigonometric Functions
        /// <summary>
        /// Computes the sine of a given angle. (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric sine at angle a.</returns>
        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        /// <summary>
        /// Computes the cssine of a given angle. (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric cssine at angle a.</returns>
        public static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }

        /// <summary>
        /// Computes the tangent of a given angle. (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric tangent at angle a.</returns>
        public static float Tan(float a)
        {
            return (float)Math.Tan(a);
        }

        /// <summary>
        /// Computes the arcsine of a given angle. Analogous to "sin^-1(a)". (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric arcsine at angle a.</returns>
        public static float Asin(float a)
        {
            return (float)Math.Asin(a);
        }

        /// <summary>
        /// Computes the cssine of a given angle. Analogous to "sin^-1(a)". (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric arccosine at angle a.</returns>
        public static float Acos(float a)
        {
            return (float)Math.Acos(a);
        }

        /// <summary>
        /// Computes the tangent of a given angle. Analogous to "sin^-1(a)". (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="a">The angle to evaluate.</param>
        /// <returns>The trigonometric arctangent at angle a.</returns>
        public static float Atan(float a)
        {
            return (float)Math.Atan(a);
        }

        /// <summary>
        /// Computes the arctangent of a given coordinate pair. (Float wrapper for the Math class method.)
        /// </summary>
        /// <param name="y">The y coordinate of the coordinate pair.</param>
        /// <param name="x">The x coordinate of the coordinate pair.</param>
        /// <returns>The trigonometric tangent at angle formed by the coordinate pair relative to the origin.</returns>
        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }
        #endregion

        #region Interpolation Functions
        /// <summary>
        /// Perform a 1-dimensional linear interpolation. (A convenience method for LinearInterp.)
        /// </summary>
        /// <param name="a">The starting value.</param>
        /// <param name="b">The ending value.</param>
        /// <param name="t">The interpolation time parameter.</param>
        /// <returns>The interpolated value at t between a and b.</returns>
        public static float Lerp(float a, float b, float t)
        {
            return LinearInterp(a, b, t);
        }
        /// <summary>
        /// Perform a 1-dimensional linear interpolation.
        /// </summary>
        /// <param name="a">The starting value.</param>
        /// <param name="b">The ending value.</param>
        /// <param name="t">The interpolation time parameter.</param>
        /// <returns>The interpolated value at t in the range of [a, b].</returns>
        public static float LinearInterp(float a, float b, float t)
        {
            return (1 - t) * a + t * b;
        }

        /// <summary>
        /// Performs a 1-dimensional cubic interpolation. (Convenience method for CubicInterp.)
        /// </summary>
        /// <param name="p0">Preceding point for the cubic spline function.</param>
        /// <param name="p1">The lower bounds for the interpolation.</param>
        /// <param name="p2">The upper bounds fot the interpolation.</param>
        /// <param name="p3">The following point for the cubic spline function.</param>
        /// <param name="t">The interpolation time parameter.</param>
        /// <returns>The interpolated value at t in the range of [p1, p2].</returns>
        public static float Cerp(float p0, float p1, float p2, float p3, float t)
        {
            return CubicInterp(p0, p1, p2, p3, t);
        }
        /// <summary>
        /// Performs a 1-dimensional cubic interpolation.
        /// </summary>
        /// <param name="p0">Preceding point for the cubic spline function.</param>
        /// <param name="p1">The lower bounds for the interpolation.</param>
        /// <param name="p2">The upper bounds fot the interpolation.</param>
        /// <param name="p3">The following point for the cubic spline function.</param>
        /// <param name="t">The interpolation time parameter.</param>
        /// <returns>The interpolated value at t in the range of [p1, p2].</returns>
        public static float CubicInterp(float p0, float p1, float p2, float p3, float t)
        {
            float a0, a1, a2, a3;

            a0 = p3 - p2 - p0 + p1;
            a1 = p0 - p1 - a0;
            a2 = p2 - p0;
            a3 = p1;

            return a0 * (t * t * t) + a1 * (t * t) + a2 * t + a3;
        }

        /// <summary>
        /// Performs a 2-dimensional linear interpolation. (A convenience method for BilinearInterp.)
        /// </summary>
        /// <param name="q00">Value in the upper left sample site.</param>
        /// <param name="q01">Value in the upper right sample site.</param>
        /// <param name="q10">Value in the lower left sample site.</param>
        /// <param name="q11">Value in the lower right sample site.</param>
        /// <param name="tx">The first dimension interpolation time parameter.</param>
        /// <param name="ty">The second dimension interpolation time parameter.</param>
        /// <returns>The interpolated value at (tx, ty) in the plane of [q00, q01, q10, q11].</returns>
        public static float Berp(
                float q00, float q01,
                float q10, float q11,
                float tx, float ty)
        {
            return BilinearInterp(q00, q01, q10, q11, tx, ty);
        }
        /// <summary>
        /// Performs a 2-dimensional linear interpolation
        /// </summary>
        /// <param name="q00">Value in the upper left sample site.</param>
        /// <param name="q01">Value in the upper right sample site.</param>
        /// <param name="q10">Value in the lower left sample site.</param>
        /// <param name="q11">Value in the lower right sample site.</param>
        /// <param name="tx">The first dimension interpolation time parameter.</param>
        /// <param name="ty">The second dimension interpolation time parameter.</param>
        /// <returns>The interpolated value at (tx, ty) in the plane of [q00, q01, q10, q11].</returns>
        public static float BilinearInterp(
                float q00, float q01,
                float q10, float q11,
                float tx, float ty)
        {
            float x1 = Lerp(q00, q01, tx);
            float x2 = Lerp(q10, q11, tx);

            float r = Lerp(x1, x2, ty);

            return r;
        }

        /// <summary>
        /// Performs a 2-dimensional cubic interpolation.
        /// </summary>
        /// <param name="p">The array representing a 4x4 grid of sample points. Must have a length of exactly 16.</param>
        /// <param name="tx">The first dimension interpolation time parameter.</param>
        /// <param name="ty">The second dimension interpolation time parameter.</param>
        /// <returns>The interpolated value at (tx, ty) in the plane of [p5, p6, p9, p10].</returns>
        public static float BicubicInterp(float[] p, float tx, float ty)
        {
            if (p.Length != 16)
                throw new ArgumentException("The number of values passed must be exactly 16.");

            float y1 = Cerp(p[0], p[4], p[8], p[12], ty);
            float y2 = Cerp(p[1], p[5], p[9], p[13], ty);
            float y3 = Cerp(p[2], p[6], p[10], p[14], ty);
            float y4 = Cerp(p[3], p[7], p[11], p[15], ty);

            return Cerp(y1, y2, y3, y4, tx);
        }

        /// <summary>
        /// Performs a 3-dimensional linear interpolation.
        /// </summary>
        /// <param name="q000">The north-west-top sample point.</param>
        /// <param name="q001">The north-east-top sample point.</param>
        /// <param name="q010">The north-west-bottom sample point.</param>
        /// <param name="q011">The north-east-bottom sample point.</param>
        /// <param name="q100">The south-west-top sample point.</param>
        /// <param name="q101">The south-east-top sample point.</param>
        /// <param name="q110">The south-west-bottom sample point.</param>
        /// <param name="q111">The south-east-bottom sample point.</param>
        /// <param name="tx">The first dimension interpolation time value.</param>
        /// <param name="ty">The second dimension interpolation time value.</param>
        /// <param name="tz">The third dimension interpolation time value.</param>
        /// <returns>The interpolated value at (tx, ty, tz) in the cube of [q000, q001, q010, q011, q100, q101, q110, q111].</returns>
        public static float TrilinearInterp(
                float q000, float q001, 
                float q010, float q011, 
                float q100, float q101, 
                float q110, float q111, 
                float tx, float ty, float tz)
            {
                float x1 = Lerp(q000, q001, tx);
                float x2 = Lerp(q010, q011, tx);
                float x3 = Lerp(q100, q101, tx);
                float x4 = Lerp(q110, q111, tx);

                float y1 = Lerp(x1, x2, ty);
                float y2 = Lerp(x3, x4, ty);

                float r = Lerp(y1, y2, tz);

                return r;
            }
        #endregion

        #region Statistical Functions
        /// <summary>
        /// Calculates the statistical mean of a given list of values.
        /// </summary>
        /// <param name="a">A variable length parameter containing values.</param>
        /// <returns>The statistical mean of the values.</returns>
        public static float Mean(params float[] a)
        {
            float val = 0;
            for (int i = 0; i < a.Length; i++) val += a[i];
            return val / a.Length;
        }

        /// <summary>
        /// Calculates the sum of a given list of values.
        /// </summary>
        /// <param name="a">A variable length parameter containing values.</param>
        /// <returns>The sum of the values.</returns>
        public static float Sum(params float[] a)
        {
            float val = 0;
            for (int i = 0; i < a.Length; i++) val += a[i];
            return val;
        }
        #endregion
    }
}

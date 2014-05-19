using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general
{
    public static class Functions
    {
        public static class Interpolation
        {
            // Linear Interpolation
            public static float Lerp(float start, float end, float t)
            {
                return (1 - t) * start + t * end;
            }

            public static float Bilinear(
                float q11, float q12, 
                float q21, float q22, 
                float tx, float ty)
            {
                float x1 = Lerp(q11, q12, tx);
                float x2 = Lerp(q21, q22, tx);

                float r = Lerp(x1, x2, ty);

                return r;
            }

            public static float Trilinear(
                float q111, float q112, 
                float q121, float q122, 
                float q211, float q212, 
                float q221, float q222, 
                float tx, float ty, float tz)
            {
                float x1 = Lerp(q111, q112, tx);
                float x2 = Lerp(q121, q122, tx);
                float x3 = Lerp(q211, q212, tx);
                float x4 = Lerp(q221, q222, tx);

                float y1 = Lerp(x1, x2, ty);
                float y2 = Lerp(x3, x4, ty);

                float r = Lerp(y1, y2, tz);

                return r;
            }

            // Cubic Interpolation
            public static float Cerp(float[] p, float t)
            {
                if (p.Length != 4)
                    throw new ArgumentException("The number of values passed must be exactly 4.");

                float a0, a1, a2, a3;

                a0 = p[3] - p[2] - p[0] + p[1];
                a1 = p[0] - p[1] - a0;
                a2 = p[2] - p[0];
                a3 = p[1];

                return a0 * (t * t * t) + a1 * (t * t) + a2 * t + a3;

                //return p[1] + 0.5f * t * (p[2] - p[0] + t * (2.0f * p[0] - 5.0f * p[1] + 4.0f * p[2] - p[3] + t * (3.0f * (p[1] - p[2]) + p[3] - p[0])));
                //return (p[0] * t * t * t) + (p[1] * t * t) + (p[2] * t) + p[3];
            }

            public static float Bicubic(float[] p, float tx, float ty)
            {
                if (p.Length != 16)
                    throw new ArgumentException("The number of values passed must be exactly 16.");

                float y1 = Cerp(new float[] { p[0], p[4], p[8], p[12] }, ty);
                float y2 = Cerp(new float[] { p[1], p[5], p[9], p[13] }, ty);
                float y3 = Cerp(new float[] { p[2], p[6], p[10], p[14] }, ty);
                float y4 = Cerp(new float[] { p[3], p[7], p[11], p[15] }, ty);

                return Cerp(new float[] { y1, y2, y3, y4 }, tx);
            }
        }
    }
}

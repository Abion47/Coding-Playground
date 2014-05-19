using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class Shapes
    {
        public struct Triangle
        {
            PointF v1;
            PointF v2;
            PointF v3;
        }

        public static Vector2F[] CreateStar(int points, Vector2F center, float radius, float r = 0, float innerRadiusRatio = 0.5f)
        {
            Vector2F[] ret = new Vector2F[points * 2];
            float delta = (float)Math.PI * 2 / (float)ret.Length;
            
            for (int i = 0; i < ret.Length; i++)
            {
                Vector2F v = new Vector2F(
                    (float)Math.Cos(delta * i + r),
                    (float)Math.Sin(delta * i + r));

                v.X *= radius;
                v.Y *= radius;

                if (i % 2 == 0)
                {
                    v.X *= innerRadiusRatio;
                    v.Y *= innerRadiusRatio;
                }

                ret[i] = v + center;
            }

            return ret;
        }
    }
}

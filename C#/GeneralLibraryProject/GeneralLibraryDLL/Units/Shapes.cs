using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public class Shapes
    {
        public static Vector2F[] CreatePolygon(int points, Vector2F center, float radius, float rotation = 0)
        {
            Vector2F[] ret = new Vector2F[points];
            float delta = MathF.PI * 2 / (float)ret.Length;

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = center + new Vector2F(
                    MathF.Cos(delta * i + rotation),
                    MathF.Sin(delta * i + rotation));
            }

            return ret;
        }

        public static Vector2F[] CreateStar(int points, Vector2F center, float radius, float rotation = 0, float innerRadiusRatio = 0.5f)
        {
            Vector2F[] ret = new Vector2F[points * 2];
            float delta = MathF.PI * 2 / (float)ret.Length;
            
            for (int i = 0; i < ret.Length; i++)
            {
                Vector2F v = new Vector2F(
                    MathF.Cos(delta * i + rotation),
                    MathF.Sin(delta * i + rotation));

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

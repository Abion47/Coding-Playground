using org.general.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace org.general.Vectorization
{
    public class Polygon
    {
        private Vector2F[] vertices;

        public Vector2F[] Vertices { get { return this.vertices; } set { this.vertices = value; } }

        public Polygon(params Vector2F[] vertices)
        {
            this.vertices = vertices;
        }

        public Bitmap Draw()
        {
            throw new NotImplementedException();
        }

        public static class Utility
        {
            public static void SimplifyPolygon(Polygon p, float minDiff = 0.05f)
            {
                List<Vector2F> simplifiedVertices = new List<Vector2F>();
                int last = -1;
                float slope = 0f;
                float lastSlope = 0f;

                for (int i = 0; i < p.vertices.Length; i++)
                {
                    if (last == -1)
                    {
                        simplifiedVertices.Add(p.vertices[i]);
                        last++;
                        continue;
                    }

                    slope = Vector2F.Utility.Slope(simplifiedVertices[last], p.vertices[i]);

                    if (float.IsNaN(slope) || float.IsNaN(lastSlope))
                    {
                        if (float.IsNaN(slope) && float.IsNaN(lastSlope))
                        {
                            lastSlope = slope;
                            simplifiedVertices.Add(p.vertices[i]);
                            last++;
                        }
                    }
                    else if (Math.Abs(slope - lastSlope) >= minDiff)
                    {
                        lastSlope = slope;
                        simplifiedVertices.Add(p.vertices[i]);
                        last++;
                    }
                }

                p.vertices = simplifiedVertices.ToArray();
            }

            public static BoxF BoundingBox(Polygon p)
            {
                float minX = float.MaxValue,
                    minY = float.MaxValue,
                    maxX = float.MinValue,
                    maxY = float.MinValue;

                foreach (var v in p.vertices)
                {
                    minX = Math.Min(v.X, minX);
                    minY = Math.Min(v.Y, minY);
                    maxX = Math.Max(v.X, maxX);
                    maxY = Math.Max(v.Y, maxY);
                }

                return new BoxF(minX, minY, maxX - minX, maxY - minY);
            } 

            public static Polygon ConvexHull(Polygon p)
            {
                throw new NotImplementedException();
            }

            public static object DelaunayTriangulation(Polygon p) 
            {
                throw new NotImplementedException();
            }

            public static object VoronoiDiagram(Polygon p)
            {
                throw new NotImplementedException();
            }

        }
    }
}

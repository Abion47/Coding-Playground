using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    partial class Matrix2D
    {
        public static Matrix2D Create2DTransformationMatrix()
        {
            return CreateIdentity(3);
        }

        public Matrix2D Translate(float tx, float ty)
        {
            return new Matrix2D(3, 3, new float[] {
					1, 0, tx,
					0, 1, ty,
					0, 0, 1
				}) * this;
        }

		// Angle in Radians
        public Matrix2D Rotate(float a)
        {
            return new Matrix2D(3, 3, new float[] { 
                (float)Math.Cos(a), (float)Math.Sin(a), 0,
                (float)-Math.Sin(a), (float)Math.Cos(a), 0,
				0, 0, 1 }) * this;
        }

        public Matrix2D Reflect(bool horizontal)
        {
            if (horizontal)
                return new Matrix2D(3, 3, new float[] {
					1, 0, 0,
					0, -1, 0,
					0, 0, 1
				}) * this;
            else
                return new Matrix2D(3, 3, new float[] {
					-1, 0, 0,
					0, 1, 0,
					0, 0, 1
				}) * this;
        }

        public Matrix2D Scale(float sx, float sy)
        {
            return new Matrix2D(3, 3, new float[] {
					sx, 0, 0,
					0, sy, 0,
					0, 0, 1
				}) * this;
        }

        public Matrix2D Shear(float k, bool horizontal)
        {
            if (horizontal)
                return new Matrix2D(3, 3, new float[] {
					1, k, 0,
					0, 1, 0,
					0, 0, 1
				}) * this;
			else
                return new Matrix2D(3, 3, new float[] {
					1, 0, 0,
					k, 1, 0,
					0, 0, 1
				}) * this;
        }

        public Vector2F ApplyTransformation(Vector2F v)
        {
            Matrix2D res = this * new Matrix2D(3, 1, new float[] { v.X, v.Y, 1 });
            return new Vector2F(res.data[0], res.data[1]);
        }
        public Vector2F[] ApplyTransformation(Vector2F[] v)
        {
            Vector2F[] res = new Vector2F[v.Length];
            for (int i = 0; i < v.Length; i++)
                res[i] = ApplyTransformation(v[i]);
            return res;
        }
        public void ApplyTransformation(ref Vector2F[] v)
        { 
            for (int i = 0; i < v.Length; i++) 
                v[i] = ApplyTransformation(v[i]); 
        }
    }
}

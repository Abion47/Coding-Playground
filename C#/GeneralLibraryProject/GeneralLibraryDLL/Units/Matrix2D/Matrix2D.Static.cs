using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    partial class Matrix2D
    {
        public static readonly Matrix2D Identity2 = new Matrix2D(2, 2, new float[] { 1, 0, 0, 1 });
        public static readonly Matrix2D Identity3 = new Matrix2D(3, 3, new float[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });
        public static Matrix2D CreateIdentity(int dimensions)
        {
            Matrix2D identity = new Matrix2D(dimensions, dimensions);
            for (int i = 0; i < dimensions; i++)
                identity[i, i] = 1;
            return identity;
        }

        public static bool IsSquare(Matrix2D a)
        {
            return a.IsSquare();
        }

        public static bool operator ==(Matrix2D a, Matrix2D b)
        {
            if (a.rows != b.rows
                || a.columns != b.columns)
                return false;

            for (int i = 0; i < a.data.Length; i++)
                if (a.data[i] != b.data[i])
                    return false;

            return true;
        }
        public static bool operator !=(Matrix2D a, Matrix2D b)
        {
            return !(a == b);
        }

        public static Matrix2D GetRow(Matrix2D a, int row)
        {
            return a.GetRow(row);
        }
        public static Matrix2D GetColumn(Matrix2D a, int column)
        {
            return a.GetColumn(column);
        }
        public static Matrix2D GetSubSection(Matrix2D a, int startRow, int startColumn, int endRow = int.MaxValue, int endColumn = int.MaxValue)
        {
            return a.GetSubSection(startRow, startColumn, endRow, endColumn);
        }

        public static Matrix2D InsertRow(Matrix2D a, int index = int.MaxValue, float initValue = 0)
        {
            Matrix2D c = new Matrix2D(a.rows + 1, a.columns);

            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    if (i >= index)
                        c[i + 1, j] = a[i, j];
                    else
                        c[i, j] = a[i, j];

            if (initValue != 0)
                for (int i = 0; i < c.columns; i++)
                    c[index == int.MaxValue ? c.rows - 1 : index, i] = initValue;

            return c;
        }
        public static Matrix2D InsertColumn(Matrix2D a, int index = int.MaxValue, float initValue = 0)
        {
            Matrix2D c = new Matrix2D(a.rows, a.columns + 1);

            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    if (j >= index)
                        c[i, j + 1] = a[i, j];
                    else
                        c[i, j] = a[i, j];

            if (initValue != 0)
                for (int i = 0; i < c.rows; i++)
                    c[i, index == int.MaxValue ? c.columns - 1 : index] = initValue;

            return c;
        }
        public static Matrix2D DeleteRow(Matrix2D a, int index)
        {
            Matrix2D c = new Matrix2D(a.rows - 1, a.columns);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (i >= index)
                        c[i, j] = a[i + 1, j];
                    else
                        c[i, j] = a[i, j];

            return c;
        }
        public static Matrix2D DeleteColumn(Matrix2D a, int index)
        {
            Matrix2D c = new Matrix2D(a);
            c.DeleteColumn(index);
            return c;
        }
        public static Matrix2D MakeDoubleWide(Matrix2D a, bool fillWithIdentity = true)
        {
            Matrix2D c = new Matrix2D(a);
            a.MakeDoubleWide(fillWithIdentity);
            return c;
        }

        public static Matrix2D MultiplyRow(Matrix2D a, int row, float f)
        {
            Matrix2D c = new Matrix2D(a);
            c.MultiplyRow(row, f);
            return c;
        }
        public static Matrix2D AddRows(Matrix2D a, int source, int target)
        {
            Matrix2D c = new Matrix2D(a);
            c.AddRows(source, target);
            return c;
        }
        public static Matrix2D SwapRows(Matrix2D a, int r1, int r2)
        {
            Matrix2D c = new Matrix2D(a);
            c.SwapRows(r1, r2);
            return c;
        }

        public static float Determinant(Matrix2D a)
        {
            return a.Determinant();
        }
    }
}

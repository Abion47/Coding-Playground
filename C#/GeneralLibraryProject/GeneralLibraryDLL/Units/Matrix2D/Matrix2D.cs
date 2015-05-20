using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.general.Units
{
    public partial class Matrix2D : IEquatable<Matrix2D>
    {
        protected float[] data;
        protected int rows;
        protected int columns;

        public int Rows
        { get { return rows; } private set { this.rows = value; } }
        public int Columns 
        { get { return columns; } private set { this.columns = value; } }
        public float this[int i, int j] 
        {
            get
            {
                return data[i * columns + j];
            }
            set
            {
                this.data[i * columns + j] = value;
            }
        }

        public Matrix2D()
            : this(CreateIdentity(2)) { }
        public Matrix2D(Matrix2D other)
        {
            this.rows = other.rows;
            this.columns = other.columns;
            this.data = new float[other.data.Length];

            for (int i = 0; i < data.Length; i++)
                data[i] = other.data[i];
        }
        public Matrix2D(int rows, int columns, float initValue = 0f)
        {
            this.rows = rows;
            this.columns = columns;

            data = new float[rows * columns];

            for (int i = 0; i < data.Length; i++)
                data[i] = initValue;
        }
        public Matrix2D(int rows, int columns, float[] values)
        {
            this.rows = rows;
            this.columns = columns;

            data = new float[rows * columns];

            if (data.Length != values.Length)
                throw new ArgumentException("The values array must be the same size as the matrix.");

            for (int i = 0; i < data.Length; i++)
                if (i < values.Length)
                    data[i] = values[i];
                else
                    data[i] = 0;
        }

        public bool IsSquare()
        {
            return rows == columns;
        }
        public bool IsIdentity()
        {
            if (!this.IsSquare())
                return false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i == j)
                    {
                        if (this[i, j] != 1)
                        {
                            return false;
                        }
                    }
                    else if (this[i, j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        IEnumerator<float> GetEnumerator()
        {
            for (int i = 0; i < data.Length; i++)
                yield return data[i];
        }

        public static Matrix2D operator +(Matrix2D a, Matrix2D b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("Matrices must the the same size to add them.");

            Matrix2D c = new Matrix2D(a);
            for (int i = 0; i < c.data.Length; i++)
                c.data[i] += b.data[i];
            return c;
        }
        public static Matrix2D operator +(Matrix2D a, float b)
        {
            Matrix2D c = new Matrix2D(a);

            for (int i = 0; i < c.data.Length; i++)
                c.data[i] += b;

            return c;
        }
        public static Matrix2D operator -(Matrix2D a, Matrix2D b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new NotSupportedException("Matrices must the the same size to subract them.");

            Matrix2D c = new Matrix2D(a);
            for (int i = 0; i < c.data.Length; i++)
                c.data[i] -= b.data[i];
            return c;
        }
        public static Matrix2D operator -(Matrix2D a, float b)
        {
            Matrix2D c = new Matrix2D(a);

            for (int i = 0; i < c.data.Length; i++)
                c.data[i] -= b;

            return c;
        }
        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        {
            if (a.columns != b.rows)
                throw new ArgumentException("The width of matrix A must be equal to the height of matrix B to perform multiplication.");

            Matrix2D c = new Matrix2D(a.rows, b.columns, 0);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    for (int k = 0; k < a.columns; k++)
                        c[i, j] += a[i, k] * b[k, j];

            return c;
        }
        public static Matrix2D operator *(Matrix2D a, float b)
        {
            Matrix2D c = new Matrix2D(a);
            for (int i = 0; i < c.data.Length; i++)
                c.data[i] *= b;
            return c;
        }

        public Matrix2D GetRow(int row)
        {
            if (row >= rows)
                throw new ArgumentException("The row doesn't exist in the given matrix.");

            Matrix2D c = new Matrix2D(1, columns);

            for (int i = 0; i < columns; i++)
                c[0, i] = this[row, i];
            return c;
        }
        public Matrix2D GetColumn(int column)
        {
            if (column >= columns)
                throw new ArgumentException("The column doesn't exist in the given matrix.");

            Matrix2D c = new Matrix2D(rows, 1);

            for (int i = 0; i < columns; i++)
                c[i, 0] = this[i, column];
            return c;
        }
        public Matrix2D GetSubSection(int startRow, int startColumn, int rowCount = int.MaxValue, int columnCount = int.MaxValue)
        {
            Matrix2D c = new Matrix2D(
                Math.Min(this.rows - startRow, rowCount), 
                Math.Min(this.columns - startColumn, columnCount));

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (i + startRow < this.rows && j + startColumn < this.columns)
                        c[i, j] = this[i + startRow, j + startColumn];
                    else
                        c[i, j] = 0;

            return c;
        }

        public void InsertRow(int index = int.MaxValue, float initValue = 0)
        {
            Matrix2D c = new Matrix2D(rows + 1, columns);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (i >= index)
                        c[i + 1, j] = this[i, j];
                    else
                        c[i, j] = this[i, j];

            if (initValue != 0)
                for (int i = 0; i < c.columns; i++)
                    c[index == int.MaxValue ? c.rows - 1 : index, i] = initValue;

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void InsertColumn(int index = int.MaxValue, float initValue = 0)
        {
            Matrix2D c = new Matrix2D(rows, columns + 1);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (j >= index)
                        c[i, j + 1] = this[i, j];
                    else
                        c[i, j] = this[i, j];

            if (initValue != 0)
                for (int i = 0; i < c.rows; i++)
                    c[i, index == int.MaxValue ? c.columns - 1 : index] = initValue;

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void DeleteRow(int index)
        {
            Matrix2D c = new Matrix2D(rows - 1, columns);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (i >= index)
                        c[i, j] = this[i + 1, j];
                    else
                        c[i, j] = this[i, j];

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void DeleteRows(int start, int count)
        {
            Matrix2D c = new Matrix2D(rows - count, columns);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (i >= start)
                        c[i, j] = this[i + count, j];
                    else
                        c[i, j] = this[i, j];

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void DeleteColumn(int index)
        {
            Matrix2D c = new Matrix2D(rows, columns - 1);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (j >= index)
                        c[i, j] = this[i, j + 1];
                    else
                        c[i, j] = this[i, j];

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void DeleteColumns(int start, int count)
        {
            Matrix2D c = new Matrix2D(rows, columns - count);

            for (int i = 0; i < c.rows; i++)
                for (int j = 0; j < c.columns; j++)
                    if (j >= start)
                        c[i, j] = this[i, j + count];
                    else
                        c[i, j] = this[i, j];

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }
        public void MakeDoubleWide(bool fillWithIdentity = true)
        {
            Matrix2D c = new Matrix2D(rows, columns * 2);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    c[i, j] = this[i, j];

            if (fillWithIdentity)
            {
                Matrix2D id = CreateIdentity(rows);

                for (int i = 0; i < id.rows; i++)
                    for (int j = 0; j < id.columns; j++)
                        c[i, j + columns] = id[i, j];
            }

            this.data = c.data;
            this.rows = c.rows;
            this.columns = c.columns;
        }

        public void MultiplyRow(int row, float f)
        {
            if (row >= rows)
                throw new ArgumentException("The row doesn't exist in the given matrix.");

            for (int i = 0; i < columns; i++)
                this[row, i] *= f;
        }
        public void AddRows(int source, int target, float premultiply = 1)
        {
            if (source >= rows)
                throw new ArgumentException("The source row doesn't exist in the given matrix.");
            if (target >= rows)
                throw new ArgumentException("The target row doesn't exist in the given matrix.");

            for (int i = 0; i < columns; i++)
                this[target, i] += this[source, i] * premultiply;
        }
        public void SwapRows(int r1, int r2)
        {
            if (r1 >= rows)
                throw new ArgumentException("The source row doesn't exist in the given matrix.");
            if (r2 >= rows)
                throw new ArgumentException("The target row doesn't exist in the given matrix.");

            float buf;
            for (int i = 0; i < columns; i++)
            {
                buf = this[r1, i];
                this[r1, i] = this[r2, i];
                this[r2, i] = buf;
            }
        }

        public float Determinant()
        {
            if (!IsSquare())
                throw new NotSupportedException("This method is defined only for square matrices.");

            float d = 0;

            if (rows == 1)
            {
                d = this[0, 0];
            }
            else if (rows == 2)
            {
                d = this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            else
            {
                for (int i = 0; i < this.columns; i++)
                {
                    Matrix2D sub = this.GetSubSection(1, 0); 
                    sub.DeleteColumn(i);

                    if (i % 2 == 0)
                        d += this[0, i] * sub.Determinant();
                    else
                        d -= this[0, i] * sub.Determinant();
                }
            }

            return d;
        }
        public Matrix2D PerformGaussJordanElimination() 
        {
            Matrix2D c = new Matrix2D(this);

            Queue<Vector2> diagPoints = new Queue<Vector2>();
            Queue<Vector2> zeroPoints = new Queue<Vector2>();

            for (int i = 0; i < c.rows; i++)
                diagPoints.Enqueue(new Vector2(i, i));

            while (diagPoints.Count > 0)
            {
                Vector2 diag = diagPoints.Dequeue();

                c.MultiplyRow(diag.X, 1 / c[diag.X, diag.Y]);

                for (int i = 0; i < c.rows; i++)
                    if (i != diag.X)
                        zeroPoints.Enqueue(new Vector2(i, diag.Y));

                while (zeroPoints.Count > 0)
                {
                    Vector2 zero = zeroPoints.Dequeue();

                    c.AddRows(diag.X, zero.X, c[diag.X, diag.Y] * -c[zero.X, zero.Y]);
                }
            }

            return c;
        }
        public Matrix2D Inverse()
        {
            if (rows != columns)
                throw new NotSupportedException("This method is only defined for square matrices.");

            Matrix2D c = new Matrix2D(this);
            c.MakeDoubleWide();

            c = c.PerformGaussJordanElimination();

            if (!c.GetSubSection(0, 0, c.rows, c.rows).IsIdentity())
                return null;

            //c.DeleteColumns(0, c.rows);
            return c.GetSubSection(0, c.rows, c.rows, c.rows);
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix2D)
                return this == (Matrix2D)obj;
            return false;
        }
        public bool Equals(Matrix2D obj)
        {
            return this == obj;
        }

        public Vector2F ToVector2F()
        {
            if (data.Length != 2)
                throw new ArrayTypeMismatchException("The matrix must have exactly two members in order to be converted to a vector.");

            return new Vector2F(data[0], data[1]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("\n\t[");
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i]);

                if (i % columns == columns - 1) 
                {
                    sb.Append("]\n");
                    if (i + 1 < data.Length)
                    {
                        sb.Append("\t[");
                    }
                }
                else
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rows);
            sb.Append(columns);

            for (int i = 0; i < rows; i++)
            {
                float total = 0;
                for (int j = 0; j < columns; j++)
                {
                    total *= this[i, j];
                }
                total = (float)Math.Round(total, 1);
                sb.Append(total * 10);
            }

            return (int)long.Parse(sb.ToString());
        }

        #region Unit Test
        public static string RunUnitTest()
        {
            StringBuilder output = new StringBuilder();
            Matrix2D a, b, rm, expected;
            float rf;

            #region Accessing
            a = new Matrix2D(2, 5, new float[]
                {
                    0, 1, 2, 3, 4,
                    5, 6, 7, 8, 9
                });

            try
            {
                if (a[0, 0] != 0
                        || a[0, 1] != 1
                        || a[0, 2] != 2
                        || a[0, 3] != 3
                        || a[0, 4] != 4
                        || a[1, 0] != 5
                        || a[1, 1] != 6
                        || a[1, 2] != 7
                        || a[1, 3] != 8
                        || a[1, 4] != 9)
                    output.AppendLine("Matrix Access test failed.");
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Access test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Addition
            a = new Matrix2D(2, 2, new float[]
                {
                    1.0f, 2.0f,
                    3.0f, -4.0f
                });
            b = new Matrix2D(2, 2, new float[]
                {
                    2.0f, 4.0f,
                    -2.0f, 1.0f
                });

            try
            {
                rm = a + b;
                expected = new Matrix2D(2, 2, new float[] { 3, 6, 1, -3 });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Matrix Addition test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Matrix Addition test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = a + 2.0f;
                expected = new Matrix2D(2, 2, new float[] { 3, 4, 5, -2 });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Scalar Addition test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Scalar Addition test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Subtraction
            a = new Matrix2D(2, 2, new float[]
                {
                    1.0f, 2.0f,
                    3.0f, -4.0f
                });
            b = new Matrix2D(2, 2, new float[]
                {
                    2.0f, 4.0f,
                    -2.0f, 1.0f
                });

            try 
            {
                rm = a - b;
                expected = new Matrix2D(2, 2, new float[] { -1, -2, 5, -5 });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Matrix Subtraction test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Matrix Subtraction test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = a - 2.0f;
                expected = new Matrix2D(2, 2, new float[] { -1, 0, 1, -6 });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Scalar Subtraction test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Scalar Subtraction test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Multiplication
            a = new Matrix2D(2, 3, new float[]
                {
                    2.5f, 3.0f, 1.5f,
                    1.0f, 2.0f, 4.0f
                });
            b = new Matrix2D(3, 2, new float[]
                {
                    1.0f, 2.5f,
                    1.0f, 3.0f,
                    4.0f, 1.5f
                });

            try
            {
                rm = a * b;
                expected = new Matrix2D(2, 2, new float[] { 11.5f, 17.5f, 19, 14.5f });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Matrix Multiplication test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Matrix Multiplication test threw an error:\n\t" + ex.Message);
            }

            try {
                rm = a * 2.0f;
                expected = new Matrix2D(2, 3, new float[] { 5, 6, 3, 2, 4, 8 });

                if (rm != expected)
                {
                    output.AppendLine("Matrix-to-Scalar Multiplication test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix-to-Scalar Multiplication test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Identity
            try { 
                rm = CreateIdentity(2);
                expected = new Matrix2D(2, 2, new float[] { 1, 0, 0, 1 });

                if (rm != expected)
                {
                    output.AppendLine("2nd Dimension Identity Generation test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("2nd Dimension Identity Generation test threw an error:\n\t" + ex.Message);
            }

            try {
                rm = CreateIdentity(4);
                expected = new Matrix2D(4, 4, new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 });
                if (rm != expected)
                {
                    output.AppendLine("4th Dimension Identity Generation test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("4th Dimension Identity Generation test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Row Operations
            a = new Matrix2D(3, 3, new float[]
                {
                    2.5f, 3.0f, 1.5f,
                    1.0f, 2.0f, 4.0f,
                    2.0f, -1.0f, 3.0f
                });

            try
            {
                rm = AddRows(a, 1, 2);
                expected = new Matrix2D(3, 3, new float[] 
                    {
                        2.5f, 3.0f, 1.5f,
                        1.0f, 2.0f, 4.0f,
                        3.0f, 1.0f, 7.0f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Row Addition test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Row Addition test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = MultiplyRow(a, 1, 2);
                expected = new Matrix2D(3, 3, new float[] 
                    {
                        2.5f, 3.0f, 1.5f,
                        2.0f, 4.0f, 8.0f,
                        2.0f, -1.0f, 3.0f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Row Multiplication test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Row Multiplication test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = SwapRows(a, 0, 2);
                expected = new Matrix2D(3, 3, new float[] 
                    {
                        2.0f, -1.0f, 3.0f,
                        1.0f, 2.0f, 4.0f,
                        2.5f, 3.0f, 1.5f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Row Swap test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Row Swap test threw an error:\n\t" + ex.Message);
            }

            #endregion

            #region Insert Operations
            a = new Matrix2D(2, 2, new float[]
                {
                    2.5f, 3.0f,
                    1.0f, 2.0f
                });

            try
            {
                rm = InsertRow(a, 1, 2);
                expected = new Matrix2D(3, 2, new float[] 
                    {
                        2.5f, 3.0f,
                        2, 2,
                        1.0f, 2.0f,
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Row Insertion test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Row Insertion test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = InsertColumn(a, 1, 2);
                expected = new Matrix2D(2, 3, new float[] 
                    {
                        2.5f, 2, 3.0f,
                        1.0f, 2, 2.0f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Column Insertion test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Column Insertion test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Delete Operations
            a = new Matrix2D(2, 2, new float[]
                {
                    2.5f, 3.0f,
                    1.0f, 2.0f
                });

            try
            {
                rm = DeleteRow(a, 1);
                expected = new Matrix2D(1, 2, new float[] 
                    {
                        2.5f, 3.0f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Row Deletion test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Row Deletion test threw an error:\n\t" + ex.Message);
            }

            try
            {
                rm = DeleteColumn(a, 1);
                expected = new Matrix2D(2, 1, new float[] 
                    {
                        2.5f,
                        1.0f
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Column Deletion test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Column Deletion test threw an error:\n\t" + ex.Message);
            }
            #endregion

            #region Higler Level Operations
            a = new Matrix2D(3, 3, new float[] 
                {
                    6, 1, 1,
                    4, -2, 5,
                    2, 8, 7
                });

            try
            {
                rf = Determinant(a);

                if (rf != -306)
                {
                    output.AppendLine("Matrix Determinant test failed.");
                    output.AppendLine("\tExpected: " + -306);
                    output.AppendLine("\tReceived: " + rf);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Determinant test threw an error:\n\t" + ex.Message);
            }

            //a = new Matrix2D(3, 3, new float[] 
            //    {
            //        1, 3, 3,
            //        1, 4, 3,
            //        1, 3, 4
            //    });

            //try
            //{
            //    rm = a.PerformGaussJordanElimination();
            //    expected = new Matrix2D(6, 6, new float[] 
            //        {
            //            1, 0, 0, 7, -3, -3,
            //            0, 1, 0, -1, 1, 0,
            //            0, 0, 1, -1, 0, 1
            //        });

            //    if (rm != expected)
            //    {
            //        output.AppendLine("Matrix Gauss-Jordan Elimination test failed.");
            //        output.AppendLine("\tExpected: " + expected);
            //        output.AppendLine("\tReceived: " + rm);
            //        output.AppendLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    output.AppendLine("Matrix Gauss-Jordan Elimination test threw an error:\n\t" + ex.Message);
            //}

            a = new Matrix2D(3, 3, new float[] 
                {
                    1, 3, 3,
                    1, 4, 3,
                    1, 3, 4
                });

            try
            {
                rm = a.Inverse();
                expected = new Matrix2D(3, 3, new float[] 
                    {
                        7, -3, -3,
                        -1, 1, 0,
                        -1, 0, 1
                    });

                if (rm != expected)
                {
                    output.AppendLine("Matrix Inverse test failed.");
                    output.AppendLine("\tExpected: " + expected);
                    output.AppendLine("\tReceived: " + rm);
                    output.AppendLine();
                }
            }
            catch (Exception ex)
            {
                output.AppendLine("Matrix Inverse test threw an error:\n\t" + ex.Message);
            }
            #endregion

            return output.ToString();
        }
        #endregion
    }
}

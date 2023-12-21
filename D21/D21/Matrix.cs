using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExtendedNumerics;


namespace D21
{
    public class Matrix
    {
        BigDecimal[,] mat;
        public int n => mat.GetLength(0);
        public int m => mat.GetLength(1);
        public int Rows => n;
        public int Columns => m;
        public BigDecimal this[int i, int j] { get => mat[i, j]; set => mat[i, j] = value; }
        public Matrix(int n) : this(n, n)
        {

        }

        public Matrix(int m, int n) 
        {
            mat = new BigDecimal[m, n];
        }
        public Matrix Inverse()
        {
            BigDecimal det = Determinant();
            if (det == 0)
                throw new InvalidOperationException();
            return Adjugate().Multiply(1 / det);
        }
        public Matrix Adjugate()
        {
            Matrix adjugate = new Matrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BigDecimal sign = ((i + j) % 2 == 0) ? 1 : -1;
                    BigDecimal cofactor = sign * Submatrix(j, i).Determinant();

                    adjugate[i, j] = cofactor;
                }
            }

            return adjugate;
        }
        public Matrix Multiply(BigDecimal constant)
        {
            Matrix r = new Matrix(n, m);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    r[i, j] = constant * this[i, j];
            return r;
        }
        public Matrix Multiply(Matrix other) 
        {
            if(this.m != other.n)
                throw new InvalidOperationException();
            Matrix r = new Matrix(n, other.m);

            for(int i = 0; i < r.n; i++)
                for(int j = 0; j < r.m; j++)
                    for(int k = 0; k < n; k++)
                        r[i, j] += this[i, k] * other[k, j];
            return r;
        }
        public BigDecimal Determinant()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Determinant can only be calculated for square matrices.");

            if (Rows == 1)
                return mat[0, 0];

            BigDecimal det = 0;

            for (int j = 0; j < Columns; j++)
                det += (j % 2 == 0 ? 1 : -1) * mat[0, j] * Submatrix(0, j).Determinant();

            return det;
        }
        private Matrix Submatrix(int rowToRemove, int colToRemove)
        {
            Matrix submatrix = new Matrix(Rows - 1, Columns - 1);

            int rowIndex = 0;
            for (int i = 0; i < Rows; i++)
            {
                if (i == rowToRemove)
                    continue;

                int colIndex = 0;
                for (int j = 0; j < Columns; j++)
                {
                    if (j == colToRemove)
                        continue;

                    submatrix[rowIndex, colIndex] = mat[i, j];
                    colIndex++;
                }

                rowIndex++;
            }

            return submatrix;
        }
        public Matrix Transpose()
        {
            Matrix r = new Matrix(n, m);
            for(int i = 0; i < n; i++)
                for(int j = 0; j < m; j++)
                    r[i, j] = this[j, i];

            return r;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    sb.Append(this[i, j]);
                    sb.Append(' ');
                }            
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();        
        }

    }
}

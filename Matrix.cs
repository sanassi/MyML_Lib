using System;

namespace MyML_Lib
{
    public class Matrix<T>
    {
        private int Row { get; }
        private int Col { get; }

        private Tuple<int, int> _shape;

        public T[] Data;

        /*------------------Constructors----------------*/

        public Matrix(int row, int col)
        {
            this.Row = row;
            this.Col = col;
            this._shape = new Tuple<int, int>(row, col);
            Data = new T[row * col];
        }
        
        public Matrix(T[] arr, int row, int col)
        {
            this.Data = arr;
            this.Row = row;
            this.Col = col;
            this._shape = new Tuple<int, int>(row, col);
        }

        public T this[int i, int j]
        {
            get => Data[i * this.Col + j]; 
            set => Data[i * this.Col + j] = value; 
        }
        
        public Matrix(int row, int col, T val)
        {
            this.Row = row;
            this.Col = col;
            this._shape = new Tuple<int, int>(row, col);
            Data = new T[row * col];
            for (int i = 0; i < row * col; i++)
            {
                Data[i] = val;
            }
        }

        public Matrix(int row, int col, double min, double max)
        {
            this.Data = new T[row * col];
            this.Row = row;
            this.Col = col;

            for (int i = 0; i < row * col; i++)
            {
                dynamic val = GetRandomNumber(min, max);
                Data[i] = val;
            }
        }
        /*
        public static Matrix<double> RandomMat(int row, int col, int min, int max)
        {
            Matrix<double> res = new Matrix<double>(row, col);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    res[i, j] = GetRandomNumber(min, max);
                }
            }

            return res;
        }
        */

        /*getter setter*/

        public Tuple<int, int> Shape()
        {
            return this._shape;
        }

        /*--------------------------------------------------*/

        /*Misc Functions*/
        
        /*-------------------Operations---------------------*/
        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            if (a.Row != b.Row || b.Col != a.Col)
            {
                throw new ArgumentException("Matrix Addition : dimensions are not the same");
            }

            Matrix<T> res = new Matrix<T>(a.Row, a.Col);
            
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < b.Col; j++)
                {
                    dynamic dx = a[i, j], dy = b[i, j];
                    res[i, j] = dx + dy;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            if (a.Row != b.Row || b.Col != a.Col)
            {
                throw new ArgumentException("Matrix Addition : dimensions are not the same");
            }

            Matrix<T> res = new Matrix<T>(a.Row, a.Col);
            
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < b.Col; j++)
                {
                    dynamic dx = a[i, j], dy = b[i, j];
                    res[i, j] = dx - dy;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator +(Matrix<T> a, double x)
        {
            Matrix<T> res = new Matrix<T>(a.Row, a.Col);

            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    dynamic val = a[i, j];
                    res[i, j] = val + x;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator +(double x, Matrix<T> a)
        {

            Matrix<T> res = new Matrix<T>(a.Row, a.Col);

            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    dynamic val = a[i, j];
                    res[i, j] = val + x;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator -(Matrix<T> a, double x) => a + (-x);
        
        public static Matrix<T> operator -(double x, Matrix<T> a) => x + (-a); // so the operation is commutative

        public static Matrix<T> operator /(Matrix<T> a, double x) => a * (1 / x);
        
        public static Matrix<T> operator /(double x, Matrix<T> a) => a * (1 / x);

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Col != b.Row)
            {
                throw new ArgumentException("Error : Multiply Matrix Invalid Dimensions");
            }

            Matrix<T> res = new Matrix<T>(a.Row, b.Col);

            for (int i = 0; i < res.Row; i++)
            {
                for (int j = 0; j < res.Col; j++)
                {
                    dynamic sum = 0;

                    for (int k = 0; k < b.Row; k++)
                    {
                        dynamic dx = a[i, k];
                        dynamic dy = b[k, j];
                        sum += dx * dy;
                    }

                    res[i, j] = sum;
                }
            }

            return res;
        }

        public static Matrix<T> operator *(Matrix<T> a, double x) // A * x (x real number)
        {
            Matrix<T> res = new Matrix<T>(a.Row, a.Col);
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < res.Col; j++)
                {
                    dynamic dx = a[i, j];
                    res[i, j] =dx * x;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator *(double x, Matrix<T> a) // so the operator is commutative (a * x = x * a)
        {
            Matrix<T> res = new Matrix<T>(a.Row, a.Col);
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < res.Col; j++)
                {
                    dynamic dx = a[i, j];
                    res[i, j] = dx * x;
                }
            }

            return res;
        }
        
        public static Matrix<T> operator -(Matrix<T> a) => (-1) * a;
        public static Matrix<T> operator +(Matrix<T> a) => a;
        
        public static Matrix<T> operator ~(Matrix<T> a) // ~ : transpose operator
        {
            Matrix<T> res = new Matrix<T>(a.Col, a.Row);

            for (int i = 0; i < res.Row; i++)
            {
                for (int j = 0; j < res.Col; j++)
                {
                    res[i, j] = a[j, i];
                }
            }

            return res;
        }
        
        public static Matrix<T> operator ^(Matrix<T> a, Matrix<T> b) // hadamard product : element-wise matrix multiplication
        {
            if (a.Row != b.Row || a.Col != b.Col)
            {
                throw new ArgumentException("Hadamard Product : Invalid matrix dimensions");
            }

            Matrix<T> res = new Matrix<T>(a.Row, b.Col);

            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    dynamic dx = a[i, j], dy = b[i, j];
                    res[i, j] = dx * dy;
                }
            }

            return res;
        }
        
        public static bool operator ==(Matrix<T> a, Matrix<T> b)
        {
            if (a.Row != b.Row || a.Col != b.Col)
                return false;

            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < b.Col; j++)
                {
                    dynamic dx = a[i, j], dy = b[i, j];
                    if (dx != dy)
                        return false;
                }
            }

            return true;
        }
        
        public static bool operator !=(Matrix<T> a, Matrix<T> b)
        {
            return !(a == b);
        }

        /*--------------------------------------------------*/

        public Matrix<T> GetRow(int row)
        {
            if (row >= this.Row)
            {
                throw new Exception("GetRow() : argument too large");
            }
            Matrix<T> res = new Matrix<T>(1, this.Col);

            for (int i = 0; i < this.Col; i++)
            {
                res[0, i] = this[row, i];
            }
            
            return res;
        }

        public Matrix<T> GetCol(int col)
        {
            if (col >= this.Col)
            {
                throw new Exception("GetCol() : argument too large");
            }
            Matrix<T> res = new Matrix<T>(this.Row, 1);

            for (int i = 0; i < this.Row; i++)
            {
                res[i, 0] = this[i, col];
            }
            
            return res;
        }
        
        /*--------------------------------------------------*/
        /*---------------------Misc-------------------------*/

        public static double GetRandomNumber(double minimum, double maximum)
        { 
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        
        public static Matrix<T> Identity(int size)
        {
            Matrix<T> id = new Matrix<T>(size, size);
            
            for (int i = 0; i < size; i++)
            {
                dynamic val = 0;
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        val = 1;
                    
                    id[i, j] = val;
                    val = 0;
                }
            }

            return id;
        }

        public static Matrix<T> Map(Matrix<T> a, Func<T, T> func)
        {
            Matrix<T> res = new Matrix<T>(a.Row, a.Col);
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    res[i, j] = func(a[i, j]);
                }
            }

            return res;
        }

        public static Matrix<double> Map(Matrix<double> a, Func<double, double> func)
        {
            Matrix<double> res = new Matrix<double>(a.Row, a.Col);
            
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    res[i, j] = func(a[i, j]);
                }
            }

            return res;
        }

        public static Matrix<double> Sigmoid(Matrix<double> a)
        {
            return Map(a, x => 1 / (1 + Math.Exp(-x)));
        }

        public static Matrix<double> SigmoidPrime(Matrix<double> a)
        {
            return a ^ (1 - a);
            //return Sigmoid(a) ^ (1 - Sigmoid(a));
        }

        public void Print()
        {
            Console.WriteLine();
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    Console.Write(this[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static double Sum(Matrix<double> a)
        {
            double sum = 0;

            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    sum += a[i, j];
                }
            }

            return sum;
        }
        
        public static Tuple<int, int> ArgMax(Matrix<double> m)
        {
            int iMax = 0, jMax = 0;

            for (int i = 0; i < m.Row; i++)
            {
                for (int j = 0; j < m.Col; j++)
                {
                    if (m[i, j] > m[iMax, jMax])
                    {
                        iMax = i;
                        jMax = j;
                    }
                }
            }

            return new Tuple<int, int>(iMax, jMax);
        }

    }
}